package com.xd.common.bridge;

import com.xd.intl.common.net.CommonParameterManager;
import com.xd.intl.common.utils.XDConfigManager;

public class XDGCommonBridge {
    public static boolean isCN() {
        return XDConfigManager.getInstance().isRegionTypeCN();
    }

    public static String getCommonQueryString(String url, long timestamp) {
        return CommonParameterManager.getInstance().buildNewRequestUrlString(url, timestamp * 1000);
    }
}
