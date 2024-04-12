package com.xd.account.bridge;

import com.xd.intl.common.net.CommonParameterManager;

public class XDGAccountBridge {
    public static String getAuthorization(String url, String method, long timestamp) {
        return CommonParameterManager.getInstance().getRequestAuthorizationHeaderString(url, method, timestamp * 1000);
    }
}
