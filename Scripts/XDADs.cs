#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System;

namespace XD.SDK.ADSubPackage
{
    public class XDADs
    {
        /**
        * 预定义事件
        * 
        * @param actionType    内置预定义事件类型
        * @param paramsJsonStr JSON 格式附加参数字符串
        */
        public static void LogPredefinedAction(XDADsActionType actionType, string paramsJsonStr)
        {
            XDADsCore.GetInstance().LogPredefinedAction(actionType, paramsJsonStr);
        }

        /**
        * 自定义事件
        *
        * @param eventName     事件名称
        * @param paramsJsonStr JSON 格式附加参数字符串
        */
        public static void LogCustomAction(string eventName, string paramsJsonStr)
        {
            XDADsCore.GetInstance().LogCustomAction(eventName, paramsJsonStr);
        }

        /**
        * 设置开启日志 默认 false
        *
        * @param debuggable true: 开启日志；false 关闭日志
        */
        public static void SetDebuggable(bool debuggable)
        {
            XDADsCore.GetInstance().SetDebuggable(debuggable);
        }

        /**
        * 暂停，生命周期函数，方便统计事件的准确性
        */
        [Obsolete("OnPause() function is obsolete, event will be automatically tracked by third-party SDK")]
        public static void OnPause()
        {
            XDADsCore.GetInstance().OnPause();
        }

        /**
        * 恢复，生命周期函数，方便统计事件的准确性
        */
        [Obsolete("OnResume() function is obsolete, event will be automatically tracked by third-party SDK")]
        public static void OnResume()
        {
            XDADsCore.GetInstance().OnResume();
        }

        /**
        * 获取 XDSDK 广告功能 SDK 版本
        */
        public static void GetADsSDKVersion(Action<string> callback)
        {
            XDADsCore.GetInstance().GetADsSDKVersion(callback);
        }

        /**
         * 获取渠道名称
         */
        public static void GetChannelName(Action<string> callback)
        {
            XDADsCore.GetInstance().GetChannelName(callback);
        }
    }
}
#endif