#if UNITY_EDITOR && UNITY_ANDROID
using System.IO;
using UnityEditor.Android;
using UnityEngine;
using XD.SDK.Common.Editor;

public class XDGAndroidCommonProcessor : IPostGenerateGradleAndroidProject{
    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path){
        var projectPath = path;
        if (path.Contains("unityLibrary")){
            projectPath = path.Substring(0, path.Length - 12);
        }

        var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;

        //拷贝 SDK json 文件，必须的
        var configJson = parentFolder + "/Assets/Plugins/XDConfig.json";
        if (File.Exists(configJson)){
        #if UNITY_2019_1_OR_NEWER
            File.Copy(configJson, projectPath + "/unityLibrary/src/main/assets/XDConfig.json", true);
        #else
            File.Copy(configJson, projectPath + "/src/main/assets/XDConfig.json", true);
        #endif
        } else{
            Debug.LogError("打包失败 ---  拷贝的json配置文件不存在");
            return;
        }

        //配置路径
        var gradlePropertiesFile = projectPath + "/gradle.properties";
        var baseProjectGradle = projectPath + "/build.gradle";
        var launcherGradle = projectPath + "/launcher/build.gradle";
#if UNITY_2019_1_OR_NEWER
        var unityLibraryGradle = projectPath + "/unityLibrary/build.gradle";
#else
        var unityLibraryGradle = projectPath + "/build.gradle";
#endif

        //implementation 可根据需要添加或删除
        if (File.Exists(unityLibraryGradle))
        {
            Debug.Log("编辑 unityLibraryGradle");
            var writerHelper = new XD.SDK.Common.Editor.XDGScriptHandlerProcessor(unityLibraryGradle);
            writerHelper.WriteBelow(@"implementation fileTree(dir: 'libs', include: ['*.jar'])", @"
    implementation 'com.android.installreferrer:installreferrer:2.2'
    implementation 'com.android.billingclient:billing:4.1.0'
    implementation 'androidx.recyclerview:recyclerview:1.2.1'
    implementation 'com.google.code.gson:gson:2.8.6'
            ");
        }
        else
        {
            Debug.LogWarning("打包警告 ---  unityLibraryGradle 不存在");
        }

        //需要
        if (File.Exists(gradlePropertiesFile))
        {
            File.Delete(gradlePropertiesFile);
        }
        Debug.Log("创建 gradlePropertiesFile");
        StreamWriter writer = File.CreateText(gradlePropertiesFile);
        writer.WriteLine("org.gradle.jvmargs=-Xmx4096M");
        writer.WriteLine("android.useAndroidX=true");
        writer.WriteLine("android.enableJetifier=true");
        writer.WriteLine("org.gradle.parallel=true");
        writer.WriteLine("android.enableR8=false");
        writer.WriteLine("unityStreamingAssets=.unity3d");
        writer.Flush();
        writer.Close();
    }

    public int callbackOrder{
        get{ return 998; }
    }
}
#endif