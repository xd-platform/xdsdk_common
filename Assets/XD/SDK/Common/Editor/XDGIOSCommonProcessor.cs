#if UNITY_EDITOR && UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using XD.SDK.Common.Editor;

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
            var resourcePath = Path.Combine(path, "SDKCommonResource");
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            if (!File.Exists(resourcePath)){
                Directory.CreateDirectory(resourcePath);
            }

            //拷贝资源文件, 根据需要修改
            CopyResource(target, projPath, proj, parentFolder,
                "com.xd.sdk.common", "Common", resourcePath,
                new[]{"XDResources.bundle"});

            //修改plist
            SetPlist(path);

            //插入代码片段
            SetScriptClass(path);
        }
    }

    private static void CopyResource(string target, string projPath, PBXProject proj, string parentFolder,
        string npmModuleName, string localModuleName, string xcodeResourceFolder, string[] bundleNames){
        //拷贝文件夹里的资源
        var tdsResourcePath = XDGFileHelper.FilterFile(parentFolder + "/Library/PackageCache/", $"{npmModuleName}@");
        if (string.IsNullOrEmpty(tdsResourcePath)){ //优先使用npm的，否则用本地的
            tdsResourcePath = parentFolder + "/Assets/XD/SDK/" + localModuleName;
        }

        tdsResourcePath = tdsResourcePath + "/Plugins/iOS/Resource";

        Debug.Log("资源路径" + tdsResourcePath);
        if (!Directory.Exists(tdsResourcePath) || tdsResourcePath == ""){
            Debug.LogError("需要拷贝的资源路径不存在");
            return;
        }

        XDGFileHelper.CopyAndReplaceDirectory(tdsResourcePath, xcodeResourceFolder);
        foreach (var name in bundleNames){
            proj.AddFileToBuild(target,
                proj.AddFile(Path.Combine(xcodeResourceFolder, name), Path.Combine(xcodeResourceFolder, name),
                    PBXSourceTree.Source));
        }

        File.WriteAllText(projPath, proj.WriteToString()); //保存
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

    public static void SetThirdLibraryId(string pathToBuildProject, XDGConfigModel configModel){
        if (configModel == null){
            Debug.LogError("配置文件Model是空");
            return;
        }

        var _plistPath = pathToBuildProject + "/Info.plist"; //Xcode工程的plist
        var _plist = new PlistDocument();
        _plist.ReadFromString(File.ReadAllText(_plistPath));
        var _rootDic = _plist.root;

        var facebookId = configModel.facebook.app_id;
        var facebookToken = configModel.facebook.client_token;
        var taptapId = configModel.tapsdk.client_id;
        var googleId = configModel.google.REVERSED_CLIENT_ID;
        var twitterId = configModel.twitter.consumer_key;
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
        if (taptapId != null){
            dict2.SetString("CFBundleURLName", "TapTap");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString($"tt{taptapId}");
        }

        if (googleId != null){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Google");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString(googleId);
        }

        if (facebookId != null){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Facebook");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString("fb" + facebookId);
        }

        if (bundleId != null){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Line");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString("line3rdp." + bundleId);
        }

        if (twitterId != null){
            dict2 = array.AddDict();
            dict2.SetString("CFBundleURLName", "Twitter");
            PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2 = dict2.CreateArray("CFBundleURLSchemes");
            array2.AddString("tdsg.twitter." + twitterId);
        }

        File.WriteAllText(_plistPath, _plist.WriteToString());
    }

    public static void CopyThirdResource(string target, string projPath, PBXProject proj, string parentFolder,
        string xcodeResourceFolder, string[] bundleNames){
        var tdsResourcePath = parentFolder + "/Assets/Plugins/iOS/Resource";

        if (!Directory.Exists(tdsResourcePath)){
            Debug.LogError("拷贝的资源路径不存在");
            return;
        }

        XDGFileHelper.CopyAndReplaceDirectory(tdsResourcePath, xcodeResourceFolder);
        foreach (var name in bundleNames){
            proj.AddFileToBuild(target,
                proj.AddFile(Path.Combine(xcodeResourceFolder, name), Path.Combine(xcodeResourceFolder, name),
                    PBXSourceTree.Source));
        }

        File.WriteAllText(projPath, proj.WriteToString()); //保存
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