#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using XD.SDK.Common;

namespace XD.SDK.Share
{
    public class XDShareMobile : IXDShare
    {
        public static void ShareText(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string text, IXDShareCallback callback){
            XDShareMobileImpl.GetInstance().ShareText(target, scene, text, callback);
        }

        public static void ShareImage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string imageUri, IXDShareCallback callback){
            XDShareMobileImpl.GetInstance().ShareImage(target, scene, imageUri, callback);
        }

        public static void ShareWebPage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, WebPageData data, IXDShareCallback callback){
            XDShareMobileImpl.GetInstance().ShareWebPage(target, scene, data, callback);
        }

        public static void IsTargetInstalled(ShareConstants.ShareTarget target, Action<bool> callback){
            XDShareMobileImpl.GetInstance().IsTargetInstalled(target, callback);
        }

        #region Interface
        void IXDShare.ShareText(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string text, IXDShareCallback callback)
        {
            ShareText(target, scene, text, callback);
        }

        void IXDShare.ShareImage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string imageUri, IXDShareCallback callback){
            ShareImage(target, scene, imageUri, callback);
        }

        void IXDShare.ShareWebPage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, WebPageData data, IXDShareCallback callback){
            ShareWebPage(target, scene, data, callback);
        }

        void IXDShare.IsTargetInstalled(ShareConstants.ShareTarget target, Action<bool> callback){
            IsTargetInstalled(target, callback);
        }
        #endregion
    }
}
#endif