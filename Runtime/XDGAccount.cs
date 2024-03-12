using System;
using System.Collections.Generic;
using System.Linq;
using TapTap.Common;
using UnityEngine;
using UnityEngine.Scripting;
using XD.SDK.Account;
using XD.SDK.Common;
using LoginType = XD.SDK.Account.LoginType;

[assembly: Preserve]
[assembly: AlwaysLinkAssembly]
namespace XD.SDK.Account{
    public class XDGAccount
    {
        private static IXDGAccount platformWrapper;
        public static LoginType LoginType { get; set; }
        
        static XDGAccount() 
        {
            var interfaceType = typeof(IXDGAccount);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz)&& clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDGAccount;
            }
            else 
            {
                TapLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }
        
        public static void Login(List<LoginType> loginTypes, Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            platformWrapper.Login(loginTypes, callback, errorCallback);
        }

        public static void LoginByType(LoginType loginType, Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            LoginType = loginType;
            platformWrapper.LoginByType(loginType, callback, errorCallback);
        }

        public static void LoginByConsole(Action<XDGUser> successCallback, Action failCallback, Action<XDGError> errorCallback)
        {
            platformWrapper.LoginByConsole(successCallback, failCallback, errorCallback);
        }

        public static void Logout()
        {
            platformWrapper.Logout();
        }
        
        public static void AddUserStatusChangeCallback(Action<XDGUserStatusCodeType, string> callback){
            platformWrapper.AddUserStatusChangeCallback(callback);
        }
        
        public static void GetUser(Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            platformWrapper.GetUser(callback, errorCallback);
        }
        
        public static void OpenUserCenter(){
            platformWrapper.OpenUserCenter();
        }

        public static void OpenUnregister(){
            platformWrapper.OpenUnregister();
        }
        
        //641 FB token
        public static void IsTokenActiveWithType(LoginType loginType, Action<bool> callback){
            platformWrapper.IsTokenActiveWithType(loginType, callback);
        }
        
        //除了 Default 和 Guest
        public static void BindByType(LoginType loginType, Action<bool,XDGError> callback){
            platformWrapper.BindByType(loginType, callback);
        }

        public static string GetLoginTypeString(LoginType loginType)
        {
            switch (loginType)
            {
                case XD.SDK.Account.LoginType.TapTap:
                    return "TapTap";
                case XD.SDK.Account.LoginType.Google:
                    return "Google";
                case XD.SDK.Account.LoginType.Facebook:
                    return "Facebook";
                case XD.SDK.Account.LoginType.Apple:
                    return "Apple";
                case XD.SDK.Account.LoginType.LINE:
                    return "LINE";
                case XD.SDK.Account.LoginType.Twitter:
                    return "Twitter";
                // case XD.SDK.Account.LoginType.QQ:
                //     return "QQ";
                // case XD.SDK.Account.LoginType.Twitch:
                //     return "Twitch";
                case XD.SDK.Account.LoginType.Steam:
                    return "Steam";
                case XD.SDK.Account.LoginType.Guest:
                    return "Guest";
                case XD.SDK.Account.LoginType.Phone:
                    return "Phone";
                default:
                    return "Default";
            }
        }
    }
}