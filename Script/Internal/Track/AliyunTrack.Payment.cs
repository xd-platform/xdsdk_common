#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XD.SDK.Common.PC.Internal {
    internal partial class AliyunTrack
    {
        private static string PaymentProcessProductId = "";
        
        private static string _paymentEventSessionId;
        
        public static string PaymentEventSessionId => _paymentEventSessionId;

        internal static void PaymentStart(string productId)
        {
            PaymentProcessProductId = productId;
            _paymentEventSessionId = GetNewEventSessionId();
            LogEventAsync("sdkcharge_request", GetPaymentModuleCommonProperties("sdkcharge_request"));
        }
        
        internal static void PaymentCallPage()
        {
            var content = GetPaymentModuleCommonProperties("sdkcharge_ask_pay_period");
            LogEventAsync("sdkcharge_ask_pay_period", content);
        }
        
        internal static void PaymentDone()
        {
            LogEventAsync("sdkcharge_done", GetPaymentModuleCommonProperties("sdkcharge_done"));
        }
        
        internal static void PaymentFailed(string reason)
        {
            var content = GetPaymentModuleCommonProperties("sdkcharge_fail");
            content["reason"] = reason;
            LogEventAsync("sdkcharge_fail", content);
        }

        private static Dictionary<string, string> GetPaymentModuleCommonProperties(string eventName)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content["tag"] = "sdkcharge";
            content["event_session_id"] = _paymentEventSessionId;
            content["pay_platform"] = "Web";
            content["product_id"] = PaymentProcessProductId;
            content["logid"] = GetLogId(AgreementEventSessionId, eventName);
            return content;
        }
    }
}
#endif