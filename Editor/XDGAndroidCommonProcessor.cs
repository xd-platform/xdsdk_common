#if UNITY_EDITOR && UNITY_ANDROID
using System.IO;
using UnityEditor.Android;
using UnityEngine;

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
        Debug.Log("修改 gradlePropertiesFile");
        // Creating a file
        var gradlePropertiesFile = projectPath + "/gradle.properties";
        bool containsAndroidX = false;
        // Opening the file for reading
        using(StreamReader sr = File.OpenText(gradlePropertiesFile))
        {
            string s = "";
            s = sr.ReadToEnd();
            containsAndroidX = s.Contains("android.useAndroidX=true") && s.Contains("android.enableJetifier=true");
        }

        if (false == containsAndroidX)
        {
            using(StreamWriter sw = File.AppendText(gradlePropertiesFile))
            {
                sw.WriteLine("android.useAndroidX=true");
                sw.WriteLine("android.enableJetifier=true");
            }
        }
#endif
    }

    public int callbackOrder{
        get{ return 998; }
    }
}
#endif