#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LC.Newtonsoft.Json;
using LC.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using TapTap.AndroidDependencyResolver.Editor;
using XD.SDK.Common.Editor;

namespace XD.SDK.ADSubPackage
{
    public class XDGADSubGradleProvider : IPreprocessBuildWithReport
    {
        public int callbackOrder => AndroidGradleProcessor.CALLBACK_ORDER - 51;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            DeleteOldProvider();
            var provider = FixProvider();
            SaveProvider(provider);
        }
        
        private AndroidGradleContextProvider FixProvider()
        {
            AndroidGradleContextProvider result = new AndroidGradleContextProvider();
            result.Version = 1;
            result.Priority = 3;
            result.Use = true;
            result.ModuleName = "XD.ADSub";
            var tmp = GetGradleContext();
            result.AndroidGradleContext = tmp;
            return result;
        }

        private void DeleteOldProvider()
        {
            var folderPath = Path.Combine(Application.dataPath, "XDSDK", "Gen", "ADSub");
            if (!Directory.Exists(folderPath)) return;
            var path = Path.Combine(folderPath, "TapAndroidProvider.txt");
            if (File.Exists(path)) File.Delete(path);
        }
        
        private void SaveProvider(AndroidGradleContextProvider provider)
        {
            var folderPath = Path.Combine(Application.dataPath, "XDSDK", "Gen", "ADSub");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }
            var path = Path.Combine(folderPath, "TapAndroidProvider.txt");
            AndroidUtils.SaveProvider(path, provider);
        }

        public List<AndroidGradleContext> GetGradleContext()
        {
            var result = GetGradleContextByXDConfig();
            return result;
            
        }

        private List<AndroidGradleContext> GetGradleContextByXDConfig()
        {
            var result = new List<AndroidGradleContext>();
            
            var jsonPath = XDGCommonEditorUtils.GetXDConfigPath("XDConfig");
            if (!File.Exists(jsonPath))
            {
                Debug.LogError("/Assets/XDConfig.json 配置文件不存在！");
                return result;
            }

            var configObject = JObject.Parse(File.ReadAllText(jsonPath));

            var thirdPartyDeps = new AndroidGradleContext();
            thirdPartyDeps.locationType = AndroidGradleLocationType.Builtin;
            thirdPartyDeps.locationParam = "DEPS";
            thirdPartyDeps.templateType = CustomTemplateType.UnityMainGradle;
            thirdPartyDeps.processContent = new List<string>();

            if (configObject["ad_config"] != null)
            {
                if (configObject["ad_config"]["tt_config"] != null)
                {
                    // 今日头条广告包 SDK
                    thirdPartyDeps.processContent.Add(@"    implementation 'com.bytedance.applog:RangersAppLog-Lite-cn:6.10.1'");
                }

                if (configObject["ad_config"]["gdt_config"] != null)
                {   
                    // 广点通分包 SDK
                    thirdPartyDeps.processContent.Add(@"    implementation 'com.tencent.vasdolly:helper:3.0.4'");
                }
            }

            if (thirdPartyDeps.processContent.Count > 0)
            {
                result.Add(thirdPartyDeps);
            }

            return result;
        }
        
    }
}

#endif