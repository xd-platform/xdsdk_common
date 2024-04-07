#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace XD.SDK.Common.PC.Internal {
    public class SteamAuth : WebAuth {
        static readonly List<HttpListener> servers = new List<HttpListener>();

        internal static async Task<Dictionary<string, object>> GetAuthData() {
            if (SteamUtils.IsSDKSupported) {
                // 先尝试从 Steam SDK 中获取 auth data
                Dictionary<string, object> authData = await GetAuthDataFromSDK();
                return authData;
            }

            // 返回 Web 授权的 AuthData
            Dictionary<string, object> queryParams = new Dictionary<string, object> {
                { "auth_type", "steam" },
                { "state", LocalServerUtils.GenerateRandomDataBase64url(32) },
                { "console_type", "steam" },
            };
            return await StartListener(servers, XD.SDK.Account.LoginType.Steam, queryParams);
        }

        internal static async Task<Dictionary<string, object>> GetAuthDataFromSDK() {
            // 返回 SDK 授权的 AuthData
            // 登陆跳转授权 Track
            AliyunTrack.LoginAuthorize();
            try
            {
                string ticket = await SteamUtils.Instance.GetAuthTicket();
                if (string.IsNullOrWhiteSpace(ticket))
                {
                    // 登陆授权失败
                    AliyunTrack.LoginAuthorizeFail("SteamNoTicket");
                }
                else
                {
                    // 登陆授权成功
                    AliyunTrack.LoginAuthorizeSuccess();
                }
                return new Dictionary<string, object> {
                    { "type", (int) XD.SDK.Account.LoginType.Steam },
                    { "token", ticket }
                };
            }
            catch (Exception e)
            {
                // 登陆授权成功
                AliyunTrack.LoginAuthorizeFail($"SteamNoTicket {e.Message}");
                return new Dictionary<string, object> {
                    { "type", (int) XD.SDK.Account.LoginType.Steam },
                    { "token", null }
                };
            }
        }

        internal static string GetSteamIdFromSDK() {
            return SteamUtils.Instance.GetSteamId();
        }

        internal static Dictionary<string, object> GetCacheData() {
            if (!SteamUtils.IsSDKSupported) {
                return null;
            }
            return new Dictionary<string, object> {
                { "steamId", GetSteamIdFromSDK() }
            };
        }
    }
}
#endif