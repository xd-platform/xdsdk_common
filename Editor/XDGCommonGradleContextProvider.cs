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

        private const string GOOGLE_GMS_PLUGIN = "apply plugin: 'com.google.gms.google-services'";
        private const string FIREBASE_CRASH_PLUGIN = "apply plugin: 'com.google.firebase.crashlytics'";
        private const string FIREBASE_CORE_DEP = "    implementation 'com.google.firebase:firebase-core:18.0.0'";
        private const string FIREBASE_MSG_DEP = "    implementation 'com.google.firebase:firebase-messaging:21.1.0'";
        private const string GMS_PLAY_DEP = "    implementation 'com.google.android.gms:play-services-auth:16.0.1'";
        private const string GMS_ADS_ID_DEP = "    implementation 'com.google.android.gms:play-services-ads-identifier:15.0.1'";
        private const string AD_INST_DEP = "    implementation 'com.android.installreferrer:installreferrer:2.2'";
        private const string AD_BILL_DEP = "    implementation 'com.android.billingclient:billing:4.1.0'";
        private const string ADX_RCV_DEP = "    implementation 'androidx.recyclerview:recyclerview:1.2.1'";
        private const string ADX_COMPAT_DEP = "    implementation 'androidx.appcompat:appcompat:1.3.1'";
        private const string GSON_DEP = "    implementation 'com.google.code.gson:gson:2.8.6'";
        private const string KOTLIN_DEP = "    implementation 'org.jetbrains.kotlin:kotlin-stdlib:1.5.10'";
        private const string GMS_CLASSPATH = "            classpath 'com.google.gms:google-services:4.0.2'";
        private const string FIREBASE_CRASH_CLASSPATH = "            classpath 'com.google.firebase:firebase-crashlytics-gradle:2.2.1'";
        private const string ANDROIDX = "android.useAndroidX=true";
        private const string ANDROID_JET = "android.enableJetifier=true";
        private const string FACEBOOK_LOGIN_DEP = "    implementation 'com.facebook.android:facebook-login:12.0.0'";
        private const string FACEBOOK_SHARE_DEP = "    implementation 'com.facebook.android:facebook-share:12.0.0'";
        private const string TWITTER_DEP = "    implementation 'com.twitter.sdk.android:twitter:3.3.0'";
        private const string TWITTE_COMPOSER_DEP = "    implementation 'com.twitter.sdk.android:tweet-composer:3.3.0'";
        private const string APPSFLYER_DEP = "    implementation 'com.appsflyer:af-android-sdk:6.5.2'";
        private const string APPSFLYER_WRAPPER_DEP = "    implementation 'com.appsflyer:unity-wrapper:6.5.2'";
        private const string ADJUST_DEP = "    implementation 'com.adjust.sdk:adjust-android:4.24.1'";
        private const string LINE_DEP = "    implementation 'com.linecorp:linesdk:5.0.1'";
        
        #endregion
        
        private HashSet<string> AdditionalProperties = new HashSet<string>()
        {
            ANDROIDX,
            ANDROID_JET,
        };
        
        private HashSet<string> OverseaIncludeLauncher = new HashSet<string>()
        {
            GOOGLE_GMS_PLUGIN,
            FIREBASE_CRASH_PLUGIN,
        };
        
        private HashSet<string> OverseaIncludeBase = new HashSet<string>()
        {
            GMS_CLASSPATH,
            FIREBASE_CRASH_CLASSPATH,
        };

        private HashSet<string> AlwaysIncludeDeps = new HashSet<string>()
        {
            ADX_RCV_DEP,
            GSON_DEP,
            KOTLIN_DEP,
            ADX_COMPAT_DEP
        };
        
        private HashSet<string> OverseaIncludeDeps = new HashSet<string>()
        {
            FIREBASE_CORE_DEP,
            FIREBASE_MSG_DEP,
            GMS_PLAY_DEP,
            GMS_ADS_ID_DEP,
            AD_INST_DEP,
            AD_BILL_DEP,
        };
        
        private HashSet<string> FacebookIncludeDeps = new HashSet<string>()
        {
            FACEBOOK_LOGIN_DEP,
            FACEBOOK_SHARE_DEP
        };
        
        private HashSet<string> TwitterIncludeDeps = new HashSet<string>()
        {
            TWITTER_DEP,
            TWITTE_COMPOSER_DEP
        };
        
        private HashSet<string> AppsflyerIncludeDeps = new HashSet<string>()
        {
            APPSFLYER_DEP,
            APPSFLYER_WRAPPER_DEP
        };

        private HashSet<string> AdjustIncludeDeps = new HashSet<string>()
        {
            ADJUST_DEP,
        };
        
        private HashSet<string> LineIncludeDeps = new HashSet<string>()
        {
            LINE_DEP,
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
            foreach (var dep in OverseaIncludeLauncher)
            {
                writerHelper.Delete(dep);
            }
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

            foreach (var dep in AlwaysIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            
            foreach (var dep in OverseaIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
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
            foreach (var dep in FacebookIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            foreach (var dep in TwitterIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            
            foreach (var dep in AppsflyerIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            
            foreach (var dep in AdjustIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            
            foreach (var dep in LineIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
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
            foreach (var dep in OverseaIncludeBase)
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

            bool noGoogleServices = configMd.google == null || (string.IsNullOrEmpty(configMd.google.CLIENT_ID) &&
                                                                  string.IsNullOrEmpty(configMd.google.CLIENT_ID_FOR_ANDROID));
            if (noGoogleServices)
            {
                DeleteOldGoogleContent();
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
            }
            else
            {
                var googleContext = new AndroidGradleContext();
                googleContext.locationType = AndroidGradleLocationType.Custom;
                googleContext.locationParam = "apply plugin: ['\"]com.android.application['\"]";
                googleContext.templateType = CustomTemplateType.LauncherGradle;
                googleContext.processContent = new List<string>(OverseaIncludeLauncher);
                result.Add(googleContext);
                
                var firebaseCoreDeps = new AndroidGradleContext();
                firebaseCoreDeps.locationType = AndroidGradleLocationType.Builtin;
                firebaseCoreDeps.locationParam = "DEPS";
                firebaseCoreDeps.templateType = CustomTemplateType.UnityMainGradle;
                firebaseCoreDeps.processContent = new List<string>(OverseaIncludeDeps);
                firebaseCoreDeps.processContent.AddRange(AlwaysIncludeDeps);
                result.Add(firebaseCoreDeps);

                var googleClassPathDependencies = new AndroidGradleContext();
                googleClassPathDependencies.locationType = AndroidGradleLocationType.Builtin;
                googleClassPathDependencies.locationParam = "BUILD_SCRIPT_DEPS";
                googleClassPathDependencies.templateType = CustomTemplateType.BaseGradle;
                googleClassPathDependencies.unityVersionCompatibleType = UnityVersionCompatibleType.EveryVersion;
                googleClassPathDependencies.processContent = new List<string>(OverseaIncludeBase);
                result.Add(googleClassPathDependencies);
                
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
            }

            var contents = GetGradleContextByXDConfig();
            if (contents != null) result.AddRange(contents);
            return result;
            
        }

        private List<AndroidGradleContext> GetGradleContextByXDConfig()
        {
            var result = new List<AndroidGradleContext>();
            
            var jsonPath = XDGCommonEditorUtils.GetXDConfigPath("XDConfig");
            if (!File.Exists(jsonPath))
            {
                Debug.LogError("XDConfig.json 配置文件不存在！");
                return result;
            }

            var configMd = JsonConvert.DeserializeObject<XDConfigModel>(File.ReadAllText(jsonPath));
            if (configMd == null)
            {
                Debug.LogError("XDConfig.json 解析失败！");
                return result;
            }

            var thirdPartyDeps = new AndroidGradleContext();
            thirdPartyDeps.locationType = AndroidGradleLocationType.Builtin;
            thirdPartyDeps.locationParam = "DEPS";
            thirdPartyDeps.templateType = CustomTemplateType.UnityMainGradle;
            thirdPartyDeps.processContent = new List<string>();
            
            //配置第三方库
            if (configMd.facebook != null && !string.IsNullOrEmpty(configMd.facebook.app_id))
            {
                thirdPartyDeps.processContent.AddRange(FacebookIncludeDeps); 
            }
        
            if (configMd.twitter != null && !string.IsNullOrEmpty(configMd.twitter.consumer_key))
            {
                thirdPartyDeps.processContent.AddRange(TwitterIncludeDeps); 
            }
        
            if (configMd.appsflyer != null && !string.IsNullOrEmpty(configMd.appsflyer.dev_key))
            {
                thirdPartyDeps.processContent.AddRange(AppsflyerIncludeDeps); 
            }
            
            if (configMd.adjust != null && !string.IsNullOrEmpty(configMd.adjust.app_token))
            {
                thirdPartyDeps.processContent.AddRange(AdjustIncludeDeps); 
            }
        
            if (configMd.line != null && !string.IsNullOrEmpty(configMd.line.channel_id))
            {
                thirdPartyDeps.processContent.AddRange(LineIncludeDeps); 
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