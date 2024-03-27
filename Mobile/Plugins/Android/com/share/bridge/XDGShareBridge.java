package com.xd.share.bridge;

import com.unity3d.player.UnityPlayer;
import com.xd.sharing.mix.XDGSharing;
import com.xd.sharing.mix.base.SharePlatformType;
import com.xd.sharing.mix.util.ShareInternalUtility;

public class XDGShareBridge {
    public static boolean isAppInstalled(int platformType) {
        SharePlatformType type = ShareInternalUtility.getSharePlatformByType(platformType);
        if (type != null) {
            return XDGSharing.isAppInstalled(UnityPlayer.currentActivity, type);
        }
        
        return false;
    }
}
