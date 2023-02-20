using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class RiskControlAlert : UIElement {
        public const int CONTROL_DONE_CODE = 0;
        public const int CLOSE_CODE = -1;

        private Button closeButton;

        private CanvasWebViewPrefab canvasWebViewPrefab;

        public static async Task<int> Control(Func<string, Task<RequestVerifyCodeData>> requestVerifyCodeFunc) {
            RiskControlAlert alert = null;
            string riskControlData = null;
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            try {
                UIManager.ShowLoading();
                RequestVerifyCodeData verifyCodeData = await requestVerifyCodeFunc(riskControlData);
                tcs.TrySetResult(verifyCodeData.ReSendInterval);
            } catch (XDException e) {
                if (e.code == PhoneModule.RISK_CONTROL) {
                    // 触发风控
                    alert = UIManager.ShowUI<RiskControlAlert>("XDRiskControlAlert", null, async (code, data) => {
                        if (code == CLOSE_CODE) {
                            UIManager.Dismiss();
                            tcs.TrySetCanceled();
                        } else if (code == CONTROL_DONE_CODE) {
                            riskControlData = data as string;
                            try {
                                UIManager.ShowLoading();
                                RequestVerifyCodeData result = await requestVerifyCodeFunc(riskControlData);
                                tcs.TrySetResult(result.ReSendInterval);
                            } catch (XDException ex) {
                                UIManager.DismissLoading();
                                if (ex.code == PhoneModule.RISK_CONTROL) {
                                    // 触发风控
                                    await alert.Reload();
                                } else if (ex.code == PhoneModule.RISK_CONTROL_FAILED) {
                                    // 滑动验证失败
                                    await alert.Reload();
                                    UIManager.ShowToast(e.Message);
                                } else {
                                    tcs.TrySetException(ex);
                                }
                            } finally {
                                UIManager.DismissLoading();
                            }
                        }
                    });
                } else {
                    tcs.TrySetException(e);
                }
            } catch (Exception e) {
                tcs.TrySetException(e);
            } finally {
                UIManager.DismissLoading();
            }

            return await tcs.Task;
        }

        private void Awake() {
            Transform webviewContainer = transform.Find("WebViewContainer");
            canvasWebViewPrefab = CanvasWebViewPrefab.Instantiate();
            canvasWebViewPrefab.transform.SetParent(webviewContainer);
            RectTransform rectTransform = canvasWebViewPrefab.GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            closeButton = transform.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(OnCloseClicked);
        }

        private async void Start() {
            UIManager.ShowLoading();

            await canvasWebViewPrefab.WaitUntilInitialized();

            canvasWebViewPrefab.DragMode = DragMode.DragWithinPage;

            canvasWebViewPrefab.WebView.MessageEmitted += (sender, eventArgs) => {
                XDLogger.Debug(eventArgs.Value);
                Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventArgs.Value);
                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["message"]);
                string riskControlData = JsonConvert.SerializeObject(data["data"]);
                XDLogger.Debug(riskControlData);
                UIManager.Dismiss();

                OnCallback(CONTROL_DONE_CODE, riskControlData);
            };

            await Reload();
        }

        private string GetIP()
        {
            string output = "";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
                NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

                if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        //IPv4
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                            XDLogger.Debug($"ip local: {output}");
                        }
                    }
                }
            }
            return output;
        }
        private async Task Reload() {
            UIManager.ShowLoading();
            var ip = GetIP();
            canvasWebViewPrefab.WebView.LoadUrl($"https://login-xdsdk.xd.cn/awsc?platform={Platform}&eventSessionId={AliyunTrack.LoginEventSessionId}&login_type=Phone&sub_login_type=2&xd_client_id={ConfigModule.ClientId}&device_id={SystemInfo.deviceUniqueIdentifier}&os={SystemInfo.operatingSystem}&ip={ip}&app_version={Application.version}&language={Localization.CurrentLang.ToString()}");
            await canvasWebViewPrefab.WebView.WaitForNextPageLoadToFinish();
            // 注入 js
            _ = canvasWebViewPrefab.WebView.ExecuteJavaScript(@";(function (window) {
              window._xd_jsbridge = {
                invoke: function (json) {
                  try {
                    window.vuplex.postMessage({ type: 'verify', message: json });
                  } catch (error) {
                    console.error('xd-jsbridge error::', error)
                  }
                },
              };
            })(window);");

            UIManager.DismissLoading();
        }

        private string Platform {
            get {
                switch (SystemInfo.operatingSystemFamily) {
                    case OperatingSystemFamily.MacOSX:
                        return "Mac";
                    case OperatingSystemFamily.Windows:
                        return "Windows";
                    case OperatingSystemFamily.Linux:
                        return "Linux";
                    default:
                        return "UnKnown";
                }
            }
        }

        #region UI Events

        private void OnCloseClicked() {
            OnCallback(CLOSE_CODE, null);
        }

        #endregion
    }
}
