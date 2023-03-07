using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using XD.SDK.Common.Internal;

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
            Debug.LogFormat($"[XD] IsInitialized! platformWrapper Type FullName: {platformWrapper.GetType().FullName}");
            platformWrapper.IsInitialized(callback);
        }

        public static void SetLanguage(LangType langType){
            platformWrapper.SetLanguage(langType);
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

        public static void TrackEvent(string eventName){
            platformWrapper.TrackEvent(eventName);
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