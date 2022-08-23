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
        
        addFile(projectPath, "XDConfig-cn");
        addFile(projectPath, "XDConfig-cn-release");
        addFile(projectPath, "XDConfig-release");
    }

    private void addFile(string projectPath, string name){
        var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
        var cnJson = parentFolder + "/Assets/Plugins/" + name + ".json";
        if (File.Exists(cnJson)){
            File.Copy(cnJson, projectPath + "/unityLibrary/src/main/assets/" + name + ".json", true);
        }
    }

    public int callbackOrder{
        get{ return 888; }
    }
}
#endif