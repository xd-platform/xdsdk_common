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
            var providers = AndroidUtils.Load();
            if (providers == null) return;
            foreach (var provider in providers.Where(provider => provider.ModuleName.Contains("XD.Common")))
            {
                FixProvider(provider);
                SaveProvider(provider);
                break;
            }
        }

        [MenuItem("XD/Common/Refresh Android Gradle")]
        public static void CommonRefresh()
        {
            var temp = new XDGCommonGradleProvider();
            temp.OnPreprocessBuild(null);
        }

        private void FixProvider(AndroidGradleContextProvider provider)
        {
            var tmp = GetGradleContext();
            Debug.LogFormat($"tmp count: {tmp.Count}");
            provider.AndroidGradleContext = tmp;
        }

        private void SaveProvider(AndroidGradleContextProvider provider)
        {
            var path = Path.Combine(Application.dataPath, "XDSDK", "Mobile", "Common", "TapAndroidProvider.txt");
            AndroidUtils.SaveProvider(path, provider);
        }

        public List<AndroidGradleContext> GetGradleContext()
        {
            var result = new List<AndroidGradleContext>();

            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            var googleJsonPath = parentFolder + "/Assets/Plugins/Android/google-services.json";
            if (File.Exists(googleJsonPath) == false)
            {
                var googleDeps = new AndroidGradleContext();
                googleDeps.locationType = AndroidGradleLocationType.Builtin;
                googleDeps.locationParam = "DEPS";
                googleDeps.templateType = CustomTemplateType.UnityMainGradle;
                googleDeps.processContent = new List<string>()
                {
                    @"    implementation 'com.google.android.gms:play-services-auth:16.0.1'"
                };
                result.Add(googleDeps);
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
            
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            var jsonPath = parentFolder + "/Assets/Plugins/Resources/XDConfig.json";
            if (!File.Exists(jsonPath))
            {
                Debug.LogError("/Assets/Plugins/Resources/XDConfig.json 配置文件不存在！");
                return result;
            }

            var configMd = JsonConvert.DeserializeObject<XDConfigModel>(File.ReadAllText(jsonPath));
            if (configMd == null)
            {
                Debug.LogError("/Assets/Plugins/Resources/XDConfig.json 解析失败！");
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