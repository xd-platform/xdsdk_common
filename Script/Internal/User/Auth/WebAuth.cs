#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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
            Dictionary<string, object> queryParameters = new Dictionary<string, object> {
                { "redirect_uri", redirectUri },
                { "game_name", gameName },
                { "lang", lang },

                { "eventSessionId", AliyunTrack.LoginEventSessionId },
                { "app_version", Application.version },
                { "brand", SystemInfo.deviceModel },
                { "channel", ConfigModule.Channel },
                { "device_id", SystemInfo.deviceUniqueIdentifier },
                { "isloginmodule", true },
                { "loc", StandaloneDeviceInfo.GetLocation() },
                { "model", SystemInfo.processorType },
                { "orientation", Screen.orientation.ToString().ToLower() },
                { "session_uuid", AliyunTrack.CurrentSessionUUId },
                { "xd_client_id", ConfigModule.ClientId },
                { "sdk_version", XDGCommonPC.SDKVersion },
                { "os", AliyunTrack.GetPlatform() },
                { "os_version", SystemInfo.operatingSystem }
            };
            if (queryParams != null) {
                foreach (KeyValuePair<string, object> kv in queryParams) {
                    queryParameters[kv.Key] = kv.Value;
                }
            }
            IEnumerable<string> queryPairs = queryParameters.Select(kv => $"{kv.Key}={kv.Value}");
            string authRequest = $"{url}?{string.Join("&", queryPairs)}";

            // 弹出授权页
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