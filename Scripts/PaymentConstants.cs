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
    }
}