#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using XD.SDK.ADSubPackage.Internal;
using XD.SDK.Common;

namespace XD.SDK.ADSubPackage
{
    public class XDADsCore : IXDADs
    {
        public static readonly string AD_SUB_PKG_SERVICE_NAME = "com.xd.ads.subpkg.bridge.XDADsBridgeService";
        public static readonly string AD_SUB_PKG_SERVICE_IMPL = "com.xd.ads.subpkg.bridge.XDADsBridgeServiceImpl";
        
        private static readonly string ADS_MODULE_UNITY_BRIDGE_NAME = "XDADsBridgeService";
        private static volatile XDADsCore _instance;
        private static readonly object _Locker = new object();

        private XDADsCore()
        {
            EngineBridge.GetInstance()
                .Register(AD_SUB_PKG_SERVICE_NAME, AD_SUB_PKG_SERVICE_IMPL);
        }

        public static XDADsCore GetInstance()
        {
            lock (_Locker)
            {
                if (_instance == null)
                {
                    _instance = new XDADsCore();
                }
            }

            return _instance;
        }

        public void LogPredefinedAction(XDADsActionType actionType, string paramsJsonStr)
        {
            var dic = new Dictionary<string, object>
            {
                {"actionType", (int)actionType},
                {"paramsJsonStr", paramsJsonStr}
            };
            var command = new Command.Builder()
                .Service(ADS_MODULE_UNITY_BRIDGE_NAME)
                .Method("logPredefinedAction")
                .Args(dic)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command);
        }

        public void LogCustomAction(string eventName, string paramsJsonStr)
        {
            var dic = new Dictionary<string, object>
            {
                {"actionType", eventName},
                {"paramsJsonStr", paramsJsonStr}
            };
            var command = new Command.Builder()
                .Service(ADS_MODULE_UNITY_BRIDGE_NAME)
                .Method("logCustomAction")
                .Args(dic)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command);
        }

        public void SetDebuggable(bool debuggable)
        {
            var command = new Command.Builder()
                .Service(ADS_MODULE_UNITY_BRIDGE_NAME)
                .Method("setDebuggable")
                .Args("debuggable", debuggable)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command);
        }

        [Obsolete("OnPause() function is obsolete, event will be automatically tracked by third-party SDK")]
        public void OnPause()
        {
            
        }

        [Obsolete("OnResume() function is obsolete, event will be automatically tracked by third-party SDK")]
        public void OnResume()
        {
        
        }

        public void GetADsSDKVersion(Action<string> callback)
        {
            var command = new Command.Builder()
                .Service(ADS_MODULE_UNITY_BRIDGE_NAME)
                .Method("getADsSDKVersion")
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                if (!CheckResultSuccess(result))
                {
                    callback("");
                    return;
                }
                callback(result.content);
            });
        }

        public void GetChannelName(Action<string> callback)
        {
            var command = new Command.Builder()
                .Service(ADS_MODULE_UNITY_BRIDGE_NAME)
                .Method("getChannelName")
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                if (!CheckResultSuccess(result))
                {
                    callback("");
                    return;
                }
                callback(result.content);
            });
        }
        
        private static bool CheckResultSuccess(Result result){
            return result.code == Result.RESULT_SUCCESS && !string.IsNullOrEmpty(result.content);
        }
    }
}
#endif
