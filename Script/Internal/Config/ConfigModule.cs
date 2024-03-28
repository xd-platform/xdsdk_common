#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    internal class ConfigModule {
        private static readonly string FETCH_CONFIG = "api/init/v1/config";

        private static readonly string CONFIG_FILE = "XDConfig";

        private static readonly Persistence persistence = new Persistence(Path.Combine(
            Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "config"));

        private static AppConfig appConfig;
        private static Config config;

        internal static string region;

        private static string LoadConfigText() {
#if UNITY_EDITOR
            var xdConfigFilePath = GetXDConfigPath(CONFIG_FILE);
#else
            var xdConfigFilePath = Path.Combine(Application.streamingAssetsPath, $"{CONFIG_FILE}.json");
#endif
            return File.ReadAllText(xdConfigFilePath);
        }
        
        private static string GetXDConfigPath(string fileName)
        {
            string xdconfigPath = null;
            
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            xdconfigPath = Path.Combine(parentFolder, $"Assets/{fileName}.json");

            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find {fileName}.json on Assets folder");
            xdconfigPath = Path.Combine(parentFolder, $"Assets/Plugins/{fileName}.json");
            
            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find {fileName}.json on Assets folder or Assets/Plugins folder");
#if UNITY_EDITOR
            bool find = false;
            var xdconfigGuids = UnityEditor.AssetDatabase.FindAssets(fileName);
            foreach (var guid in xdconfigGuids)
            {
                var xdPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var fileInfo = new FileInfo(xdPath);
                if (fileInfo.Name != $"{fileName}.json") continue;
                xdconfigPath = Path.Combine(parentFolder, xdPath);
                find = true;
                break;
            }

            if (find)
            { 
                Debug.LogFormat($"[XDSDK.Common] Can find {fileName}.json at: {xdconfigPath}");
                return xdconfigPath;
            }
#endif
            Debug.LogFormat($"[XDSDK.Common] CAN NOT find {fileName}.json!!");
            return "";
        }
        
        internal static void LoadConfig() {
            var txtAsset = LoadConfigText();
            appConfig = JsonConvert.DeserializeObject<AppConfig>(txtAsset);
        }

        internal static async Task<Config> FetchConfig() {
            try {
                ConfigResponse response = await XDHttpClient.Get<ConfigResponse>(FETCH_CONFIG);
                config = response.ConfigData.Config;
                await persistence.Save(config);
                return config;
            } catch (Exception e) {
                XDLogger.Warn($"拉取配置失败: {e}");
                config = await persistence.Load<Config>();
                return config;
            } finally {
                _ = UploadConfigModule.TryUploadConfig();
            }
        }

        internal static string ClientId => appConfig.ClientId;

        internal static string GameName => appConfig.GameName;

        internal static string RegionType => appConfig.RegionType;

        internal static TapConfig TapConfig => appConfig.TapConfig;

        internal static string AppId => config != null && !string.IsNullOrEmpty(config.AppId) ?
            config.AppId : appConfig.AppId;

        internal static string WebPayUrl => config != null && !string.IsNullOrEmpty(config.WebPayUrl) ?
            config.WebPayUrl : appConfig.WebPayUrl;

        internal static string WebPayUrlForPC => config != null && !string.IsNullOrEmpty(config.WebPayUrlForPC) ?
            config.WebPayUrlForPC : appConfig.WebPayUrlForPC;

        internal static string Region => !string.IsNullOrEmpty(region) ? region : config?.Region;

        internal static string Channel => appConfig?.TapConfig?.DBConfig?.Channel;

        internal static List<BindEntryConfig> BindEntryConfigs => config?.BindEntryConfigs;

        internal static List<string> BindEntries => config?.BindEntryConfigs?.Select(item => item.EntryName)?.ToList();

        internal static string ReportUrl => config != null && !string.IsNullOrEmpty(config.ReportUrl) ?
            config.ReportUrl : appConfig.ReportUrl;

        internal static GoogleConfig Google => appConfig.Google;

        internal static AppleConfig Apple => appConfig.Apple;

        internal static FacebookConfig Facebook => appConfig.Facebook;

        internal static bool IsGlobal => appConfig.IsGlobal;

        internal static string CancelUrl => config != null && !string.IsNullOrEmpty(config.CancelUrl) ?
            config.CancelUrl : appConfig.CancelUrl;
    }
}
#endif