#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    public class CustomerModule {
        public static async Task<string> GetCustomerCenterUrl(string serverId, string roleId, string roleName) {
            var clientId = ConfigModule.ClientId;
            var userMd = await UserModule.GetLocalUser();
            var tkModel = await AccessTokenModule.GetLocalAccessToken();
            if (userMd == null) {
                return null;
            }

            var uri = new Uri(ConfigModule.ReportUrl);
            var url = uri.GetLeftPart(UriPartial.Path);
            Dictionary<string, object> param = new Dictionary<string, object> {
                { "client_id", string.IsNullOrEmpty(clientId) ? "" : clientId },
                { "access_token", tkModel.kid },
                { "user_id", userMd.userId },
                { "server_id", serverId },
                { "role_id", roleId },
                { "role_name", roleName },
                { "region", ConfigModule.Region },
                { "sdk_ver", XDGCommonPC.SDKVersion },
                { "sdk_lang", XD.SDK.Common.PC.Internal.Localization.GetCustomerCenterLang() },
                { "app_ver", Application.version },
                { "app_ver_code", Application.version },
                { "res", Screen.width + "_" + Screen.height },
                { "cpu", SystemInfo.processorType },
                { "mem", SystemInfo.systemMemorySize / 1024 + "GB" },
                { "pt", GetPlatform() },
                { "os", SystemInfo.operatingSystem },
                { "brand", SystemInfo.deviceModel },
                { "game_name", Application.productName },
            };
            return $"{url}?{XDHttpClient.EncodeQueryParams(param)}";
        }

        private static string GetPlatform() {
            string os = "Linux";
#if UNITY_STANDALONE_OSX
            os = "macOS";
#endif
#if UNITY_STANDALONE_WIN
            os = "Windows";
#endif
            return os;
        }
    }
}
#endif