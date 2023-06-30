#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LC.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using TapTap.AndroidDependencyResolver.Editor;
using XD.SDK.Common.Editor;

namespace XD.SDK.Oversea.Editor
{
    public class XDGOverseaGradleProvider : IPreprocessBuildWithReport
    {
        public int callbackOrder => AndroidGradleProcessor.CALLBACK_ORDER - 50;

        private string ProviderFilePath => Path.Combine(Application.dataPath, "XDSDK", "Gen", "Oversea");
        
        #region Gradle Content String

        private const string FIREBASE_GMS_PLUGIN = "apply plugin: 'com.google.gms.google-services'";
        private const string FIREBASE_CRASH_PLUGIN = "apply plugin: 'com.google.firebase.crashlytics'";
        private const string FIREBASE_CORE_DEP = "    implementation 'com.google.firebase:firebase-core:18.0.0'";
        private const string FIREBASE_ANALYTICS_DEP = "    implementation \"com.google.firebase:firebase-analytics:21.3.0\"";
        private const string FIREBASE_MSG_DEP = "    implementation 'com.google.firebase:firebase-messaging:21.1.0'";
        // 不是必须要 Google-Services.json
        private const string GOOGLE_AUTH_DEP = "    implementation 'com.google.android.gms:play-services-auth:16.0.1'";
        private const string GMS_ADS_ID_DEP = "    implementation 'com.google.android.gms:play-services-ads-identifier:15.0.1'";
        private const string AD_BILL_DEP = "    implementation 'com.android.billingclient:billing:4.1.0'";
        private const string GMS_CLASSPATH = "            classpath 'com.google.gms:google-services:4.0.2'";
        private const string FIREBASE_CRASH_CLASSPATH = "            classpath 'com.google.firebase:firebase-crashlytics-gradle:2.2.1'";
        private const string FACEBOOK_LOGIN_DEP = "    implementation 'com.facebook.android:facebook-login:12.0.0'";
        private const string FACEBOOK_SHARE_DEP = "    implementation 'com.facebook.android:facebook-share:12.0.0'";
        private const string TWITTER_DEP = "    implementation 'com.twitter.sdk.android:twitter:3.3.0'";
        private const string TWITTE_COMPOSER_DEP = "    implementation 'com.twitter.sdk.android:tweet-composer:3.3.0'";
        private const string AF_INST_DEP = "    implementation 'com.android.installreferrer:installreferrer:2.2'";
        private const string APPSFLYER_DEP = "    implementation 'com.appsflyer:af-android-sdk:6.5.2'";
        private const string APPSFLYER_WRAPPER_DEP = "    implementation 'com.appsflyer:unity-wrapper:6.5.2'";
        private const string ADJUST_DEP = "    implementation 'com.adjust.sdk:adjust-android:4.24.1'";
        private const string LINE_DEP = "    implementation 'com.linecorp:linesdk:5.0.1'";
        
        #endregion

        private HashSet<string> OBSOLETE_DEPS = new HashSet<string>()
        {
            FIREBASE_CORE_DEP,
        };
        private HashSet<string> FirebaseIncludeLauncher = new HashSet<string>()
        {
            FIREBASE_CRASH_PLUGIN,
            FIREBASE_GMS_PLUGIN,
        };
        
        private HashSet<string> FirebaseIncludeBase = new HashSet<string>()
        {
            GMS_CLASSPATH,
            FIREBASE_CRASH_CLASSPATH,
        };

        private HashSet<string> OverseaIncludeDeps = new HashSet<string>()
        {
            GMS_ADS_ID_DEP,
            AD_BILL_DEP,
        };
        
        private HashSet<string> GoogleIncludeDeps = new HashSet<string>()
        {
            GOOGLE_AUTH_DEP,
        };

        private HashSet<string> FirebaseIncludeDeps = new HashSet<string>()
        {
            FIREBASE_ANALYTICS_DEP,
            FIREBASE_MSG_DEP,
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
            APPSFLYER_WRAPPER_DEP,
            AF_INST_DEP
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
        
        public static void OverseaRefresh()
        {
            var temp = new XDGOverseaGradleProvider();
            temp.OnPreprocessBuild(null);
        }

        private AndroidGradleContextProvider FixProvider()
        {
            AndroidGradleContextProvider result = new AndroidGradleContextProvider();
            result.Version = 1;
            result.Priority = 3;
            result.Use = true;
            result.ModuleName = "XD.Oversea";
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
        
        private void DeleteOldOverseaContent()
        {
            var pluginsFolder = Path.Combine(Application.dataPath, "Plugins", "Android");
            DeleteMainOldOverseaContent(pluginsFolder);
            DeleteThirdPartyContent(pluginsFolder);
        }

        private void DeleteOldGoogleContent()
        {
            var pluginsFolder = Path.Combine(Application.dataPath, "Plugins", "Android");
            DeleteMainOldGoogleContent(pluginsFolder);
        }

        private void DeleteOldFirebaseContent()
        {
            var pluginsFolder = Path.Combine(Application.dataPath, "Plugins", "Android");
            DeleteMainOldFirebaseContent(pluginsFolder);
        }

        private void DeleteObsoleteContent()
        {
            var pluginsFolder = Path.Combine(Application.dataPath, "Plugins", "Android");
            var gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle");
            if (!File.Exists(gradleTemplate))
            {
                gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle.DISABLED");
            }
            if (!File.Exists(gradleTemplate)) return;
            var writerHelper = new XDGScriptHandlerProcessor(gradleTemplate);
            
            foreach (var dep in OBSOLETE_DEPS)
            {
                writerHelper.Delete(dep);
            }
            writerHelper.Dispose();
        }
        
        private void DeleteMainOldOverseaContent(string pluginsFolder)
        {
            var gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle");
            if (!File.Exists(gradleTemplate))
            {
                gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle.DISABLED");
            }
            if (!File.Exists(gradleTemplate)) return;
            var writerHelper = new XDGScriptHandlerProcessor(gradleTemplate);
            
            foreach (var dep in OverseaIncludeDeps)
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
            
            foreach (var dep in GoogleIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            writerHelper.Dispose();
        }
        
        private void DeleteMainOldFirebaseContent(string pluginsFolder)
        {
            var gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle");
            if (!File.Exists(gradleTemplate))
            {
                gradleTemplate = Path.Combine(pluginsFolder, "mainTemplate.gradle.DISABLED");
            }
            if (!File.Exists(gradleTemplate)) return;
            var writerHelper = new XDGScriptHandlerProcessor(gradleTemplate);
            
            foreach (var dep in FirebaseIncludeDeps)
            {
                writerHelper.Delete(dep);
            }
            writerHelper.Dispose();
            
            var launcherTemplate = Path.Combine(pluginsFolder, "launcherTemplate.gradle");
            if (!File.Exists(launcherTemplate))
            {
                launcherTemplate = Path.Combine(pluginsFolder, "launcherTemplate.gradle.DISABLED");
            }
            if (!File.Exists(launcherTemplate)) return;
            writerHelper = new XDGScriptHandlerProcessor(launcherTemplate);
            
            foreach (var dep in FirebaseIncludeLauncher)
            {
                writerHelper.Delete(dep);
            }
            foreach (var dep in FirebaseIncludeBase)
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
            DeleteObsoleteContent();
            DeleteOldOverseaContent();
            DeleteOldFirebaseContent();
            DeleteOldGoogleContent();

            if (XDGCommonGradleProvider.IsNeedOverseaDep(configMd))
            {
                var firebaseCoreDeps = new AndroidGradleContext();
                firebaseCoreDeps.locationType = AndroidGradleLocationType.Builtin;
                firebaseCoreDeps.locationParam = "DEPS";
                firebaseCoreDeps.templateType = CustomTemplateType.UnityMainGradle;
                firebaseCoreDeps.processContent = new List<string>(OverseaIncludeDeps);
                result.Add(firebaseCoreDeps);
            }

            if (XDGCommonGradleProvider.IsNeedFirebaseDep(configMd))
            {
                var firebaseLancherDeps = new AndroidGradleContext();
                firebaseLancherDeps.locationType = AndroidGradleLocationType.Custom;
                firebaseLancherDeps.locationParam = "apply plugin: ['\"]com.android.application['\"]";
                firebaseLancherDeps.templateType = CustomTemplateType.LauncherGradle;
                firebaseLancherDeps.processContent = new List<string>(FirebaseIncludeLauncher);
                result.Add(firebaseLancherDeps);
                    
                var firebaseClassPathDependencies = new AndroidGradleContext();
                firebaseClassPathDependencies.locationType = AndroidGradleLocationType.Builtin;
                firebaseClassPathDependencies.locationParam = "BUILD_SCRIPT_DEPS";
                firebaseClassPathDependencies.templateType = CustomTemplateType.BaseGradle;
                firebaseClassPathDependencies.unityVersionCompatibleType = UnityVersionCompatibleType.EveryVersion;
                firebaseClassPathDependencies.processContent = new List<string>(FirebaseIncludeBase);
                result.Add(firebaseClassPathDependencies);
                
                var firebaseIncludeDeps = new AndroidGradleContext();
                firebaseIncludeDeps.locationType = AndroidGradleLocationType.Builtin;
                firebaseIncludeDeps.locationParam = "DEPS";
                firebaseIncludeDeps.templateType = CustomTemplateType.UnityMainGradle;
                firebaseIncludeDeps.processContent = new List<string>();
                firebaseIncludeDeps.processContent.AddRange(FirebaseIncludeDeps); 
                result.Add(firebaseIncludeDeps);
            }
            
            //配置第三方库
            if (XDGCommonGradleProvider.IsNeedGoogleDep(configMd))
            {
                var googleIncludeDeps = new AndroidGradleContext();
                googleIncludeDeps.locationType = AndroidGradleLocationType.Builtin;
                googleIncludeDeps.locationParam = "DEPS";
                googleIncludeDeps.templateType = CustomTemplateType.UnityMainGradle;
                googleIncludeDeps.processContent = new List<string>();
                googleIncludeDeps.processContent.AddRange(GoogleIncludeDeps); 
                result.Add(googleIncludeDeps);
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