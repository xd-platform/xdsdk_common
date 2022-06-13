#if UNITY_EDITOR && UNITY_ANDROID
using System.IO;
#if UNITY_ANDROID
using UnityEditor.Android;
#endif
using UnityEngine;
using XD.SDK.Common.Editor;

public class XDGAndroidProcessor : IPostGenerateGradleAndroidProject{
    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path){
        var projectPath = path;
        if (path.Contains("unityLibrary")){
            projectPath = path.Substring(0, path.Length - 12);
        }
        
        var googleServiceOriginPath = projectPath + "/unityLibrary/src/main/assets/google-services.json";
        if (File.Exists(googleServiceOriginPath)){
            Debug.Log("拷贝谷歌 google-services");
            File.Copy(googleServiceOriginPath, projectPath + "/launcher/google-services.json", true);
        }

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

                implementation 'com.facebook.android:facebook-login:12.0.0'
                implementation 'com.facebook.android:facebook-share:12.0.0'
                implementation 'com.appsflyer:af-android-sdk:6.5.2'
                implementation 'com.appsflyer:unity-wrapper:6.5.2'

                implementation 'com.adjust.sdk:adjust-android:4.24.1'
                implementation 'com.android.installreferrer:installreferrer:2.2'
                implementation 'com.android.billingclient:billing:3.0.0'
                implementation 'androidx.recyclerview:recyclerview:1.2.1'
                implementation 'com.google.android.gms:play-services-ads-identifier:15.0.1'
                implementation 'com.twitter.sdk.android:twitter:3.3.0'
                implementation 'com.twitter.sdk.android:tweet-composer:3.3.0'
                implementation 'com.linecorp:linesdk:5.0.1'
                implementation 'androidx.appcompat:appcompat:1.3.1'
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