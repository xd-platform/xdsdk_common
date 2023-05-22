namespace XD.SDK.Payment
{
    public class PaymentConstants
    {
        public static class PaymentResponseCode
        {
            public static int OK = 0;
            public static int ERROR = -1;
            public static int NETWORK_ERROR = -2;
            public static int USER_CANCEL = 1;
            public static int PURCHASE_PROCESSING = 2;
        }
        
        internal static class PayTypeInternal
        {
            public const string ALIPAY = "alipay";
            public const string WECHAT_PAY = "wechat_pay";
        }
    }
    
    public enum PayType
    {
        Alipay,
        WechatPay,
    }
}