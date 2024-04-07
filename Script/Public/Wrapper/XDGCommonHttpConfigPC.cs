using System;
using System.Collections.Generic;
using XD.SDK.Common.Internal;
using XD.SDK.Common.PC.Internal;
using UnityEngine;

namespace XD.SDK.Common {
    public class XDGCommonHttpConfigPC : IXDCommonHttpConfig {
        // 正式
        // 国内
        private readonly static string CN_HOST = "https://xdsdk-6.xd.cn";
        // 国际
        private readonly static string GLOBAL_HOST = "https://xdsdk-intnl-6.xd.com";

        public Dictionary<string, string> GetCommonQueryParams(string url, long timestamp) {
            string clientId = ConfigModule.ClientId;
            Dictionary<string, string> param = new Dictionary<string, string>{
                { "clientId", string.IsNullOrEmpty(clientId) ? "" : clientId },
                { "appId", ConfigModule.AppId == null ? "" : ConfigModule.AppId },
                { "region", ConfigModule.RegionType },
                { "did", SystemInfo.deviceUniqueIdentifier },
                { "sdkLang", Localization.GetLanguageKey() },
                { "time", $"{timestamp}" },
                { "chn", "PC" },
                { "locationInfoType", "ip" },
                { "mem", SystemInfo.systemMemorySize / 1024 + "GB" },
                { "res", Screen.width + "_" + Screen.height },
                { "mod", SystemInfo.deviceModel },
                { "sdkVer", XDGCommonPC.SDKVersion },
                { "pkgName", Application.identifier },
                { "brand", SystemInfo.deviceModel },
                { "os", SystemInfo.operatingSystem },
                { "pt", GetPlatform() },
                { "appVer", Application.version },
                { "appVerCode", Application.version },
                { "cpu", SystemInfo.processorType },
                { "lang", StandaloneDeviceInfo.GetLanguage() },
                { "loc", StandaloneDeviceInfo.GetLocation() }
            };
            if (!string.IsNullOrEmpty(ConfigModule.region)) {
                param["countryCode"] = ConfigModule.region;
            }
            return param;
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
