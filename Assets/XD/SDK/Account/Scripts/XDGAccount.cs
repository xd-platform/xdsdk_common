using System;
using System.Collections.Generic;
using XD.SDK.Common;

namespace XD.SDK.Account{
    public class XDGAccount{
        public static void Login(List<LoginType> loginTypes, Action<XDGUser> callback, Action<XDGError> errorCallback){
            XDGAccountImpl.GetInstance().Login(loginTypes, (u) => {
                callback(u);
                EventManager.LoginSuccessEvent();
            }, (e) => {
                errorCallback(e);
                EventManager.LoginFailEvent(e.error_msg);
            });
        }

        public static void LoginByType(LoginType loginType, Action<XDGUser> callback, Action<XDGError> errorCallback){
            XDGAccountImpl.GetInstance().LoginByType(loginType, (u) => {
                callback(u);
                EventManager.LoginSuccessEvent();
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
    }
}