#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XD.SDK.Common.PC.Internal {
    internal partial class AliyunTrack
    {
        internal const string PAY_PLATFORM_WEB = "Web";
        internal const string PAY_PLATFORM_STEAM = "Steam";

        internal const string PAY_TYPE_STEAM = "Steam";

        internal const string PAY_CHANNEL_STEAM = "Steam";

        private static string PaymentProcessProductId = "";

        private static string PaymentPlatform;

        private static string PaymentType;

        private static string PaymntChannel;

        private static string PaymentRoleId;
        
        private static string _paymentEventSessionId;
        
        public static string PaymentEventSessionId => _paymentEventSessionId;

        internal static void PaymentStart(string productId,
            string roleId,
            string platform = PAY_PLATFORM_WEB,
            string type = null,
            string channel = null)
        {
            PaymentProcessProductId = productId;
            PaymentRoleId = roleId;
            PaymentPlatform = platform;
            PaymentType = type;
            PaymntChannel = channel;

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

        internal static void CreateOrder() {
            string eventId = "sdkcharge_order_request";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            data["pick_country"] = SteamUtils.Instance.GetSteamCountry();
            LogEventAsync(eventId, data);
        }

        internal static void CreateOrderSuccess() {
            string eventId = "sdkcharge_order_create_success";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            LogEventAsync(eventId, data);
        }

        internal static void CreateOrderFailure(int code) {
            string eventId = "sdkcharge_order_create_fail";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            data["status"] = code.ToString();
            LogEventAsync(eventId, data);
        }

        internal static void ReceiveSteamOrder(string orderId) {
            string eventId = "sdkcharge_payment_issue";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            data["order_id"] = orderId;
            LogEventAsync(eventId, data);
        }

        internal static void ReportOrder(string orderId) {
            string eventId = "sdkcharge_receipt_report";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            data["order_id"] = orderId;
            LogEventAsync(eventId, data);
        }

        internal static void ReportOrderSuccess(string orderId) {
            string eventId = "sdkcharge_payment_upload_success";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            data["order_id"] = orderId;
            LogEventAsync(eventId, data);
        }

        internal static void ReportOrderFailure(string orderId, int code, string msg) {
            string eventId = "sdkcharge_payment_upload_fail";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            data["order_id"] = orderId;
            data["status"] = code.ToString();
            data["reason"] = msg;
            LogEventAsync(eventId, data);
        }

        internal static void PayFailure(string orderId, int code, string msg) {
            string eventId = "sdkcharge_fail";
            Dictionary<string, string> data = GetPaymentModuleCommonProperties(eventId);
            data["order_id"] = orderId;
            data["status"] = code.ToString();
            data["reason"] = msg;
            LogEventAsync(eventId, data);
        }

        private static Dictionary<string, string> GetPaymentModuleCommonProperties(string eventName)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content["tag"] = "sdkcharge";
            content["event_session_id"] = _paymentEventSessionId;
            content["pay_platform"] = PaymentPlatform;
            if (PaymentType != null) {
                content["pay_type"] = PaymentType;
            }
            if (PaymntChannel != null) {
                content["pay_channel"] = PaymntChannel;
            }
            content["product_id"] = PaymentProcessProductId;
            content["role_id"] = PaymentRoleId;
            content["logid"] = GetLogId(PaymentEventSessionId, eventName);
            return content;
        }
    }
}
#endif