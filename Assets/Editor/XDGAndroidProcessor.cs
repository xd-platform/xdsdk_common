#if UNITY_EDITOR && UNITY_ANDROID
using System.IO;
using UnityEditor.Android;
using UnityEngine;
using XD.SDK.Common.Editor;

public class XDGAndroidProcessor : IPostGenerateGradleAndroidProject{
    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path){
        var projectPath = path;
        if (path.Contains("unityLibrary")){
            projectPath = path.Substring(0, path.Length - 12);
        }
        var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
        
        //拷贝 google-services 可选
        var googleJsonPath = parentFolder + "/Assets/Plugins/google-services.json";
        if (File.Exists(googleJsonPath)){
            Debug.Log("拷贝谷歌 google-services");
            File.Copy(googleJsonPath, projectPath + "/launcher/google-services.json");
            File.Copy(googleJsonPath, projectPath + "/unityLibrary/src/main/assets/google-services.json");
        }
        
        //拷贝 SDK json 文件，必须的
        var configJson = parentFolder + "/Assets/Plugins/XDConfig.json";
        if (File.Exists(configJson)){
            File.Copy(configJson, projectPath + "/unityLibrary/src/main/assets/XDConfig.json");   
        } else{
            Debug.LogError("打包失败 ---  拷贝的json配置文件不存在");
            return;
        }

        //Demo用的，游戏不用配置这个 XDConfig-cn.json
        var cnJson = parentFolder + "/Assets/Plugins/XDConfig-cn.json";
        if (File.Exists(cnJson)){
            File.Copy(cnJson, projectPath + "/unityLibrary/src/main/assets/XDConfig-cn.json");   
        }
       
        
        //配置路径
        var gradlePropertiesFile = projectPath + "/gradle.properties";
        var baseProjectGradle = projectPath + "/build.gradle";
        var launcherGradle = projectPath + "/launcher/build.gradle";
        var unityLibraryGradle = projectPath + "/unityLibrary/build.gradle";
        
        //apply plugin 可根据需要添加或删除
        if (File.Exists(launcherGradle)){
            Debug.Log("编辑 launcherGradle");
            var writerHelper = new XD.SDK.Common.Editor.XDGScriptHandlerProcessor(launcherGradle);
            writerHelper.WriteBelow(@"apply plugin: 'com.android.application'", @"
                apply plugin: 'com.google.gms.google-services'
                apply plugin: 'com.google.firebase.crashlytics'
            ");
        }

        //classpath 可根据需要添加或删除
        if (File.Exists(baseProjectGradle)){
            Debug.Log("编辑 baseProjectGradle");
            var writerHelper = new XD.SDK.Common.Editor.XDGScriptHandlerProcessor(baseProjectGradle);
            writerHelper.WriteBelow(@"task clean(type: Delete) {
    delete rootProject.buildDir
}", @"allprojects {
    buildscript {
        dependencies {
            classpath 'com.google.gms:google-services:4.0.2'
            classpath 'com.google.firebase:firebase-crashlytics-gradle:2.2.1'
        }
    }
}");
        }

        //implementation 可根据需要添加或删除
        if (File.Exists(unityLibraryGradle)){
            Debug.Log("编辑 unityLibraryGradle");
            var writerHelper = new XD.SDK.Common.Editor.XDGScriptHandlerProcessor(unityLibraryGradle);
            writerHelper.WriteBelow(@"implementation fileTree(dir: 'libs', include: ['*.jar'])", @"

                implementation 'com.google.firebase:firebase-core:18.0.0'
                implementation 'com.google.firebase:firebase-messaging:21.1.0'
                implementation 'com.google.code.gson:gson:2.8.6'
                implementation 'com.google.android.gms:play-services-auth:16.0.1'
                implementation 'com.google.android.gms:play-services-ads-identifier:15.0.1'

                implementation 'com.facebook.android:facebook-login:12.0.0'
                implementation 'com.facebook.android:facebook-share:12.0.0'

                implementation 'com.appsflyer:af-android-sdk:6.5.2'
                implementation 'com.appsflyer:unity-wrapper:6.5.2'

                implementation 'com.adjust.sdk:adjust-android:4.24.1'

                implementation 'com.twitter.sdk.android:twitter:3.3.0'
                implementation 'com.twitter.sdk.android:tweet-composer:3.3.0'

                implementation 'com.linecorp:linesdk:5.0.1'

                implementation 'androidx.appcompat:appcompat:1.3.1'
                implementation 'com.android.installreferrer:installreferrer:2.2'
                implementation 'com.android.billingclient:billing:3.0.0'
                implementation 'androidx.recyclerview:recyclerview:1.2.1'
            ");
        }
        
        //需要
        if (File.Exists(gradlePropertiesFile)){
            Debug.Log("编辑 gradlePropertiesFile");
            if (File.Exists(gradlePropertiesFile)){
                var writeHelper = new XDGScriptHandlerProcessor(gradlePropertiesFile);
                writeHelper.WriteBelow(@"org.gradle.jvmargs=-Xmx4096M", @"
android.useAndroidX=true
android.enableJetifier=true");
            }
        }
    }

    public int callbackOrder{
        get{ return 999; }
    }
}
#endif