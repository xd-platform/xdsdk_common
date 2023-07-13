#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    public class WebAuth {
        protected static async Task<Dictionary<string, object>> StartListener(List<HttpListener> servers,
            XD.SDK.Account.LoginType loginType,
            Dictionary<string, object> queryParams) {
            string redirectUri = $"http://{IPAddress.Loopback}:{LocalServerUtils.GetRandomUnusedPort()}";
            string gameName = ConfigModule.GameName;
            string lang = XD.SDK.Common.PC.Internal.Localization.GetLanguageKey();

            string url = ConfigModule.IsGlobal ? AuthConstants.GLOBAL_AUTH_HOST : AuthConstants.CN_AUTH_HOST;
            // 弹出授权页
            string authRequest = $"{url}?" +
                $"redirect_uri={redirectUri}&" +
                $"game_name={gameName}&" +
                $"lang={lang}";
            if (queryParams != null) {
                foreach (KeyValuePair<string, object> kv in queryParams) {
                    authRequest = $"{authRequest}&{kv.Key}={kv.Value}";
                }
            }

            XDLogger.Debug($"auth: {authRequest}");
            AliyunTrack.LoginAuthorize();
            // 登陆跳转授权 Track
            Application.OpenURL(Uri.EscapeUriString(authRequest));

            // 启动监听
            HttpListener server = new HttpListener();
            server.Prefixes.Add($"{redirectUri}/");
            server.Start();

            servers.Add(server);

            Dictionary<string, object> authData = new Dictionary<string, object> {
                { "type", (int) loginType }
            };

            while (true) {
                try {
                    HttpListenerContext context = await server.GetContextAsync();

                    XDLogger.Debug($"Request url: {context.Request.RawUrl}");
                    XDLogger.Debug($"Request method: {context.Request.HttpMethod}");

                    context.Response.StatusCode = 200;
                    context.Response.Close();

                    if (context.Request.HttpMethod == "OPTIONS") {
                        continue;
                    }

                    foreach (HttpListener svr in servers) {
                        svr.Stop();
                    }
                    servers.Clear();

                    // 解析 Web 像 SDK 传递的参数
                    string[] urlSegments = context.Request.RawUrl.Split('?');
                    if (urlSegments.Length < 2) {
                        return new Dictionary<string, object>();
                    }

                    string queryString = urlSegments[1];
                    string[] queries = queryString.Split('&');

                    foreach (string query in queries) {
                        if (string.IsNullOrEmpty(query)) {
                            continue;
                        }
                        string[] kv = query.Split('=');
                        if (kv.Length < 2) {
                            continue;
                        }
                        authData[kv[0]] = kv[1];
                    }
                    // 登陆授权成功
                    AliyunTrack.LoginAuthorizeSuccess();
                    return authData;
                } catch (Exception e) {
                    XDLogger.Debug(e.Message);
                    // 异常结果则一直 pending
                    // 登陆授权失败
                    AliyunTrack.LoginAuthorizeFail(e.Message);
                    TaskCompletionSource<Dictionary<string, object>> tcs = new TaskCompletionSource<Dictionary<string, object>>();
                    return await tcs.Task;
                }
            }
        }
    }
}
#endif