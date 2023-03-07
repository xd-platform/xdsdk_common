#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class VerifyCodeInputAlert : UIElement {
        public const int INPUT_DONE_CODE = 0;
        public const int CLOSE_CODE = -1;
        public const int RESEND_CODE = 1;
        public const int BACK_CODE = 2;

        private const int VERIFY_CODE_LENGTH = 6;

        public Action OnBack { get; set; }
        public Action<string, string, string> OnVerify { get; set; }

        private Button backButton;
        private Button closeButton;
        private Text titleText;
        private Text phoneNumberTipsText;
        private InputField codeInput;
        private Text errorCodeTipsText;
        private Button reSendButton;

        public static async Task Verify(string phoneNumber,
            Func<string, Task<RequestVerifyCodeData>> requestVerifyCodeFunc,
            Func<string, Task> verifyFunc) {

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            int resendInterval;
            try {
                // 通过风控
                resendInterval = await RiskControlAlert.Control(requestVerifyCodeFunc);

                // 输入验证码
                Dictionary<string, object> conf = new Dictionary<string, object> {
                    { "phoneNumber", phoneNumber },
                    { "reSendInterval", resendInterval }
                };
                VerifyCodeInputAlert alert = null;

                AliyunTrack.LoginMobileVerifyCodeInput();
                alert = UIManager.ShowUI<VerifyCodeInputAlert>("XDVerifyCodeInputAlert", conf, async (code, data) => {
                    if (code == CLOSE_CODE) {
                        tcs.TrySetCanceled();
                        UIManager.Dismiss();
                    } else if (code == BACK_CODE) {
                        UIManager.Dismiss();
                    } else if (code == RESEND_CODE) {
                        try {
                            resendInterval = await RiskControlAlert.Control(requestVerifyCodeFunc);
                            alert.Extra["reSendInterval"] = resendInterval;
                            alert.Start();
                            AliyunTrack.LoginMobileVerifyCodeInput();
                        } catch (TaskCanceledException) {
                            tcs.TrySetCanceled();
                            UIManager.Dismiss();
                        }
                    } else if (code == INPUT_DONE_CODE) {

                        try {
                            await verifyFunc(data as string);
                            tcs.TrySetResult(null);
                            UIManager.Dismiss();
                        } catch (XDException e) {
                            if (e.code == PhoneModule.INVALID_VERIFY_CODE) {
                                alert.VerifyFailed();
                                AliyunTrack.LoginMobileVerifyCodePassFail(e.error_msg);
                            } else {
                                AliyunTrack.LoginMobileVerifyCodePassFail(e.error_msg);
                                tcs.TrySetException(e);
                                UIManager.Dismiss();
                            }
                        } catch (Exception e) {
                            tcs.TrySetException(e);
                            UIManager.Dismiss();
                        }
                    }
                });
            } catch (TaskCanceledException) {
                tcs.TrySetCanceled();
            }
            
            await tcs.Task;
        }

        private void Awake() {
            backButton = transform.Find("Alert/BackButton").GetComponent<Button>();
            closeButton = transform.Find("Alert/CloseButton").GetComponent<Button>();
            titleText = transform.Find("Alert/TitleText").GetComponent<Text>();
            phoneNumberTipsText = transform.Find("Alert/PhoneNumberTips").GetComponent<Text>();
            codeInput = transform.Find("Alert/Input/CodeInput").GetComponent<InputField>();
            errorCodeTipsText = transform.Find("Alert/ErrorCodeTips").GetComponent<Text>();
            reSendButton = transform.Find("Alert/ReSendButton").GetComponent<Button>();

            errorCodeTipsText.gameObject.SetActive(false);

            closeButton.onClick.AddListener(OnCloseClicked);
            backButton.onClick.AddListener(OnVerifyDialogBackClicked);
            codeInput.onValueChanged.AddListener(OnCodeInputValueChanged);
            reSendButton.onClick.AddListener(OnReSendClicked);
        }

        private void Start()
        {
            phoneNumberTipsText.text = $"已发送至 {Extra["phoneNumber"]}";
            // 倒计时
            StartCoroutine(UpdateResendInterval());
            codeInput.Select();
        }

        IEnumerator UpdateResendInterval() {
            int resendInterval = (int) Extra["reSendInterval"];
            while (resendInterval > 0) {
                reSendButton.interactable = false;
                Text reSendText = reSendButton.GetComponentInChildren<Text>();
                reSendText.text = $"{resendInterval} 秒后可重新发送";
                yield return new WaitForSeconds(1);
                resendInterval--;
            }
            reSendButton.interactable = true;
            reSendButton.GetComponentInChildren<Text>().text = "重新发送";
        }

        public void VerifyFailed() {
            errorCodeTipsText.gameObject.SetActive(true);
        }

        #region UI Events

        private void OnCloseClicked() {
            OnCallback(CLOSE_CODE, null);
        }

        private void OnVerifyDialogBackClicked() {
            OnCallback(BACK_CODE, null);
        }

        private void OnCodeInputValueChanged(string code) {
            errorCodeTipsText.gameObject.SetActive(false);

            if (code?.Length != VERIFY_CODE_LENGTH) {
                return;
            }

            // 请求校验验证码
            OnCallback(INPUT_DONE_CODE, code);
        }

        private void OnReSendClicked() {
            OnCallback(RESEND_CODE, null);
        }

        #endregion
    }
}
#endif