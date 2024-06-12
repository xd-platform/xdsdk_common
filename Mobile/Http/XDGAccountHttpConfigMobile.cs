using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XD.SDK.Account.Internal {
    public class XDGAccountHttpConfigMobile : IXDAccountHttpConfig {
        public Task<Dictionary<string, string>> GetAuthorizationHeaders(HttpRequestMessage request, long timestamp) {
            string url = request.RequestUri.AbsoluteUri;
            string method = request.Method.ToString().ToUpper();
            string authorization;
#if UNITY_ANDROID
            using (AndroidJavaClass AccountBridgeClass = new AndroidJavaClass("com.xd.account.bridge.XDGAccountBridge")) {
                authorization = AccountBridgeClass.CallStatic<string>("getAuthorization", url, method, timestamp);
            }
#elif UNITY_IOS
            IntPtr ptr = XDAccountBridgeGetAuthorization(url, method, timestamp);
            authorization = Marshal.PtrToStringAnsi(ptr);
#endif
            if (string.IsNullOrEmpty(authorization)) {
                return Task.FromResult(new Dictionary<string, string>());
            }

            return Task.FromResult(new Dictionary<string, string> {
                { "Authorization", authorization }
            });
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern IntPtr XDAccountBridgeGetAuthorization(string url, string method, long timestamp);
#endif
    }
}