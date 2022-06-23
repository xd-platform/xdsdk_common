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

public static class XDGIOSCommonProcessor{
    [PostProcessBuild(199)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string path){
        if (BuildTarget == BuildTarget.iOS){
            var projPath = PBXProject.GetPBXProjectPath(path);
            var proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            // 2019.3以上有多个target
#if UNITY_2019_3_OR_NEWER
            string unityFrameworkTarget = proj.TargetGuidByName("UnityFramework");
            string target = proj.GetUnityMainTargetGuid();
#else
                string unityFrameworkTarget = proj.TargetGuidByName("Unity-iPhone");
                string target = proj.TargetGuidByName("Unity-iPhone");
#endif

            //添加基本配置
            proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
            proj.AddBuildProperty(unityFrameworkTarget, "OTHER_LDFLAGS", "-ObjC ");
            proj.AddFrameworkToProject(unityFrameworkTarget, "Accelerate.framework", true);

            //创建文件夹
            var resourcePath = Path.Combine(path, "SDKResource");
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            if (!File.Exists(resourcePath)){
                Directory.CreateDirectory(resourcePath);
            }

            //拷贝SDK内部资源文件 common 
            CopyResource(target, projPath, proj, parentFolder,
                "com.xd.sdk.common", "Common", resourcePath);
            
            //拷贝SDK内部资源文件 third
            CopyResource(target, projPath, proj, parentFolder,
                "com.xd.sdk.thirdoversea", "ThirdOversea", resourcePath);

            //拷贝外面配置的文件夹
            CopyThirdResource(target, projPath, proj, parentFolder, resourcePath);
            
            //拷贝 XDConfig.json
            var jsonPath = parentFolder + "/Assets/Plugins/XDConfig.json";
            if (!File.Exists(jsonPath)){
                Debug.LogError("XDConfig.json 配置文件不存在，这个是必须的");
                return;
            }
            var json = File.ReadAllText(jsonPath);
            var md = JsonConvert.DeserializeObject<XDGConfigModel>(json);
            if (md == null){
                Debug.LogError("json 配置文件解析失败: " + jsonPath);
                return;
            }
            var filePath = Path.Combine(resourcePath, "XDConfig.json");
            File.Copy(jsonPath, filePath);
            AddXcodeConfig(target, proj, filePath); //先拷贝，后配置
            SetThirdLibraryId(path, md);
            File.WriteAllText(projPath, proj.WriteToString()); //保存

            //修改plist
            SetPlist(path);

            //插入代码片段
            SetScriptClass(path);
        }
    }

    private static string FilterFile(string srcPath, string filterName){
        if (!Directory.Exists(srcPath)){
            return null;
        }

        foreach (var dir in Directory.GetDirectories(srcPath)){
            string fileName = Path.GetFileName(dir);
            if (fileName.StartsWith(filterName)){
                Debug.Log("筛选到指定文件夹:" + Path.Combine(srcPath, Path.GetFileName(dir)));
                return Path.Combine(srcPath, Path.GetFileName(dir));
            }
        }

        return null;
    }

    private static void CopyResource(string target, string projPath, PBXProject proj, string parentFolder,
        string npmModuleName, string localModuleName, string xcodeResourceFolder){
        //拷贝文件夹里的资源
        var tdsResourcePath = FilterFile(parentFolder + "/Library/PackageCache/", $"{npmModuleName}@");
        if (string.IsNullOrEmpty(tdsResourcePath)){ //优先使用npm的，否则用本地的
            tdsResourcePath = parentFolder + "/Assets/XD/SDK/" + localModuleName;
        }
        tdsResourcePath = tdsResourcePath + "/Plugins/iOS/Resource";

        if (Directory.Exists(tdsResourcePath) && tdsResourcePath != ""){
            Debug.Log("拷贝资源路径: " + tdsResourcePath);
            CopyAndReplaceDirectory(tdsResourcePath, xcodeResourceFolder, target, proj);
            File.WriteAllText(projPath, proj.WriteToString()); //保存  
        }
    }

    public static void AddXcodeConfig(string target, PBXProject proj, string xcodeFilePath){
        if (!xcodeFilePath.EndsWith("/Info.plist")){ //有些bundle里有这个，和项目的冲突
            proj.AddFileToBuild(target, proj.AddFile(xcodeFilePath, xcodeFilePath, PBXSourceTree.Source));
        }
    }

    private static void CopyAndReplaceDirectory(string srcPath, string dstPath, string target, PBXProject proj){
        if (!File.Exists(dstPath)){
            Directory.CreateDirectory(dstPath);
        }

        //.framework文件 和 meta文件不拷贝
        foreach (var file in Directory.GetFiles(srcPath)){
            var name = Path.GetFileName(file);
            var filePath = Path.Combine(dstPath, name);

            if (!name.EndsWith(".meta") && !File.Exists(filePath)){
                File.Copy(file, filePath);
            }

            //添加到 xcode 配置
            if (filePath.EndsWith(".bundle") || filePath.EndsWith(".plist") || filePath.EndsWith(".json")){
                AddXcodeConfig(target, proj, filePath);
            }
        }

        foreach (var dir in Directory.GetDirectories(srcPath)){
            var name = Path.GetFileName(dir);
            var dirPath = Path.Combine(dstPath, name);
            if (!name.EndsWith(".framework") && !name.EndsWith(".xcframework")){
                CopyAndReplaceDirectory(dir, dirPath, target, proj);
            }

            if (dirPath.EndsWith(".bundle")){
                AddXcodeConfig(target, proj, dirPath);
            }
        }
    }


    private static void SetPlist(string pathToBuildProject){
        var _plistPath = pathToBuildProject + "/Info.plist"; //Xcode工程的plist
        var _plist = new PlistDocument();
        _plist.ReadFromString(File.ReadAllText(_plistPath));
        var _rootDic = _plist.root;

        //添加 scheme
        var items = new List<string>(){
            "tapsdk",
            "tapiosdk",
            "fbapi",
            "fbapi20130214",
            "fbapi20130410",
            "fbapi20130702",
            "fbapi20131010",
            "fbapi20131219",
            "fbapi20140410",
            "fbapi20140116",
            "fbapi20150313",
            "fbapi20150629",
            "fbapi20160328",
            "fb-messenger-share-api",
            "fbauth2",
            "fbauth",
            "fbshareextension",
            "lineauth2"
        };

        //添加Scheme，用添加，不要覆盖替换！
        PlistElementArray _list = null;
        foreach (var item in _rootDic.values){
            if (item.Key.Equals("LSApplicationQueriesSchemes")){
                _list = (PlistElementArray) item.Value;
                break;
            }
        }

        if (_list == null){
            _list = _rootDic.CreateArray("LSApplicationQueriesSchemes");
        }

        for (int i = 0; i < items.Count; i++){
            _list.AddString(items[i]);
        }

        File.WriteAllText(_plistPath, _plist.WriteToString());
    }

    private static void SetThirdLibraryId(string pathToBuildProject, XDGConfigModel configModel){
        if (configModel == null){
            Debug.LogError("打包失败  ----  XDConfig 配置文件Model是空");
            return;
        }

        var _plistPath = pathToBuildProject + "/Info.plist"; //Xcode工程的plist
        var _plist = new PlistDocument();
        _plist.ReadFromString(File.ReadAllText(_plistPath));
        var _rootDic = _plist.root;

        var facebookId = (configModel.facebook != null ? configModel.facebook.app_id : null);
        var facebookToken = (configModel.facebook != null ? configModel.facebook.client_token : null);
        var taptapId = (configModel.tapsdk != null ? configModel.tapsdk.client_id : null);
        var googleId = (configModel.google != null ? configModel.google.REVERSED_CLIENT_ID : null);
        var twitterId = (configModel.twitter != null ? configModel.twitter.consumer_key : null);
        var bundleId = configModel.bundle_id;

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

        //添加FacebookAppID 和 FacebookClientToken
        if (facebookId != null && facebookToken != null){
            dict.SetString("FacebookAppID", facebookId);
            dict.SetString("FacebookClientToken", facebookToken);
        }

        PlistElementDict dict2 = array.AddDict();
        if (!string.IsNullOrEmpty(taptapId)){
            dict2.SetString("CFBundleURLName", "Tap");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString($"tt{taptapId}");
        }

        if (!string.IsNullOrEmpty(googleId)){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Google");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString(googleId);
        }

        if (!string.IsNullOrEmpty(facebookId)){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Facebook");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString("fb" + facebookId);
        }

        if (!string.IsNullOrEmpty(bundleId)){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Line");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString("line3rdp." + bundleId);
        }

        if (!string.IsNullOrEmpty(twitterId)){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Twitter");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString("tdsg.twitter." + twitterId);
        }

        File.WriteAllText(_plistPath, _plist.WriteToString());
    }

    private static void CopyThirdResource(string target, string projPath, PBXProject proj, string parentFolder,
        string xcodeResourceFolder){
        var tdsResourcePath = parentFolder + "/Assets/Plugins/iOS";

        //拷贝文件夹
        if (Directory.Exists(tdsResourcePath)){
            Debug.Log("拷贝资源路径: " + tdsResourcePath);
            CopyAndReplaceDirectory(tdsResourcePath, xcodeResourceFolder, target, proj);
            File.WriteAllText(projPath, proj.WriteToString()); //保存  
        }
    }

    private static void SetScriptClass(string pathToBuildProject){
        //读取Xcode中 UnityAppController.mm文件
        var unityAppControllerPath = pathToBuildProject + "/Classes/UnityAppController.mm";
        var UnityAppController = new XDGScriptHandlerProcessor(unityAppControllerPath);

        //在指定代码后面增加一行代码
        UnityAppController.WriteBelow(@"#include <assert.h>", @"#import <XDCommonSDK/XDCommonSDK.h>");
        UnityAppController.WriteBelow(@"#include <assert.h>", @"#import <XDCommonSDK/XDGSDKSettings.h>");

        UnityAppController.WriteBelow(@"[KeyboardDelegate Initialize];",
            @"[XDGSDK application:application didFinishLaunchingWithOptions:launchOptions];");
        UnityAppController.WriteBelow(@"[KeyboardDelegate Initialize];",
            @"[XDGSDKSettings setDebugMode:YES];");
        UnityAppController.WriteBelow(@"AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);",
            @"[XDGSDK application:app openURL:url options:options];");

        if (CheckoutUniversalLinkHolder(unityAppControllerPath, @"NSURL* url = userActivity.webpageURL;")){
            UnityAppController.WriteBelow(@"NSURL* url = userActivity.webpageURL;",
                @"[XDGSDK application:application continueUserActivity:userActivity restorationHandler:restorationHandler];");
        } else{
            UnityAppController.WriteBelow(@"- (void)preStartUnity               {}",
                @"-(BOOL) application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray<id<UIUserActivityRestoring>> * _Nullable))restorationHandler{[XDGSDK application:application continueUserActivity:userActivity restorationHandler:restorationHandler];return YES;}");
        }
    }

    private static bool CheckoutUniversalLinkHolder(string filePath, string below){
        StreamReader streamReader = new StreamReader(filePath);
        string all = streamReader.ReadToEnd();
        streamReader.Close();
        int beginIndex = all.IndexOf(below, StringComparison.Ordinal);
        return beginIndex != -1;
    }

    private static string GetValueFromPlist(string infoPlistPath, string key){
        if (infoPlistPath == null){
            return null;
        }

        var dic = (Dictionary<string, object>) Plist.readPlist(infoPlistPath);
        foreach (var item in dic){
            if (item.Key.Equals(key)){
                return (string) item.Value;
            }
        }

        return null;
    }
}

#endif