#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace XD.SDK.Common.PC.Internal {
    public class PayModule {
        // 查询补款订单信息
        private readonly static string XDG_PAYBACK_LIST = "order/v1/user/repayOrders";

        internal readonly static int PAY_DONE = 0;
        internal readonly static int PAY_CANCEL = -1;

        internal readonly static string PAY_FRAGMENT_SUCCESS = "#success";
        internal readonly static string PAY_FRAGMENT_FAILURE = "#fail";
        internal readonly static string PAY_FRAGMENT_CANCEL = "#cancel";


        public static string GetPayUrl(string serverId, string roleId, string productId,
            string orderId = null,
            string productName = null,
            double payAmount = 0,
            string extras = null) {

            Dictionary<string, object> queryParmas = new Dictionary<string, object> {
                { "appId", ConfigModule.AppId },
                { "serverId", serverId },
                { "roleId", roleId },
                { "productSkuCode", productId },
                { "lang", XD.SDK.Common.PC.Internal.Localization.GetLanguageKey() },
                { "platform", Platform },
                { "sign", Sign(serverId, roleId, productId, ConfigModule.ClientId) }
            };
            if (!string.IsNullOrEmpty(ConfigModule.Region)) {
                queryParmas.Add("region", ConfigModule.Region);
            }
            if (!string.IsNullOrEmpty(orderId)) {
                queryParmas.Add("orderId", orderId);
            }
            if (!string.IsNullOrEmpty(productName)) {
                queryParmas.Add("productName", productName);
            }
            if (Math.Abs(payAmount) > float.Epsilon) {
                queryParmas.Add("payAmount", payAmount);
            }
            if (!string.IsNullOrEmpty(extras)) {
                queryParmas.Add("extras", extras);
            }

            return $"{ConfigModule.WebPayUrlForPC}?{XDHttpClient.EncodeQueryParams(queryParmas)}";
        }

        public static async Task<List<PayInfo>> CheckPay() {
            var user = await UserModule.GetLocalUser();

            Dictionary<string, object> param = new Dictionary<string, object>() {
                { "userId", user.userId },
            };
            PayCheckResponse response = await XDHttpClient.Get<PayCheckResponse>(XDG_PAYBACK_LIST,
                queryParams: param);
            return response.Result.List;
        }

        static string Sign(string serverId, string roleId, string productId, string clientId) {
            MD5 md5 = MD5.Create();
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes($"{productId}{roleId}{serverId}{timestamp}{clientId}"));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++) {
                sb.Append(data[i].ToString("x2"));
            }
            return $"{sb},{timestamp}";
        }

        static string Platform {
            get {
#if UNITY_STANDALONE_OSX
                return "macOS";
#elif UNITY_STANDALONE_WIN
                return "Windows";
#elif UNITY_STANDALONE_LINUX
                return "Linux";
#elif UNITY_ANDROID
                return "Android";
#elif UNITY_IOS
                return "iOS";
#else
                return "unknown";
#endif
            }
        }


    }
}
#endif
