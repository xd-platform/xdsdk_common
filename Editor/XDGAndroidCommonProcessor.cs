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
        var configJson = parentFolder + "/Assets/Plugins/Resources/XDConfig.json";
        if (File.Exists(configJson)){
        #if UNITY_2019_3_OR_NEWER
            File.Copy(configJson, projectPath + "/unityLibrary/src/main/assets/XDConfig.json", true);
        #else
            File.Copy(configJson, projectPath + "/src/main/assets/XDConfig.json", true);
        #endif
        } else{
            Debug.LogWarning("打包警告 ---  拷贝的json配置文件不存在");
        }
        
#if !UNITY_2019_3_OR_NEWER
        //配置路径
        var gradlePropertiesFile = projectPath + "/gradle.properties";
        
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
        writer.Flush();
        writer.Close();
#endif
    }

    public int callbackOrder{
        get{ return 998; }
    }
}
#endif