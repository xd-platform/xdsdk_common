using System;
using System.Collections.Generic;
using XD.SDK.Common;

namespace XD.SDK.Account{
    public class XDGAccount{
        public static void Login(List<LoginType> loginTypes, Action<XDGUser> callback, Action<XDGError> errorCallback){
            XDGAccountImpl.GetInstance().Login(loginTypes, (u) => {
                callback(u);
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
            XDGAccountImpl.GetInstance().LoginByType(loginType, (u) => {
                callback(u);
                EventManager.LoginSuccessEvent();

                if (loginType == LoginType.Default){ //自动登录需要异步刷 Facebook token
                    XDGTokenManager.updateFacebookToken(u);
                } else if (loginType == LoginType.Facebook){
                    XDGTokenManager.updateFacebookRefreshTime();
                }

            }, (e) => {
                errorCallback(e);
                EventManager.LoginFailEvent(e.error_msg);
            });
        }


        public static void Logout(){
            XDGAccountImpl.GetInstance().Logout();
        }

        public static void AddUserStatusChangeCallback(Action<XDGUserStatusCodeType, string> callback){
            XDGAccountImpl.GetInstance().AddUserStatusChangeCallback(callback);
        }

        public static void GetUser(Action<XDGUser> callback, Action<XDGError> errorCallback){
            XDGAccountImpl.GetInstance().GetUser(callback, errorCallback);
        }

        public static void OpenUserCenter(){
            XDGAccountImpl.GetInstance().OpenUserCenter();
        }

        public static void OpenUnregister(){
            XDGAccountImpl.GetInstance().OpenUnregister();
        }
        
        //641 FB token
        public static void IsTokenActiveWithType(LoginType loginType, Action<bool> callback){
            XDGAccountImpl.GetInstance().IsTokenActiveWithType(loginType, callback);
        }
        
        //除了 Default 和 Guest
        public static void BindByType(LoginType loginType, Action<bool,XDGError> callback){
            XDGAccountImpl.GetInstance().BindByType(loginType, callback);
        }

    }
}