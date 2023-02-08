#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        public void OnPreprocessBuild(BuildReport report)
        {
            DeleteOldProvider();
            var provider = FixProvider();
            SaveProvider(provider);
        }

        [MenuItem("XDSDK/Common/Refresh Android Gradle")]
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
            var folderPath = Path.Combine(Application.dataPath, "XDSDK", "Gen", "Common");
            if (!Directory.Exists(folderPath)) return;
            var path = Path.Combine(folderPath, "TapAndroidProvider.txt");
            if (File.Exists(path)) File.Delete(path);
        }
        
        private void SaveProvider(AndroidGradleContextProvider provider)
        {
            var folderPath = Path.Combine(Application.dataPath, "XDSDK", "Gen", "Common");
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
            DeleteLaucherOldGoogleContent(pluginsFolder);
            DeleteMainOldGoogleContent(pluginsFolder);
            DeleteThirdPartyContent(pluginsFolder);
            DeleteBaseOldGoogleContent(pluginsFolder);
        }

        private void DeleteLaucherOldGoogleContent(string pluginsFolder)
        {
        #if UNITY_2019_3_OR_NEWER
            var launcherGradle = Path.Combine(pluginsFolder, "launcherTemplate.gradle");
            if (!File.Exists(launcherGradle))
            {
                launcherGradle = Path.Combine(pluginsFolder, "launcherTemplate.gradle.DISABLED");
            }
        #else
            var launcherGradle = Path.Combine(pluginsFolder, "mainTemplate.gradle");
            if (!File.Exists(launcherGradle))
            {
                launcherGradle = Path.Combine(pluginsFolder, "mainTemplate.gradle.DISABLED");
            }
        #endif
            if (!File.Exists(launcherGradle)) return;
            var writerHelper = new XDGScriptHandlerProcessor(launcherGradle);
            writerHelper.Delete(@"apply plugin: 'com.google.gms.google-services'");
            writerHelper.Delete(@"apply plugin: 'com.google.firebase.crashlytics'");
            writerHelper.Dispose();
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
            writerHelper.Delete(@"    implementation 'com.google.firebase:firebase-core:18.0.0'");
            writerHelper.Delete(@"    implementation 'com.google.firebase:firebase-messaging:21.1.0'");
            writerHelper.Delete(@"    implementation 'com.google.android.gms:play-services-auth:16.0.1'");
            writerHelper.Delete(@"    implementation 'com.google.android.gms:play-services-ads-identifier:15.0.1'");
            writerHelper.Delete(@"    implementation 'com.android.installreferrer:installreferrer:2.2'");
            writerHelper.Delete(@"    implementation 'com.android.billingclient:billing:4.1.0'");
            writerHelper.Delete(@"    implementation 'androidx.recyclerview:recyclerview:1.2.1'");
            writerHelper.Delete(@"    implementation 'com.google.code.gson:gson:2.8.6'");
            writerHelper.Delete(@"    implementation 'org.jetbrains.kotlin:kotlin-stdlib:1.5.10'");
            writerHelper.Dispose();
        }

        private void DeleteThirdPartyContent(string pluginsFolder)
        {
            var gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle");
            if (!File.Exists(gradleTemplate))
            {
                gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle.DISABLED");
            }
            if (!File.Exists(gradleTemplate)) return;
            var writerHelper = new XDGScriptHandlerProcessor(gradleTemplate);
            writerHelper.Delete(@"    implementation 'com.facebook.android:facebook-login:12.0.0'");
            writerHelper.Delete(@"    implementation 'com.facebook.android:facebook-share:12.0.0'");
            writerHelper.Delete(@"    implementation 'com.twitter.sdk.android:twitter:3.3.0'");
            writerHelper.Delete(@"    implementation 'com.twitter.sdk.android:tweet-composer:3.3.0'");
            writerHelper.Delete(@"    implementation 'com.appsflyer:af-android-sdk:6.5.2'");
            writerHelper.Delete(@"    implementation 'com.appsflyer:unity-wrapper:6.5.2'");
            writerHelper.Delete(@"    implementation 'com.adjust.sdk:adjust-android:4.24.1'");
            writerHelper.Delete(@"    implementation 'com.linecorp:linesdk:5.0.1'");
            writerHelper.Dispose();
        }
        
        private void DeleteBaseOldGoogleContent(string pluginsFolder)
        {
        #if UNITY_2019_3_OR_NEWER
            var gradleTemplate = Path.Combine(pluginsFolder, "baseProjectTemplate.gradle");
            if (!File.Exists(gradleTemplate))
            {
                gradleTemplate = Path.Combine(pluginsFolder, "baseProjectTemplate.gradle.DISABLED");
            }
        #else
            var gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle");
            if (!File.Exists(gradleTemplate))
            {
                gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle.DISABLED");
            }
        #endif
            if (!File.Exists(gradleTemplate)) return;
            var writerHelper = new XDGScriptHandlerProcessor(gradleTemplate);
            writerHelper.Delete(@"        classpath 'com.google.gms:google-services:4.0.2'");
            writerHelper.Delete(@"        classpath 'com.google.firebase:firebase-crashlytics-gradle:2.2.1'");
            writerHelper.Dispose();
        }
        
        private string GetXDConfigPath()
        {
            string xdconfigPath = null;
            
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            xdconfigPath = Path.Combine(parentFolder, "Assets/XDConfig.json");

            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find XDConfig.json on Assets folder");
            xdconfigPath = Path.Combine(parentFolder, "Assets/Plugins/XDConfig.json");
            
            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find XDConfig.json on Assets folder or Assets/Plugins folder");

            bool find = false;
            var xdconfigGuids = AssetDatabase.FindAssets("XDConfig");
            foreach (var guid in xdconfigGuids)
            {
                var xdPath = AssetDatabase.GUIDToAssetPath(guid);
                var fileInfo = new FileInfo(xdPath);
                if (fileInfo.Name != "XDConfig.json") continue;
                xdconfigPath = Path.Combine(parentFolder, xdPath);
                find = true;
                break;
            }

            if (find)
            { 
                Debug.LogFormat($"[XDSDK.Common] Can find XDConfig.json at: {xdconfigPath}");
                return xdconfigPath;
            }
            Debug.LogFormat($"[XDSDK.Common] CAN NOT find XDConfig.json!!");
            return "";
        }

        public List<AndroidGradleContext> GetGradleContext()
        {
            var result = new List<AndroidGradleContext>();
            
            var xdconfigPath = GetXDConfigPath();
            if (!File.Exists(xdconfigPath))
            {
                Debug.LogError("/Assets/XDConfig.json 配置文件不存在！");
                return result;
            }
            var configMd = JsonConvert.DeserializeObject<XDConfigModel>(File.ReadAllText(xdconfigPath));
            if (configMd == null)
            {
                Debug.LogError("/Assets/XDConfig.json 解析失败！");
                return result;
            }

            AndroidGradleContext thirdPartyDeps = null;

            bool noGoogleServices = configMd.google == null || (string.IsNullOrEmpty(configMd.google.CLIENT_ID) &&
                                                                  string.IsNullOrEmpty(configMd.google.CLIENT_ID_FOR_ANDROID));
            if (noGoogleServices)
            {
                DeleteOldGoogleContent();
                var noGoogleDeps = new AndroidGradleContext();
                noGoogleDeps.locationType = AndroidGradleLocationType.Builtin;
                noGoogleDeps.locationParam = "DEPS";
                noGoogleDeps.templateType = CustomTemplateType.UnityMainGradle;
                noGoogleDeps.processContent = new List<string>()
                {
                    @"    implementation 'com.google.android.gms:play-services-auth:16.0.1'",
                    @"    implementation 'androidx.recyclerview:recyclerview:1.2.1'",
                    @"    implementation 'com.google.code.gson:gson:2.8.6'",
                    @"    implementation 'org.jetbrains.kotlin:kotlin-stdlib:1.5.10'"
                };
                result.Add(noGoogleDeps);
                
                // Unity 2018 不支持自定义gradle.properties
                var gradlePropertiesContext = new AndroidGradleContext();
                gradlePropertiesContext.locationType = AndroidGradleLocationType.Builtin;
                gradlePropertiesContext.locationParam = "ADDITIONAL_PROPERTIES";
                gradlePropertiesContext.templateType = CustomTemplateType.GradleProperties;
                gradlePropertiesContext.unityVersionCompatibleType = UnityVersionCompatibleType.Unity_2019_3_Above;
                gradlePropertiesContext.processContent = new List<string>()
                {
                    @"android.useAndroidX=true",
                    @"android.enableJetifier=true",
                };
                result.Add(gradlePropertiesContext);
            }
            else
            {
                var googleContext = new AndroidGradleContext();
                googleContext.locationType = AndroidGradleLocationType.Custom;
                googleContext.locationParam = "apply plugin: ['\"]com.android.application['\"]";
                googleContext.templateType = CustomTemplateType.LauncherGradle;
                googleContext.processContent = new List<string>()
                {
                    @"apply plugin: 'com.google.gms.google-services'",
                    @"apply plugin: 'com.google.firebase.crashlytics'",
                };
                result.Add(googleContext);
                
                var firebaseCoreDeps = new AndroidGradleContext();
                firebaseCoreDeps.locationType = AndroidGradleLocationType.Builtin;
                firebaseCoreDeps.locationParam = "DEPS";
                firebaseCoreDeps.templateType = CustomTemplateType.UnityMainGradle;
                firebaseCoreDeps.processContent = new List<string>()
                {
                    @"    implementation 'com.google.firebase:firebase-core:18.0.0'",
                    @"    implementation 'com.google.firebase:firebase-messaging:21.1.0'",
                    @"    implementation 'com.google.android.gms:play-services-auth:16.0.1'",
                    @"    implementation 'com.google.android.gms:play-services-ads-identifier:15.0.1'",
                    @"    implementation 'com.android.installreferrer:installreferrer:2.2'",
                    @"    implementation 'com.android.billingclient:billing:4.1.0'",
                    @"    implementation 'androidx.recyclerview:recyclerview:1.2.1'",
                    @"    implementation 'com.google.code.gson:gson:2.8.6'",
                    @"    implementation 'org.jetbrains.kotlin:kotlin-stdlib:1.5.10'"
                };
                result.Add(firebaseCoreDeps);

                var googleClassPathDependencies = new AndroidGradleContext();
                googleClassPathDependencies.locationType = AndroidGradleLocationType.Builtin;
                googleClassPathDependencies.locationParam = "BUILD_SCRIPT_DEPS";
                googleClassPathDependencies.templateType = CustomTemplateType.BaseGradle;
                googleClassPathDependencies.unityVersionCompatibleType = UnityVersionCompatibleType.EveryVersion;
                googleClassPathDependencies.processContent = new List<string>()
                {
                    @"        classpath 'com.google.gms:google-services:4.0.2'",
                    @"        classpath 'com.google.firebase:firebase-crashlytics-gradle:2.2.1'"
                };
                result.Add(googleClassPathDependencies);
                
                // Unity 2018 不支持自定义gradle.properties
                var gradlePropertiesContext = new AndroidGradleContext();
                gradlePropertiesContext.locationType = AndroidGradleLocationType.Builtin;
                gradlePropertiesContext.locationParam = "ADDITIONAL_PROPERTIES";
                gradlePropertiesContext.templateType = CustomTemplateType.GradleProperties;
                gradlePropertiesContext.unityVersionCompatibleType = UnityVersionCompatibleType.Unity_2019_3_Above;
                gradlePropertiesContext.processContent = new List<string>()
                {
                    @"android.useAndroidX=true",
                    @"android.enableJetifier=true",
                };
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
            }

            var contents = GetGradleContextByXDConfig();
            if (contents != null) result.AddRange(contents);
            return result;
            
        }

        private List<AndroidGradleContext> GetGradleContextByXDConfig()
        {
            var result = new List<AndroidGradleContext>();
            
            var jsonPath = GetXDConfigPath();
            if (!File.Exists(jsonPath))
            {
                Debug.LogError("/Assets/XDConfig.json 配置文件不存在！");
                return result;
            }

            var configMd = JsonConvert.DeserializeObject<XDConfigModel>(File.ReadAllText(jsonPath));
            if (configMd == null)
            {
                Debug.LogError("/Assets/XDConfig.json 解析失败！");
                return result;
            }

            AndroidGradleContext thirdPartyDeps = null;
            
            //配置第三方库
            if (configMd.facebook != null && !string.IsNullOrEmpty(configMd.facebook.app_id))
            {
                InitThirdPartyDeps(ref thirdPartyDeps);
                thirdPartyDeps.processContent.Add(@"    implementation 'com.facebook.android:facebook-login:12.0.0'"); 
                thirdPartyDeps.processContent.Add(@"    implementation 'com.facebook.android:facebook-share:12.0.0'"); 
            }
        
            if (configMd.twitter != null && !string.IsNullOrEmpty(configMd.twitter.consumer_key))
            {
                InitThirdPartyDeps(ref thirdPartyDeps);
                thirdPartyDeps.processContent.Add(@"    implementation 'com.twitter.sdk.android:twitter:3.3.0'"); 
                thirdPartyDeps.processContent.Add(@"    implementation 'com.twitter.sdk.android:tweet-composer:3.3.0'"); 
            }
        
            if (configMd.appsflyer != null && !string.IsNullOrEmpty(configMd.appsflyer.dev_key))
            {
                InitThirdPartyDeps(ref thirdPartyDeps);
                thirdPartyDeps.processContent.Add(@"    implementation 'com.appsflyer:af-android-sdk:6.5.2'"); 
                thirdPartyDeps.processContent.Add(@"    implementation 'com.appsflyer:unity-wrapper:6.5.2'"); 
            }
        
            if (configMd.adjust != null && !string.IsNullOrEmpty(configMd.adjust.app_token))
            {
                InitThirdPartyDeps(ref thirdPartyDeps);
                thirdPartyDeps.processContent.Add(@"    implementation 'com.adjust.sdk:adjust-android:4.24.1'"); 
            }
        
            if (configMd.line != null && !string.IsNullOrEmpty(configMd.line.channel_id))
            {
                InitThirdPartyDeps(ref thirdPartyDeps);
                thirdPartyDeps.processContent.Add(@"    implementation 'com.linecorp:linesdk:5.0.1'"); 
            }

            if (thirdPartyDeps != null)
            {
                result.Add(thirdPartyDeps);
            }

            return result;
        }

        private void InitThirdPartyDeps(ref AndroidGradleContext thirdPartyDeps)
        {
            if (thirdPartyDeps == null)
            {
                thirdPartyDeps = new AndroidGradleContext();
                thirdPartyDeps.locationType = AndroidGradleLocationType.Builtin;
                thirdPartyDeps.locationParam = "DEPS";
                thirdPartyDeps.templateType = CustomTemplateType.UnityMainGradle;
                thirdPartyDeps.processContent = new List<string>();
            }
        }

        
    }
}

#endif