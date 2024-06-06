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

namespace XD.SDK.Mainland.Editor
{
    public class XDMainlandGradleContextProvider : IPreprocessBuildWithReport
    {
        public int callbackOrder => AndroidGradleProcessor.CALLBACK_ORDER - 51;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            DeleteOldProvider();
            var provider = FixProvider();
            SaveProvider(provider);
        }
        
        public static void CommonRefresh()
        {
            var temp = new XDMainlandGradleContextProvider();
            temp.OnPreprocessBuild(null);
        }
        
        private AndroidGradleContextProvider FixProvider()
        {
            AndroidGradleContextProvider result = new AndroidGradleContextProvider();
            result.Version = 1;
            result.Priority = 4;
            result.Use = true;
            result.ModuleName = "XD.Share";
            var tmp = GetGradleContext();
            result.AndroidGradleContext = tmp;
            return result;
        }

        private void DeleteOldProvider()
        {
            var folderPath = Path.Combine(Application.dataPath, "XDSDK", "Gen", "Share");
            if (!Directory.Exists(folderPath)) return;
            var path = Path.Combine(folderPath, "TapAndroidProvider.txt");
            if (File.Exists(path)) File.Delete(path);
        }
        
        private void SaveProvider(AndroidGradleContextProvider provider)
        {
            var folderPath = Path.Combine(Application.dataPath, "XDSDK", "Gen", "Share");
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
            
            var mavenCentralDeps = new AndroidGradleContext();
            mavenCentralDeps.locationType = AndroidGradleLocationType.Builtin;
            mavenCentralDeps.locationParam = "ARTIFACTORYREPOSITORY";
            mavenCentralDeps.templateType = CustomTemplateType.BaseGradle;
            mavenCentralDeps.processContent = new List<string>();
            mavenCentralDeps.processContent.Add(@"            mavenCentral()");
            result.Add(mavenCentralDeps);

            var bytedanceDeps = new AndroidGradleContext {
                // 仓库地址  
                locationType = AndroidGradleLocationType.End,
                templateType = CustomTemplateType.BaseGradle,
                processContent = new List<string> {
                    "allprojects {\n    repositories {\n        maven { url 'https://artifact.bytedance.com/repository/AwemeOpenSDK' }\n    }\n}"
                }
            };
            result.Add(bytedanceDeps);

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
            

            // 微信
            if (configObject["wechat"] != null)
            {
                thirdPartyDeps.processContent.Add(@"    implementation ""com.tencent.mm.opensdk:wechat-sdk-android:6.8.0""");
            }
            
            // 微博
            if (configObject["weibo"] != null)
            {
                thirdPartyDeps.processContent.Add(@"    implementation ""io.github.sinaweibosdk:core:12.5.0@aar""");
            }

            // qq 通过本地添加 jar 包的方式，不用再 gradle 里面添加依赖
            // if (configObject["qq"] != null)
            // {
            //     thirdPartyDeps.processContent.Add(@"    implementation files('libs/open_sdk_3.5.14.3_rc26220c_lite.jar')");
            // }

            // 抖音
            if (configObject["douyin"] != null) {
                thirdPartyDeps.processContent.Add(@"    implementation ""com.bytedance.ies.ugc.aweme:opensdk-china-external:0.1.9.9""");
                thirdPartyDeps.processContent.Add(@"    implementation ""com.bytedance.ies.ugc.aweme:opensdk-common:0.1.9.9""");
            }
            // 小红书，本地 jar 包方式
            //if (configObject["xhs"] != null) {
            //    thirdPartyDeps.processContent.Add();
            //}

            if (thirdPartyDeps.processContent.Count > 0)
            {
                result.Add(thirdPartyDeps);
            }

            return result;
        }
        
    }
}

#endif