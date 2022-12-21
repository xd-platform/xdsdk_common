#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.Editor
{
    public class XDGCommonGradleContextProvider : IAndroidGradleContextProvider
    {
        public int Priority
        {
            get => 1;
        }
        
        public List<XDGAndroidGradleContext> GetAndroidGradleContext()
        {
            var result = new List<XDGAndroidGradleContext>();
            
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            var googleJsonPath = parentFolder + "/Assets/Plugins/Android/google-services.json";
            if (File.Exists(googleJsonPath) == false)
            {
                var googleDeps = new XDGAndroidGradleContext();
                googleDeps.locationType = AndroidGradleLocationType.Builtin;
                googleDeps.locationParam = "DEPS";
                googleDeps.templateType = CustomTemplateType.UnityMainGradle;
                googleDeps.processContent = @"    implementation 'com.google.android.gms:play-services-auth:16.0.1'";
                result.Add(googleDeps);
            }
            else
            {
                var googleContext = new XDGAndroidGradleContext();
                googleContext.locationType = AndroidGradleLocationType.Custom;
                googleContext.locationParam = "apply plugin: ['\"]com.android.application['\"]";
                googleContext.templateType = CustomTemplateType.LauncherGradle;
                googleContext.processContent = @"apply plugin: 'com.google.gms.google-services'
apply plugin: 'com.google.firebase.crashlytics'";
                result.Add(googleContext);
                
                 var googleDeps = new XDGAndroidGradleContext();
                 googleDeps.locationType = AndroidGradleLocationType.Builtin;
                 googleDeps.locationParam = "DEPS";
                 googleDeps.templateType = CustomTemplateType.UnityMainGradle;
                 googleDeps.processContent = @"    implementation 'com.google.firebase:firebase-core:18.0.0'
    implementation 'com.google.firebase:firebase-messaging:21.1.0'
    implementation 'com.google.android.gms:play-services-auth:16.0.1'
    implementation 'com.google.android.gms:play-services-ads-identifier:15.0.1'";
                 result.Add(googleDeps);
    
#if UNITY_2019_1_OR_NEWER
                 var googleDependencies = new XDGAndroidGradleContext();
                 googleDependencies.locationType = AndroidGradleLocationType.End;
                 googleDependencies.templateType = CustomTemplateType.BaseGradle;
                 googleDependencies.processContent = @"allprojects {
    buildscript {
        dependencies {
            classpath 'com.google.gms:google-services:4.0.2'
            classpath 'com.google.firebase:firebase-crashlytics-gradle:2.2.1'
        }
    }
}";
                 result.Add(googleDependencies);
#else
                var googleDependencies = new XDGAndroidGradleContext();
                googleDependencies.locationType = AndroidGradleLocationType.Builtin;
                googleDependencies.locationParam = "BUILD_SCRIPT_DEPS";
                googleDependencies.templateType = CustomTemplateType.BaseGradle;
                googleDependencies.processContent = @"        classpath 'com.google.gms:google-services:4.0.2'
        classpath 'com.google.firebase:firebase-crashlytics-gradle:2.2.1'";
                result.Add(googleDependencies);
#endif
                
#if UNITY_2019_1_OR_NEWER
                // Unity 2018 不支持自定义gradle.properties
                var gradlePropertiesContext = new XDGAndroidGradleContext();
                gradlePropertiesContext.locationType = AndroidGradleLocationType.Builtin;
                gradlePropertiesContext.locationParam = "ADDITIONAL_PROPERTIES";
                gradlePropertiesContext.templateType = CustomTemplateType.GradleProperties;
                gradlePropertiesContext.processContent = @"android.useAndroidX=true
    android.enableJetifier=true";
                result.Add(gradlePropertiesContext);
#endif

                var commonDepContext = new XDGAndroidGradleContext();
                commonDepContext.locationType = AndroidGradleLocationType.Builtin;
                commonDepContext.locationParam = "DEPS";
                commonDepContext.templateType = CustomTemplateType.UnityMainGradle;
                commonDepContext.processContent = @"    implementation 'com.android.installreferrer:installreferrer:2.2'
    implementation 'com.android.billingclient:billing:4.1.0'
    implementation 'androidx.recyclerview:recyclerview:1.2.1'
    implementation 'com.google.code.gson:gson:2.8.6'";
                result.Add(commonDepContext);
        
                var androidPluginContext = new XDGAndroidGradleContext();
                androidPluginContext.locationType = AndroidGradleLocationType.Custom;
                androidPluginContext.locationParam = "classpath 'com.android.tools.build:gradle:\\d{1}.\\d{1}.\\d{1}'";
                androidPluginContext.templateType = CustomTemplateType.BaseGradle;
                androidPluginContext.processType = AndroidGradleProcessType.Replace;
                androidPluginContext.processContent = @"classpath 'com.android.tools.build:gradle:4.0.1'";
                result.Add(androidPluginContext);
            }

            var contents = GetGradleContextByXDConfig();
            if (contents != null) result.AddRange(contents);
            return result;
        }
        
        private List<XDGAndroidGradleContext> GetGradleContextByXDConfig()
        {
            var result = new List<XDGAndroidGradleContext>();
            
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            var jsonPath = parentFolder + "/Assets/Plugins/Mobile/XDConfig.json";
            if (!File.Exists(jsonPath))
            {
                Debug.LogError("/Assets/Plugins/Mobile/XDConfig.json 配置文件不存在！");
                return result;
            }

            
            var configMd = JsonConvert.DeserializeObject<XDConfigModel>(File.ReadAllText(jsonPath));
            if (configMd == null)
            {
                Debug.LogError("/Assets/Plugins/Mobile/XDConfig.json 解析失败！");
                return result;
            }

            //配置第三方库
            if (configMd.facebook != null && !string.IsNullOrEmpty(configMd.facebook.app_id))
            {
                var facebookContent = new XDGAndroidGradleContext();
                facebookContent.locationType = AndroidGradleLocationType.Builtin;
                facebookContent.locationParam = "DEPS";
                facebookContent.templateType = CustomTemplateType.UnityMainGradle;
                facebookContent.processContent = @"    implementation 'com.facebook.android:facebook-login:12.0.0'
    implementation 'com.facebook.android:facebook-share:12.0.0'";
                result.Add(facebookContent);
            }
        
            if (configMd.twitter != null && !string.IsNullOrEmpty(configMd.twitter.consumer_key))
            {
                var twitterContent = new XDGAndroidGradleContext();
                twitterContent.locationType = AndroidGradleLocationType.Builtin;
                twitterContent.locationParam = "DEPS";
                twitterContent.templateType = CustomTemplateType.UnityMainGradle;
                twitterContent.processContent = @"    implementation 'com.twitter.sdk.android:twitter:3.3.0'
    implementation 'com.twitter.sdk.android:tweet-composer:3.3.0'";
                result.Add(twitterContent);
            }
        
            if (configMd.appsflyer != null && !string.IsNullOrEmpty(configMd.appsflyer.dev_key))
            {
                var appsflyerContent = new XDGAndroidGradleContext();
                appsflyerContent.locationType = AndroidGradleLocationType.Builtin;
                appsflyerContent.locationParam = "DEPS";
                appsflyerContent.templateType = CustomTemplateType.UnityMainGradle;
                appsflyerContent.processContent = @"    implementation 'com.appsflyer:af-android-sdk:6.5.2'
    implementation 'com.appsflyer:unity-wrapper:6.5.2'";
                result.Add(appsflyerContent);
            }
        
            if (configMd.adjust != null && !string.IsNullOrEmpty(configMd.adjust.app_token))
            {
                var adjustContent = new XDGAndroidGradleContext();
                adjustContent.locationType = AndroidGradleLocationType.Builtin;
                adjustContent.locationParam = "DEPS";
                adjustContent.templateType = CustomTemplateType.UnityMainGradle;
                adjustContent.processContent = @"    implementation 'com.adjust.sdk:adjust-android:4.24.1'";
                result.Add(adjustContent);
            }
        
            if (configMd.line != null && !string.IsNullOrEmpty(configMd.line.channel_id))
            {
                var lineContent = new XDGAndroidGradleContext();
                lineContent.locationType = AndroidGradleLocationType.Builtin;
                lineContent.locationParam = "DEPS";
                lineContent.templateType = CustomTemplateType.UnityMainGradle;
                lineContent.processContent = @"    implementation 'com.linecorp:linesdk:5.0.1'";
                result.Add(lineContent);
            }

            return result;
        }
    }
}

#endif