#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    internal class PayListener
    {
        private const float TIMEOUT = 0.25f;
        private float _timeLeft = 0;
        internal async Task Start(string url) {
            string redirectUri = $"http://{IPAddress.Loopback}:{LocalServerUtils.GetRandomUnusedPort()}";
            string state = LocalServerUtils.GenerateRandomDataBase64url(32);
            string payRequest = $"{url}&redirect_uri={redirectUri}&state={state}";

            XDLogger.Debug($"pay: {payRequest}");
            _timeLeft = TIMEOUT;
            UIManager.ShowLoading(true);
            Application.OpenURL(Uri.EscapeUriString(payRequest));
            AliyunTrack.PaymentCallPage();
            // 启动监听
            HttpListener server = new HttpListener();
            server.Prefixes.Add($"{redirectUri}/");
            server.Start();

            while (true) {
                UIManager.Instance.DelayAction(TIMEOUT, () =>
                {
                    CloseLoading();
                });
                HttpListenerContext context = await server.GetContextAsync();

                Debug.Log($"Request method: {context.Request.HttpMethod}");

                context.Response.StatusCode = 200;
                context.Response.Close();

                if (context.Request.HttpMethod == "OPTIONS") {
                    continue;
                }

                server.Stop();

                XDLogger.Debug($"pay callback: {context.Request.RawUrl}");

                WindowUtils.BringToFront();

                string result = context.Request.QueryString["pay_result"];
                if (result == "success")
                {
                    CloseLoading();
                    AliyunTrack.PaymentDone();
                    return;
                } else if (result == "fail") {
                    CloseLoading();
                    AliyunTrack.PaymentFailed("fail");
                    throw new Exception("pay failed");
                } else if (result == "cancel") {
                    CloseLoading();
                    AliyunTrack.PaymentFailed("user_cancel");
                    throw new TaskCanceledException();
                }
            }
        }

        private void CloseLoading()
        {
            UIManager.DismissLoading();
            _timeLeft = 0;
        }
    }
}
#endif