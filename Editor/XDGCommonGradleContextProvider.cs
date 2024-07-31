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
        private const string KOTLIN_DEP = "    implementation 'org.jetbrains.kotlin:kotlin-stdlib:1.7.21'";
        private const string RETROFIT2 = "    implementation \"com.squareup.retrofit2:retrofit:2.9.0\"";
        private const string RETROFIT2_RX = "    implementation \"com.squareup.retrofit2:adapter-rxjava2:2.9.0\"";
        private const string RETROFIT2_CONVERTER = "    implementation \"com.squareup.retrofit2:converter-gson:2.9.0\"";
        private const string RXJAVA2 = "    implementation \"io.reactivex.rxjava2:rxandroid:2.1.1\"";
        private const string OkHttp = "    implementation \"com.squareup.okhttp3:okhttp:4.7.2\"";
        private const string Okio = "    implementation \"com.squareup.okio:okio:2.6.0\"";
        private const string ZXing = "    implementation \"com.google.zxing:core:3.5.3\"";

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
            ADX_COMPAT_DEP,
            RETROFIT2,
            RETROFIT2_RX,
            RETROFIT2_CONVERTER,
            RXJAVA2,
            OkHttp,
            Okio,
            ZXing
        };
        
        public void OnPreprocessBuild(BuildReport report)
        {
            DeleteOldProvider();
            var provider = FixProvider();
            SaveProvider(provider);
        }

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
        
        public static bool IsNeedOverseaDep(XDConfigModel xdConfigModel)
        {
            if (xdConfigModel == null)
                return false;

            return IsNeedGoogleDep(xdConfigModel) || IsNeedFirebaseDep(xdConfigModel);
        }
        
        public static bool IsNeedGoogleDep(XDConfigModel xdConfigModel)
        {
            if (xdConfigModel == null)
                return false;
            bool noGoogleServices = xdConfigModel.google == null || (string.IsNullOrEmpty(xdConfigModel.google.CLIENT_ID) &&
                                                                  string.IsNullOrEmpty(xdConfigModel.google.CLIENT_ID_FOR_ANDROID));
            return !noGoogleServices;
        }
        
        public static bool IsNeedFirebaseDep(XDConfigModel xdConfigModel)
        {
            if (xdConfigModel == null)
                return false;
            bool noFirebaseServices = xdConfigModel.firebase == null || !xdConfigModel.firebase.enableTrack;
            return !noFirebaseServices;
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
            
            var alwaysIncludeDeps = new AndroidGradleContext();
            alwaysIncludeDeps.locationType = AndroidGradleLocationType.Builtin;
            alwaysIncludeDeps.locationParam = "DEPS";
            alwaysIncludeDeps.templateType = CustomTemplateType.UnityMainGradle;
            alwaysIncludeDeps.processContent = new List<string>(AlwaysIncludeDeps);
            result.Add(alwaysIncludeDeps);
                
            // Unity 2018 不支持自定义gradle.properties
            var gradlePropertiesContext = new AndroidGradleContext();
            gradlePropertiesContext.locationType = AndroidGradleLocationType.Builtin;
            gradlePropertiesContext.locationParam = "ADDITIONAL_PROPERTIES";
            gradlePropertiesContext.templateType = CustomTemplateType.GradleProperties;
            gradlePropertiesContext.unityVersionCompatibleType = UnityVersionCompatibleType.Unity_2019_3_Above;
            gradlePropertiesContext.processContent = new List<string>(AdditionalProperties);
            result.Add(gradlePropertiesContext);
            
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

            return result;
            
        }
    }
}

#endif