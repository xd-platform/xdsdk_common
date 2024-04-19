#if UNITY_EDITOR && UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using LC.Newtonsoft.Json;
using System.Linq;

namespace XD.SDK.Common.Editor{
    public static class XDGIOSCommonProcessor
    {
        private static string rootPath;
        [PostProcessBuild(199)]
        public static void OnPostprocessBuild(BuildTarget BuildTarget, string path)
        {
            if (BuildTarget != BuildTarget.iOS) return;
            rootPath = path;
            Debug.LogFormat($"[XDSDK::Common] iOS rootPath: {rootPath}");
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
            proj.AddFrameworkToProject(unityFrameworkTarget, "AuthenticationServices.framework", true);

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
                "com.xd.sdk.oversea", "Oversea", resourcePath);
            
            CopyResource(target, projPath, proj, parentFolder,
                "com.xd.sdk.mainland", "Mainland", resourcePath);

            //拷贝外面配置的文件夹
            CopyThirdResource(target, projPath, proj, parentFolder, resourcePath);
            
            //拷贝 XDConfig.json
            var jsonPath = Editor.XDGCommonEditorUtils.GetXDConfigPath("XDConfig");
            if (!File.Exists(jsonPath)){
                Debug.LogError("XDConfig.json 配置文件不存在，这个是必须的");
                return;
            }
            var json = File.ReadAllText(jsonPath);
            var md = JsonConvert.DeserializeObject<XDConfigModel>(json);
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

        private static void CopyResource(string mainTargetGuid, string projPath, PBXProject proj, string dataPath,
            string npmModuleName, string localModuleName, string xcodeResourceFolder){
            //拷贝文件夹里的资源
            var moduleIOSResourcesFolder = FilterFile(dataPath + "/Library/PackageCache/", $"{npmModuleName}@");
            // 再检查 embed upm
            if (string.IsNullOrEmpty(moduleIOSResourcesFolder)){ //优先使用npm的，否则用本地的
                moduleIOSResourcesFolder = FilterFile(dataPath + "/Packages/", $"{npmModuleName}");
                if (string.IsNullOrEmpty(moduleIOSResourcesFolder)){ //优先使用npm的，否则用本地的{
                    moduleIOSResourcesFolder = dataPath + "/Assets/XDSDK/Mobile/" + localModuleName;
                }
            }
            moduleIOSResourcesFolder = moduleIOSResourcesFolder + "/Plugins/iOS/Resource";

            if (Directory.Exists(moduleIOSResourcesFolder) && moduleIOSResourcesFolder != ""){
                Debug.Log("拷贝资源路径: " + moduleIOSResourcesFolder);
                CopyAndReplaceDirectory(moduleIOSResourcesFolder, xcodeResourceFolder, mainTargetGuid, proj);
                File.WriteAllText(projPath, proj.WriteToString()); //保存  
            }
        }

        public static void AddXcodeConfig(string target, PBXProject proj, string xcodeFilePath){
            if (!xcodeFilePath.EndsWith("/Info.plist"))
            {
                var fromPath = rootPath;
                if (!fromPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    fromPath += Path.DirectorySeparatorChar.ToString();
                var relativePath = MakeRelativePath(fromPath, xcodeFilePath);
                Debug.LogFormat($"[XDSDK::Common] AddXcodeConfig fromPath: {rootPath + Path.DirectorySeparatorChar.ToString()}, toPath: {xcodeFilePath}, result: {relativePath}");
                
                var fileGuid = proj.AddFile(relativePath, relativePath);
                proj.AddFileToBuild(target, fileGuid);
            }
        }

        private static void CopyAndReplaceDirectory(string srcPath, string dstPath, string mainTargetGuid, PBXProject proj){
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
                if (filePath.EndsWith(".plist") || filePath.EndsWith(".json") || filePath.EndsWith("InfoPlist.strings")){
                    AddXcodeConfig(mainTargetGuid, proj, filePath);
                }
            }

            foreach (var dir in Directory.GetDirectories(srcPath)){
                var name = Path.GetFileName(dir);
                var dirPath = Path.Combine(dstPath, name);
#if !UNITY_2019_3_OR_NEWER
                if (!name.EndsWith(".framework") && !name.EndsWith(".xcframework")){
                    CopyAndReplaceDirectory(dir, dirPath, mainTargetGuid, proj);
                }
#else
                if (!name.EndsWith(".bundle") && !name.EndsWith(".framework") && !name.EndsWith(".xcframework")){
                    CopyAndReplaceDirectory(dir, dirPath, mainTargetGuid, proj);
                }
#endif

                if (dirPath.EndsWith(".bundle"))
                {
#if !UNITY_2019_3_OR_NEWER
                    AddXcodeConfig(mainTargetGuid, proj, dirPath);
                    return;
#endif
                    List<string> deletingFolders = new List<string>();
                    DirectoryInfo oldBundleDi = null;
                    string[] directories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
                    foreach (string directory in directories)
                    {
                        oldBundleDi = new DirectoryInfo(directory);
                        if (oldBundleDi.Name == name)
                        {
                            deletingFolders.Add(oldBundleDi.FullName);
                            break;
                        }
                    }
                    foreach (var deletingFolder in deletingFolders)
                    {
                        var relativePath = MakeRelativePath(rootPath + Path.DirectorySeparatorChar.ToString(),
                            deletingFolder);
                        
                        var unityIPhoneTargetGuid = proj.GetUnityMainTargetGuid();
                        var resourceTarget = proj.GetResourcesBuildPhaseByTarget(unityIPhoneTargetGuid);
                        
                        var fileGUID = proj.AddFile(deletingFolder, relativePath);
                        proj.AddFileToBuildSection(unityIPhoneTargetGuid, resourceTarget, fileGUID);
                        var unityFrameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();
                        proj.RemoveFileFromBuild(unityFrameworkTargetGuid, fileGUID);
                    }
                    
                }
            }
        }
        
        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private static string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath))   throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private static void SetPlist(string pathToBuildProject){
            var _plistPath = pathToBuildProject + "/Info.plist"; //Xcode工程的plist
            var _plist = new PlistDocument();
            _plist.ReadFromString(File.ReadAllText(_plistPath));
            var _rootDic = _plist.root;

            //添加 scheme
            var items = new List<string>(){
                "sinaweibo",
                "weibosdk",
                "weibosdk2.5",
                "weibosdk3.3",
                "mqqopensdkapiV2",
                "mqq",
                "mqqapi",
                "tim",
                "mqqopensdknopasteboard",
                "weixin",
                "weixinULAPI",
                "weixinURLParamsAPI",
                "tapsdk",
                "tapiosdk",

                "fbapi",
                "fb-messenger-share-api",
                
                "lineauth2",

                "twitterauth",

                "xhsdiscover",

                "douyinopensdk",
                "douyinliteopensdk",
                "douyinsharesdk",

                "snssdk1128",
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

        private static void SetThirdLibraryId(string pathToBuildProject, XDConfigModel configModel){
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
            var googleId = (configModel.google != null ? configModel.google.CLIENT_ID : null); //注意要反转一下！
            var twitterId = (configModel.twitter != null ? configModel.twitter.consumer_key : null);
            var bundleId = configModel.bundle_id;
            var qqId = (configModel.qq != null ? configModel.qq.app_id : null);
            var wechatId = (configModel.wechat != null ? configModel.wechat.app_id : null);
            var weiboId = (configModel.weibo != null ? configModel.weibo.app_id : null);

            string xhsId = configModel.xhs != null ? configModel.xhs.app_id_ios : null;
            string douyinId = configModel.douyin != null ? configModel.douyin.app_id : null;


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
                var reverseGoogleId = reverseStr(googleId);
                
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "Google");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString(reverseGoogleId);
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

             if (!string.IsNullOrEmpty(qqId)){
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "qq");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString("tencent" + qqId);
            }

            if (!string.IsNullOrEmpty(wechatId)){
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "wechat");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString(wechatId);
            }

            if (!string.IsNullOrEmpty(weiboId)){
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "weibo");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString("wb" + weiboId);
            }

            if (!string.IsNullOrEmpty(xhsId)) {
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "xhs");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString(xhsId);
            }

            if (!string.IsNullOrEmpty(douyinId)) {
                dict2 = array.AddDict();
                dict2.SetString("CFBundleURLName", "douyin");
                PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2 = dict2.CreateArray("CFBundleURLSchemes");
                array2.AddString(douyinId);
            }

            File.WriteAllText(_plistPath, _plist.WriteToString());
        }

        private static string reverseStr(string str){ //用.分割然后反转,谷歌配置用
            if (string.IsNullOrEmpty(str)){
                return "";
            }
            var list = str.Split('.').Reverse();
            return string.Join(".", list);
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

            UnityAppController.WriteBelow(@"applicationDidFinishLaunching",
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

    }
}
#endif