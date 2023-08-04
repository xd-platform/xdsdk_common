#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace  XD.SDK.Common{
    public class XDGCommonMobile : IXDGCommon
    {
        public string UserId { get; set; }
        
        public void InitSDK(Action<bool, string> callback){
            XDGCommonMobileImpl.GetInstance().InitSDK(callback);
        }

        public void IsInitialized(Action<bool> callback){
            XDGCommonMobileImpl.GetInstance().IsInitialized(callback);
        }

        public void SetLanguage(LangType langType){
            XDGCommonMobileImpl.GetInstance().SetLanguage(langType);
        }

        public void Share(ShareFlavors shareFlavors, string uri, XDGShareCallback callback){
            XDGCommonMobileImpl.GetInstance().Share(shareFlavors, uri, callback);
        }

        public void Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback){
            XDGCommonMobileImpl.GetInstance().Share(shareFlavors, uri, message, callback);
        }

        public void Report(string serverId, string roleId, string roleName){
            XDGCommonMobileImpl.GetInstance().Report(serverId, roleId, roleName);
        }

        public void TrackRole(string serverId, string roleId, string roleName, int level){
            XDGCommonMobileImpl.GetInstance().TrackRole(serverId, roleId, roleName, level);
        }

        public void TrackUser(string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = UserId;
            }
            XDGCommonMobileImpl.GetInstance().TrackUser(userId);
        }

        public void TrackEvent(string eventName, Dictionary<string,object> properties = null){
            XDGCommonMobileImpl.GetInstance().TrackEvent(eventName, properties);
        }

        public void EventCompletedTutorial(){
            XDGCommonMobileImpl.GetInstance().EventCompletedTutorial();
        }

        public void EventCreateRole(){
            XDGCommonMobileImpl.GetInstance().EventCreateRole();
        }

        public void GetVersionName(Action<string> callback){
            XDGCommonMobileImpl.GetInstance().GetVersionName(callback);
        }

        public void TrackAchievement(){
            XDGCommonMobileImpl.GetInstance().TrackAchievement();
        }

        public void SetCurrentUserPushServiceEnable(bool enable){
            XDGCommonMobileImpl.GetInstance().SetCurrentUserPushServiceEnable(enable);
        }

        public void IsCurrentUserPushServiceEnable(Action<bool> callback){
            XDGCommonMobileImpl.GetInstance().IsCurrentUserPushServiceEnable(callback);
        }

        public void StoreReview(){
            XDGCommonMobileImpl.GetInstance().StoreReview();
        }

        public void ShowLoading(){
            XDGCommonMobileImpl.GetInstance().ShowLoading();
        }

        public void HideLoading(){
            XDGCommonMobileImpl.GetInstance().HideLoading();
        }

        public void GetRegionInfo(Action<XDGRegionInfoWrapper> callback){
            XDGCommonMobileImpl.GetInstance().GetRegionInfo(callback);
        }
        
        public void DisableAgreementUI(){
            XDGCommonMobileImpl.GetInstance().disableAgreementUI();
        }

        public void ReplaceChannelAndVersion(string channel, string gameVersion){
            XDGCommonMobileImpl.GetInstance().ReplaceChannelAndVersion(channel, gameVersion);
        }
        
        public void GetAgreementList(Action<List<XDGAgreement>> callback){
            XDGCommonMobileImpl.GetInstance().GetAgreementList(callback);
        }
        
        public void ShowDetailAgreement(string agreementUrl){
            XDGCommonMobileImpl.GetInstance().ShowDetailAgreement(agreementUrl);
        }

        public void SetExitHandler(Action onExitHandler)
        {
            XDGCommonMobileImpl.GetInstance().SetExitHandler(onExitHandler);
        }
    }
}
#endif