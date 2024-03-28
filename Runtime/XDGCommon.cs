using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using TapTap.Common;

[assembly: Preserve]
[assembly: AlwaysLinkAssembly]
namespace XD.SDK.Common{
    public class XDGCommon
    {
        public static IXDGCommon platformWrapper;

        static XDGCommon() 
        {
            var interfaceType = typeof(IXDGCommon);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz)&& clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDGCommon;
            }
            else 
            {
                TapLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }
        
        public static string UserId {
            get
            {
                return platformWrapper?.UserId;
            }
            set
            {
                if (platformWrapper != null)
                    platformWrapper.UserId = value;
            }
        }
        
        public static void InitSDK(Action<bool, string> callback)
        {
            platformWrapper.InitSDK(callback);
        }

        public static void IsInitialized(Action<bool> callback){
            platformWrapper.IsInitialized(callback);
        }

        public static void SetLanguage(LangType langType){
            platformWrapper.SetLanguage(langType);

            //设置 TapCommon
            TapLanguage tType = TapLanguage.AUTO;
            if (langType == LangType.ZH_CN) {
                tType = TapLanguage.ZH_HANS;
            } else if (langType == LangType.ZH_TW) {
                tType = TapLanguage.ZH_HANT;
            } else if (langType == LangType.JP) {
                tType = TapLanguage.JA;
            } else if (langType == LangType.KR) {
                tType = TapLanguage.KO;
            } else if (langType == LangType.EN) {
                tType = TapLanguage.EN;
            } else if (langType == LangType.TH) {
                tType = TapLanguage.TH;
            } else if (langType == LangType.ID) {
                tType = TapLanguage.ID;
            }

            TapCommon.SetLanguage(tType);
        }

        public static void Share(ShareFlavors shareFlavors, string uri, XDGShareCallback callback){
            platformWrapper.Share(shareFlavors, uri, callback);
        }

        public static void Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback){
            platformWrapper.Share(shareFlavors, uri, message, callback);
        }

        public static void Report(string serverId, string roleId, string roleName){
            platformWrapper.Report(serverId, roleId, roleName);
        }

        public static void TrackRole(string serverId, string roleId, string roleName, int level){
            platformWrapper.TrackRole(serverId, roleId, roleName, level);
        }

        public static void TrackUser(string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = UserId;
            }
            platformWrapper.TrackUser(userId);
        }

        public static void TrackEvent(string eventName, Dictionary<string,object> properties = null){
            platformWrapper.TrackEvent(eventName, properties);
        }

        public static void EventCompletedTutorial(){
            platformWrapper.EventCompletedTutorial();
        }

        public static void EventCreateRole(){
            platformWrapper.EventCreateRole();
        }

        public static void GetVersionName(Action<string> callback){
            platformWrapper.GetVersionName(callback);
        }

        public static void TrackAchievement(){
            platformWrapper.TrackAchievement();
        }

        public static void SetCurrentUserPushServiceEnable(bool enable){
            platformWrapper.SetCurrentUserPushServiceEnable(enable);
        }

        public static void IsCurrentUserPushServiceEnable(Action<bool> callback){
            platformWrapper.IsCurrentUserPushServiceEnable(callback);
        }

        public static void StoreReview(){
            platformWrapper.StoreReview();
        }

        public static void ShowLoading(){
            platformWrapper.ShowLoading();
        }

        public static void HideLoading(){
            platformWrapper.HideLoading();
        }

        public static void GetRegionInfo(Action<XDGRegionInfoWrapper> callback){
            platformWrapper.GetRegionInfo(callback);
        }
        
        public static void DisableAgreementUI(){
            platformWrapper.DisableAgreementUI();
        }

        public static void ReplaceChannelAndVersion(string channel, string gameVersion){
            platformWrapper.ReplaceChannelAndVersion(channel, gameVersion);
        }
        
        public static void GetAgreementList(Action<List<XDGAgreement>> callback){
            platformWrapper.GetAgreementList(callback);
        }
        
        public static void ShowDetailAgreement(string agreementUrl){
            platformWrapper.ShowDetailAgreement(agreementUrl);
        }

        public static void SetExitHandler(Action onExitHandler)
        {
            platformWrapper.SetExitHandler(onExitHandler);
        }
    }
}