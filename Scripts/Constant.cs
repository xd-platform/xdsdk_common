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
    }

    public class GlobalUnKnowError
    {
        public static int UN_KNOW = 0x9009;
    }
}