#if UNITY_EDITOR && UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using XD.SDK.Common.Editor;
using LC.Newtonsoft.Json;

public static class XDGIOSProcessor{
    [PostProcessBuild(200)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string path){
        if (BuildTarget == BuildTarget.iOS){
            var parentPath = Directory.GetParent(Application.dataPath)?.FullName;

            //国内配置文件 Demo用
            var jsonPath_CN = parentPath + "/Assets/Plugins/XDConfig-cn.json";
            SetCNConfig(path, jsonPath_CN); 
        }
    }

    private static void SetCNConfig(string path, string configJsonPath){
        var projPath = PBXProject.GetPBXProjectPath(path);
        var proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));

        // 2019.3 以上有多个target
#if UNITY_2019_3_OR_NEWER
        string unityFrameworkTarget = proj.TargetGuidByName("UnityFramework");
        string target = proj.GetUnityMainTargetGuid();
#else
                string unityFrameworkTarget = proj.TargetGuidByName("Unity-iPhone");
                string target = proj.TargetGuidByName("Unity-iPhone");
#endif


        if (File.Exists(configJsonPath)){
            var folderPath = Path.Combine(path, "SDKResource");
            if (!File.Exists(folderPath)){
                Directory.CreateDirectory(folderPath);
            }

            var json = File.ReadAllText(configJsonPath);
            var md = JsonConvert.DeserializeObject<XDGConfigModel>(json);
            if (md == null){
                Debug.LogError("json 配置文件解析失败: " + configJsonPath);
                return;
            }

            var filePath = Path.Combine(folderPath, "XDConfig-cn.json");
            File.Copy(configJsonPath, filePath);

            XDGIOSCommonProcessor.AddXcodeConfig(target, proj, filePath); //先拷贝，后配置
            SetThirdLibraryId_CN(path, md);

            File.WriteAllText(projPath, proj.WriteToString()); //保存
        } else{
            Debug.LogError("json 配置文件不存在: " + configJsonPath);
        }
    }

    private static void SetThirdLibraryId_CN(string pathToBuildProject, XDGConfigModel configModel){
        if (configModel == null){
            Debug.LogError("打包失败  ----  XDConfig-cn 配置文件Model是空");
            return;
        }

        var _plistPath = pathToBuildProject + "/Info.plist"; //Xcode工程的plist
        var _plist = new PlistDocument();
        _plist.ReadFromString(File.ReadAllText(_plistPath));
        var _rootDic = _plist.root;

        var taptapId = configModel.tapsdk.client_id;

        //添加url 用添加，不要覆盖
        PlistElementDict dict = _plist.root.AsDict();
        PlistElementArray array = null;
        foreach (var item in _rootDic.values){
            if (item.Key.Equals("CFBundleURLTypes")){
                array = (PlistElementArray) item.Value;
                break;
            }
        }

        if (array == null){
            array = dict.CreateArray("CFBundleURLTypes");
        }

        PlistElementDict dict2 = array.AddDict();
        if (taptapId != null){
            dict2.SetString("CFBundleURLName", "TapCN");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString($"tt{taptapId}");
        }

        File.WriteAllText(_plistPath, _plist.WriteToString());
    }
}

#endif