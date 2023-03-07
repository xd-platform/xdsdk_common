#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    /// <summary>
    /// Steam SDK 封装接口
    /// </summary>
    public interface ISteamSDKWrapper {
        Task<string> GetAuthTicket();
        string GetSteamId();
    }

    public class SteamAuth : WebAuth {
        static readonly List<HttpListener> servers = new List<HttpListener>();

        static readonly ISteamSDKWrapper steamSDKWrapper;

        static SteamAuth() {
            Type interfaceType = typeof(ISteamSDKWrapper);
            Type steamWrapperType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);
            if (steamWrapperType != null) {
                steamSDKWrapper = Activator.CreateInstance(steamWrapperType) as ISteamSDKWrapper;
            }
        }

        internal static async Task<Dictionary<string, object>> GetAuthData() {
            if (IsSDKSupported) {
                // 先尝试从 Steam SDK 中获取 auth data
                Dictionary<string, object> authData = await GetAuthDataFromSDK();
                return authData;
            }

            // 返回 Web 授权的 AuthData
            Dictionary<string, object> queryParams = new Dictionary<string, object> {
                { "auth_type", "steam" },
                { "state", LocalServerUtils.GenerateRandomDataBase64url(32) }
            };
            return await StartListener(servers, XD.SDK.Account.LoginType.Steam, queryParams);
        }

        internal static async Task<Dictionary<string, object>> GetAuthDataFromSDK() {
            // 返回 SDK 授权的 AuthData
            // 登陆跳转授权 Track
            AliyunTrack.LoginAuthorize();
            try
            {
                string ticket = await steamSDKWrapper.GetAuthTicket();
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
            return steamSDKWrapper.GetSteamId();
        }

        /// <summary>
        /// 是否有 SDK 支持，即 Steam 包
        /// </summary>
        internal static bool IsSDKSupported => steamSDKWrapper != null;

        internal static Dictionary<string, object> GetCacheData() {
            if (!IsSDKSupported) {
                return null;
            }
            return new Dictionary<string, object> {
                { "steamId", GetSteamIdFromSDK() }
            };
        }
    }
}
#endif