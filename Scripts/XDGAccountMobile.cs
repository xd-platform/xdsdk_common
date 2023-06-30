#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using XD.SDK.Account.Internal;
using XD.SDK.Common;
using XD.SDK.Common.Internal;
using LoginType = XD.SDK.Account.LoginType;

namespace XD.SDK.Account{
    public class XDGAccountMobile : IXDGAccount
    {
        public static void Login(List<LoginType> loginTypes, Action<XDGUser> callback, Action<XDGError> errorCallback){
            XDGAccountMobileImpl.GetInstance().Login(loginTypes, (u) =>
            {
                XDGCommon.UserId = u.userId;
                callback?.Invoke(u);
                EventManager.LoginSuccessEvent();

                if (u.loginType.ToLower().Equals("facebook")){
                    XDGTokenManager.updateFacebookRefreshTime();
                }
            }, (e) => {
                errorCallback(e);
                EventManager.LoginFailEvent(e.error_msg);
            });
        }

        public static void LoginByType(LoginType loginType, Action<XDGUser> callback, Action<XDGError> errorCallback){
            XDGAccountMobileImpl.GetInstance().LoginByType(loginType, (u) => {
                XDGCommon.UserId = u.userId;
                callback?.Invoke(u);
                EventManager.LoginSuccessEvent();

                if (loginType == XD.SDK.Account.LoginType.Default){ //自动登录需要异步刷 Facebook token
                    XDGTokenManager.updateFacebookToken(u);
                } else if (loginType == XD.SDK.Account.LoginType.Facebook){
                    XDGTokenManager.updateFacebookRefreshTime();
                }

            }, (e) => {
                errorCallback(e);
                EventManager.LoginFailEvent(e.error_msg);
            });
        }

        public static void Logout()
        {
            XDGCommon.UserId = null;
            XDGAccountMobileImpl.GetInstance().Logout();
        }

        public static void AddUserStatusChangeCallback(Action<XDGUserStatusCodeType, string> callback){
            XDGAccountMobileImpl.GetInstance().AddUserStatusChangeCallback(callback);
        }
        
        public static void OpenUserCenter(){
            XDGAccountMobileImpl.GetInstance().OpenUserCenter();
        }

        public static void OpenUnregister(){
            XDGAccountMobileImpl.GetInstance().OpenUnregister();
        }
        
        //641 FB token
        public static void IsTokenActiveWithType(LoginType loginType, Action<bool> callback){
            XDGAccountMobileImpl.GetInstance().IsTokenActiveWithType(loginType, callback);
        }

        #region Interface
        void IXDGAccount.Login(List<LoginType> loginTypes, Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            Login(loginTypes, callback, (error)=> errorCallback(error as XDGError));
        }

        void IXDGAccount.LoginByType(LoginType loginType, Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            LoginByType(loginType, callback, (error)=> errorCallback(error as XDGError));
        }

        /// <summary>
        /// 发行在主机(Steam/PS/Nintendo)上的游戏的自动登录接口
        /// </summary>
        /// <param name="successCallback">该主机的账号注册过心动账号，那么会返回登录成功（第二次登录的时候，如果主机账号没有发生改变，会直接返回登录成功，略过网络请求）</param>
        /// <param name="failCallback">该主机的账号未注册过心动账号，那么会返回失败。这个时候需要调用LoginByType登录</param>
        /// <param name="errorCallback">网络请求错误或者非主机平台的游戏调用，以及其他错误</param>
        void IXDGAccount.LoginByConsole(Action<XDGUser> successCallback, Action failCallback, Action<XDGError> errorCallback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        void IXDGAccount.Logout()
        {
            Logout();
        }

        void IXDGAccount.AddUserStatusChangeCallback(Action<XDGUserStatusCodeType, string> callback)
        {
            AddUserStatusChangeCallback(callback);
        }

        public void GetUser(Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            XDGAccountMobileImpl.GetInstance().GetUser((u) =>
            {
                XDGCommon.UserId = u.userId;
                callback?.Invoke(u);
            }, (error)=> errorCallback(error as XDGError));
        }

        void IXDGAccount.OpenUserCenter()
        {
            OpenUserCenter();
        }

        void IXDGAccount.OpenUnregister()
        {
            OpenUnregister();
        }

        void IXDGAccount.IsTokenActiveWithType(LoginType loginType, Action<bool> callback)
        {
            IsTokenActiveWithType(loginType, callback);
        }

        //除了 Default 和 Guest
        public void BindByType(LoginType loginType, Action<bool, XDGError> callback)
        {
            XDGAccountMobileImpl.GetInstance().BindByType(loginType, (b, error)=> callback(b, error as XDGError));
        }
        #endregion
    }
}
#endif