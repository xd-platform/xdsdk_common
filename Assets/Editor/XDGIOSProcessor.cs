#if UNITY_EDITOR && UNITY_IOS
using System.IO;
using LC.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using XD.SDK.Common.Editor;

public static class XDGIOSProcessor{
    [PostProcessBuild(200)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string path){
        if (BuildTarget == BuildTarget.iOS){
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
            var parentPath = Directory.GetParent(Application.dataPath)?.FullName;
            
            //创建文件夹
            var resourcePath = Path.Combine(path, "SDKResource");
            if (!File.Exists(resourcePath)){
                Directory.CreateDirectory(resourcePath);
            }
            
            //拷贝资源文件, 根据需要添加修改list, 国内不要谷歌Line等
            XDGIOSCommonProcessor.CopyThirdResource(target, projPath, proj, parentPath, resourcePath,
                new[]{"LineSDKResource.bundle", "GoogleSignIn.bundle", "XDConfig.json", "XDConfig-cn.json","GoogleService-Info.plist"});  //XDConfig.json SDk内部固定值

            //json 配置
            var jsonPath_IO = parentPath + "/Assets/Plugins/XDConfig.json";  
            var jsonPath_CN = parentPath + "/Assets/Plugins/XDConfig-cn.json";  
            XDGIOSCommonProcessor.SetThirdLibraryId(path, jsonPath_IO, false); //海外
            XDGIOSCommonProcessor.SetThirdLibraryId(path, jsonPath_CN, true);  //国内
            
        }
    }
}

#endif