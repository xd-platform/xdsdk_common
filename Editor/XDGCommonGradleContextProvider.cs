#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LC.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using TapTap.AndroidDependencyResolver.Editor;

namespace XD.SDK.Common.Editor
{
    public class XDGCommonGradleProvider : IPreprocessBuildWithReport
    {
        public int callbackOrder => AndroidGradleProcessor.CALLBACK_ORDER - 50;

        private string ProviderFilePath => Path.Combine(Application.dataPath, "XDSDK", "Gen", "Common");
        
        #region Gradle Content String
        
        private const string ADX_RCV_DEP = "    implementation 'androidx.recyclerview:recyclerview:1.2.1'";
        private const string ADX_COMPAT_DEP = "    implementation 'androidx.appcompat:appcompat:1.3.1'";
        private const string GSON_DEP = "    implementation 'com.google.code.gson:gson:2.8.6'";
        private const string KOTLIN_DEP = "    implementation 'org.jetbrains.kotlin:kotlin-stdlib:1.5.10'";
        private const string ANDROIDX = "android.useAndroidX=true";
        private const string ANDROID_JET = "android.enableJetifier=true";

        #endregion
        
        private HashSet<string> AdditionalProperties = new HashSet<string>()
        {
            ANDROIDX,
            ANDROID_JET,
        };
        
        private HashSet<string> AlwaysIncludeDeps = new HashSet<string>()
        {
            ADX_RCV_DEP,
            GSON_DEP,
            KOTLIN_DEP,
            ADX_COMPAT_DEP
        };
        
        public void OnPreprocessBuild(BuildReport report)
        {
            DeleteOldProvider();
            var provider = FixProvider();
            SaveProvider(provider);
        }

        [MenuItem("XD/Common/Refresh Android Gradle")]
        public static void CommonRefresh()
        {
            var temp = new XDGCommonGradleProvider();
            temp.OnPreprocessBuild(null);
        }

        private AndroidGradleContextProvider FixProvider()
        {
            AndroidGradleContextProvider result = new AndroidGradleContextProvider();
            result.Version = 1;
            result.Priority = 2;
            result.Use = true;
            result.ModuleName = "XD.Common";
            var tmp = GetGradleContext();
            result.AndroidGradleContext = tmp;
            return result;
        }

        private void DeleteOldProvider()
        {
            var folderPath = ProviderFilePath;
            if (!Directory.Exists(folderPath)) return;
            var path = Path.Combine(folderPath, "TapAndroidProvider.txt");
            if (File.Exists(path)) File.Delete(path);
        }
        
        private void SaveProvider(AndroidGradleContextProvider provider)
        {
            var folderPath = ProviderFilePath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }
            var path = Path.Combine(folderPath, "TapAndroidProvider.txt");
            AndroidUtils.SaveProvider(path, provider);
        }

        private void DeleteOldGoogleContent()
        {
            var pluginsFolder = Path.Combine(Application.dataPath, "Plugins", "Android");
            DeleteMainOldGoogleContent(pluginsFolder);
        }
        
        private void DeleteMainOldGoogleContent(string pluginsFolder)
        {
            var gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle");
            if (!File.Exists(gradleTemplate))
            {
                gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle.DISABLED");
            }
            if (!File.Exists(gradleTemplate)) return;
            var writerHelper = new XDGScriptHandlerProcessor(gradleTemplate);

            foreach (var dep in AlwaysIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            
            writerHelper.Dispose();
        }

        public List<AndroidGradleContext> GetGradleContext()
        {
            var result = new List<AndroidGradleContext>();
            
            var xdconfigPath = XDGCommonEditorUtils.GetXDConfigPath("XDConfig");
            if (!File.Exists(xdconfigPath))
            {
                Debug.LogError("XDConfig.json 配置文件不存在！");
                return result;
            }
            var configMd = JsonConvert.DeserializeObject<XDConfigModel>(File.ReadAllText(xdconfigPath));
            if (configMd == null)
            {
                Debug.LogError("XDConfig.json 解析失败！");
                return result;
            }

            AndroidGradleContext thirdPartyDeps = null;
            
            var noGoogleDeps = new AndroidGradleContext();
            noGoogleDeps.locationType = AndroidGradleLocationType.Builtin;
            noGoogleDeps.locationParam = "DEPS";
            noGoogleDeps.templateType = CustomTemplateType.UnityMainGradle;
            noGoogleDeps.processContent = new List<string>(AlwaysIncludeDeps);
            result.Add(noGoogleDeps);
                
            // Unity 2018 不支持自定义gradle.properties
            var gradlePropertiesContext = new AndroidGradleContext();
            gradlePropertiesContext.locationType = AndroidGradleLocationType.Builtin;
            gradlePropertiesContext.locationParam = "ADDITIONAL_PROPERTIES";
            gradlePropertiesContext.templateType = CustomTemplateType.GradleProperties;
            gradlePropertiesContext.unityVersionCompatibleType = UnityVersionCompatibleType.Unity_2019_3_Above;
            gradlePropertiesContext.processContent = new List<string>(AdditionalProperties);
            result.Add(gradlePropertiesContext);
            
            bool noGoogleServices = configMd.google == null || (string.IsNullOrEmpty(configMd.google.CLIENT_ID) &&
                                                                  string.IsNullOrEmpty(configMd.google.CLIENT_ID_FOR_ANDROID));
            if (noGoogleServices)
            {
                DeleteOldGoogleContent();
            }
            else
            {
                var androidPluginContext = new AndroidGradleContext();
                androidPluginContext.locationType = AndroidGradleLocationType.Custom;
                androidPluginContext.locationParam =
                    "classpath 'com.android.tools.build:gradle:3.\\d{1}.\\d{1}'";
                androidPluginContext.templateType = CustomTemplateType.BaseGradle;
                androidPluginContext.processType = AndroidGradleProcessType.Replace;
                androidPluginContext.processContent = new List<string>()
                {
                    @"classpath 'com.android.tools.build:gradle:4.0.1'",
                };
                result.Add(androidPluginContext);
                
                var androidPluginContext1 = new AndroidGradleContext();
                androidPluginContext1.locationType = AndroidGradleLocationType.Custom;
                androidPluginContext1.locationParam =
                    "classpath 'com.android.tools.build:gradle:4.0.0'";
                androidPluginContext1.templateType = CustomTemplateType.BaseGradle;
                androidPluginContext1.processType = AndroidGradleProcessType.Replace;
                androidPluginContext1.processContent = new List<string>()
                {
                    @"classpath 'com.android.tools.build:gradle:4.0.1'",
                };
                result.Add(androidPluginContext1);
            }

            return result;
            
        }
    }
}

#endif