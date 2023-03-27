using System;
using System.Collections.Generic;

namespace XD.SDK.Share
{
    public interface IXDShare
    {
        void ShareText(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string text, IXDShareCallback callback);

        void ShareImage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string imageUri, IXDShareCallback callback);

        void ShareWebPage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, WebPageData data, IXDShareCallback callback);

        void IsTargetInstalled(ShareConstants.ShareTarget target, Action<bool> callback);
    }
}