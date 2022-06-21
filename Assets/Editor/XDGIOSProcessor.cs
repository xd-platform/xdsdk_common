#if UNITY_EDITOR && UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class XDGIOSProcessor{
    [PostProcessBuild(200)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string path){
        if (BuildTarget == BuildTarget.iOS){
            var parentPath = Directory.GetParent(Application.dataPath)?.FullName;

            //海外配置文件 
            var jsonPath_IO = parentPath + "/Assets/Plugins/XDConfig.json";
            XDGIOSCommonProcessor.SetThirdLibraryId(path, jsonPath_IO, false); //海外

            //国内配置文件，国内海外二选一或者都配置
            var jsonPath_CN = parentPath + "/Assets/Plugins/XDConfig-cn.json";
            XDGIOSCommonProcessor.SetThirdLibraryId(path, jsonPath_CN, true); //国内
        }
    }
}

#endif