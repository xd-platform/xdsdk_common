namespace XD.SDK.Common
{
    public class XDGUnityBridge
    {
        //Common
        public static string COMMON_SERVICE_NAME = "com.xd.intl.common.bridge.XDGCoreService";
        public static string COMMON_SERVICE_IMPL = "com.xd.intl.common.bridge.XDGCoreServiceImpl";
        
        //Account
        public static string ACCOUNT_SERVICE_NAME = "com.xd.intl.account.unitybridge.XDGLoginService";
        public static string ACCOUNT_SERVICE_IMPL = "com.xd.intl.account.unitybridge.XDGLoginServiceImpl";
        
        //Payment
        public static string PAYMENT_SERVICE_NAME = "com.xd.intl.payment.unityBridge.XDGPaymentService";
        public static string PAYMENT_SERVICE_IMPL = "com.xd.intl.payment.unityBridge.XDGPaymentServiceImpl";

        //Share
        public static string SHARE_SERVICE_NAME = "com.xd.share.cn.unityBridge.XDShareService";
        public static string SHARE_SERVICE_IMPL = "com.xd.share.cn.unityBridge.XDShareServiceImpl";

    }

    public class GlobalUnKnowError
    {
        public static int UN_KNOW = 0x9009;
    }
}