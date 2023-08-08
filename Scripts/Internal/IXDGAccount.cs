using System;
using System.Collections.Generic;
using XD.SDK.Common;
using XD.SDK.Common.Internal;
using LoginType = XD.SDK.Account.LoginType;

namespace XD.SDK.Account.Internal{
    public interface IXDGAccount
    {
        void Login(List<LoginType> loginTypes, Action<XDGUser> callback,
            Action<XDGError> errorCallback);

        void LoginByType(LoginType loginType, Action<XDGUser> callback,
            Action<XDGError> errorCallback);

        /// <summary>
        /// 发行在主机(Steam/PS/Nintendo)上的游戏的自动登录接口
        /// </summary>
        /// <param name="successCallback">该主机的账号注册过心动账号，那么会返回登录成功（第二次登录的时候，如果主机账号没有发生改变，会直接返回登录成功，略过网络请求）</param>
        /// <param name="failCallback">该主机的账号未注册过心动账号，那么会返回失败。这个时候需要调用LoginByType登录</param>
        /// <param name="errorCallback">网络请求错误或者非主机平台的游戏调用，以及其他错误</param>
        void LoginByConsole(Action<XDGUser> successCallback, Action failCallback,
            Action<XDGError> errorCallback);


        void Logout();

        void AddUserStatusChangeCallback(Action<XDGUserStatusCodeType, string> callback);

        void GetUser(Action<XDGUser> callback, Action<XDGError> errorCallback);

        void OpenUserCenter();

        void OpenUnregister();
        
        //641 FB token
        void IsTokenActiveWithType(LoginType loginType, Action<bool> callback);
        
        //除了 Default 和 Guest
        void BindByType(LoginType loginType, Action<bool, XDGError> callback);

    }
}