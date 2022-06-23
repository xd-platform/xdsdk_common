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

        //implementation 可根据需要添加或删除
        if (File.Exists(unityLibraryGradle)){
            Debug.Log("编辑 unityLibraryGradle");
            var writerHelper = new XD.SDK.Common.Editor.XDGScriptHandlerProcessor(unityLibraryGradle);
            writerHelper.WriteBelow(@"implementation fileTree(dir: 'libs', include: ['*.jar'])", @"
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
        get{ return 998; }
    }
}
#endif