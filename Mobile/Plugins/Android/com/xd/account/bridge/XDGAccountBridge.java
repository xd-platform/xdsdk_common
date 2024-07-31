package com.xd.account.bridge;

import com.xd.intl.common.net.CommonParameterManager;
import com.xd.intl.common.bridge.BridgeUtil;
import com.xd.intl.common.global.GlobalUserStore;

public class XDGAccountBridge {
    public static String getAuthorization(String url, String method, long timestamp) {
        return CommonParameterManager.getInstance().getRequestAuthorizationHeaderString(url, method, timestamp * 1000);
    }
    
    public static String getCurrentUser() {
        return BridgeUtil.convertUserJsonString4Bridge(GlobalUserStore.INSTANCE.getCurrentXDUser());
    }
}
