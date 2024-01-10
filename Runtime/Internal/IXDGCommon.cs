using System;
using System.Collections.Generic;

namespace XD.SDK.Common
{
    public interface IXDGCommon
    {
        string UserId { get; set; }

        void InitSDK(Action<bool, string> callback);

        void IsInitialized(Action<bool> callback);
        
        void SetLanguage(LangType langType);

        void Share(ShareFlavors shareFlavors, string uri, XDGShareCallback callback);

        void Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback);

        void Report(string serverId, string roleId, string roleName);

        void TrackRole(string serverId, string roleId, string roleName, int level);

        void TrackUser(string userId = null);

        void TrackEvent(string eventName, Dictionary<string,object> properties = null);

        void EventCompletedTutorial();
        
        void EventCreateRole();

        void GetVersionName(Action<string> callback);

        void TrackAchievement();

        void SetCurrentUserPushServiceEnable(bool enable);

        void IsCurrentUserPushServiceEnable(Action<bool> callback);

        void StoreReview();

        void ShowLoading();

        void HideLoading();

        void GetRegionInfo(Action<XDGRegionInfoWrapper> callback);

        void DisableAgreementUI();

        void ReplaceChannelAndVersion(string channel, string gameVersion);

        void GetAgreementList(Action<List<XDGAgreement>> callback);

        void ShowDetailAgreement(string agreementUrl);

        void SetExitHandler(Action onExitHandler);
    }
}