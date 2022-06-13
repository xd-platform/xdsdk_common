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
            var jsonPath = parentPath + "/Assets/Plugins/iOS/Resource/XDConfig.json"; //国外
            var jsonPath_CN = parentPath + "/Assets/Plugins/iOS/Resource/XDConfig-cn.json";  //国内
            
            //创建文件夹
            var resourcePath = Path.Combine(path, "SDKResource");
            if (!File.Exists(resourcePath)){
                Directory.CreateDirectory(resourcePath);
            }
            
            //拷贝资源文件, 根据需要添加修改list, 国内不要谷歌Line等
            XDGIOSCommonProcessor.CopyThirdResource(target, projPath, proj, parentPath, resourcePath,
                new[]{"LineSDKResource.bundle", "GoogleSignIn.bundle", "XDConfig.json", "XDConfig-cn.json","GoogleService-Info.plist"});  //XDConfig.json SDk内部固定值

            //配置第三方id到 Xcode plist
            if (File.Exists(jsonPath)){
                var json = File.ReadAllText(jsonPath);
                var md = JsonConvert.DeserializeObject<XDGConfigModel>(json);
                XDGIOSCommonProcessor.SetThirdLibraryId(path, md);
            } else{
                Debug.LogError("json配置文件不存在");
            }
        }
    }
}

#endif