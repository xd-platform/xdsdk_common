using System;

namespace XD.SDK.ADSubPackage.Internal
{
    public interface IXDADs
    {
        void LogPredefinedAction(XDADsActionType actionType, string paramsJsonStr);
        void LogCustomAction(string eventName, string paramsJsonStr);
        void SetDebuggable(bool debuggable);
        void OnPause();
        void OnResume();
        void GetADsSDKVersion(Action<string> callback);
        void GetChannelName(Action<string> callback);
    }
}