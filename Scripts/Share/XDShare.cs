using System;
using System.Collections.Generic;
using System.Linq;
using TapTap.Common;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: Preserve]
[assembly: AlwaysLinkAssembly]
namespace XD.SDK.Share{
    public class XDShare{
        public static IXDShare platformWrapper;
        
        static XDShare() 
        {
            var interfaceType = typeof(IXDShare);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDShare;
            }
            else 
            {
                TapLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }
        
        public static void ShareText(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string text, IXDShareCallback callback){
            platformWrapper.ShareText(target, scene, text, callback);
        }

        public static void ShareImage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string imageUri, IXDShareCallback callback){
            platformWrapper.ShareImage(target, scene, imageUri, callback);

        }

        public static void ShareWebPage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, WebPageData data, IXDShareCallback callback){
            platformWrapper.ShareWebPage(target, scene, data, callback);
        }

        public static void IsTargetInstalled(ShareConstants.ShareTarget target, Action<bool> callback){
            platformWrapper.IsTargetInstalled(target, callback);
        }
    }
}