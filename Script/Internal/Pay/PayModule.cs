#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using XD.SDK.Payment;
using XD.SDK.Account;
using XD.SDK.Common.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class PayModule {
        // 查询补款订单信息
        private readonly static string XDG_PAYBACK_LIST = "order/v1/user/repayOrders";

        // 创建订单
        private readonly static string XDG_CREATE_ORDER = "trade/v1/client/createOrder";
        // 上报订单
        private readonly static string XDG_REPORT_ORDER = "callback/v1/client/pay/notify";

        // 查询商品
        private readonly static string XDG_QUERY_PRODUCTS = "payment/product/v1/query/game/skus";

        internal readonly static int PAY_DONE = 0;
        internal readonly static int PAY_CANCEL = -1;

        internal readonly static string PAY_FRAGMENT_SUCCESS = "#success";
        internal readonly static string PAY_FRAGMENT_FAILURE = "#fail";
        internal readonly static string PAY_FRAGMENT_CANCEL = "#cancel";

        private readonly static int STEAM_CHANNEL_TYPE = 17;
        private readonly static int STEAM_PAYMENT_TYPE = 0;
        private readonly static int STEAM_PAY_ORDER_TYPE = 0;

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
                { "sign", Sign(serverId, roleId, productId, ConfigModule.ClientId) },
                { "eventSessionId", AliyunTrack.PaymentEventSessionId},
                { "account", UserModule.current?.userId },
                { "loginType", XDGAccount.GetLoginTypeString(XDGAccount.LoginType) },
                { "userLoginType", UserModule.current?.loginType },
                { "deviceId", SystemInfo.deviceUniqueIdentifier },
                { "xdClientId", ConfigModule.ClientId },
                { "sessionUUID", AliyunTrack.CurrentSessionUUId },
                { "sdkVer", XDGCommonPC.SDKVersion },
                { "os", AliyunTrack.GetPlatform() },
                { "os_version", SystemInfo.operatingSystem }
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

            return $"{ConfigModule.WebPayUrlForPC}?{XDUrlUtils.ToQueryString(queryParmas)}";
        }

        public static async Task<List<PayInfo>> CheckPay(CancellationToken cancellationToken = default) {
            var user = await UserModule.GetLocalUser();

            Dictionary<string, object> param = new Dictionary<string, object>() {
                { "userId", user.userId },
            };
            PayCheckResponse response = await XDHttpClient.Get<PayCheckResponse>(XDG_PAYBACK_LIST,
                queryParams: param,
                cancellationToken: cancellationToken);
            return response.Result.List;
        }

        public static async Task PayWithSteam(string productId, string serverId, string roldId, string orderId, string ext) {
            // 开始支付
            AliyunTrack.PaymentStart(productId, AliyunTrack.PAY_PLATFORM_STEAM, AliyunTrack.PAY_TYPE_STEAM, AliyunTrack.PAY_CHANNEL_STEAM);
            string xdOrderId = null;
            try {
                CreateOrderResult createOrderResult = await CreateSteamOrder(productId, serverId, roldId, orderId, ext);

                string microTxn = await SteamUtils.Instance.GetMicroTxn(createOrderResult.TradeNo);
                AliyunTrack.ReceiveSteamOrder(createOrderResult.TradeNo.ToString());

                await ReportSteamOrder(productId, createOrderResult.TradeNo, microTxn);
            } catch (XDException e) {
                // 支付失败
                AliyunTrack.PayFailure(xdOrderId, e.code, e.error_msg);
                throw e;
            }
            catch (Exception e) {
                // 支付失败
                AliyunTrack.PayFailure(xdOrderId, XDException.DEFAULT_CODE, e.Message);
                throw e;
            }
        }

        private static async Task<CreateOrderResult> CreateSteamOrder(string productId, string serverId, string roldId, string orderId, string ext) {
            // 创建订单
            AliyunTrack.CreateOrder();
            try {
                string steamId = SteamUtils.Instance.GetSteamId();
                string steamLanguage = SteamUtils.Instance.GetSteamLanguage();
                Dictionary<string, object> data = new Dictionary<string, object> {
                    { "productId", productId },
                    { "appServerId", serverId },
                    { "appRoleId", roldId },
                    { "outTradeNo", orderId },
                    { "channelType", STEAM_CHANNEL_TYPE },
                    { "paymentType", STEAM_PAYMENT_TYPE },
                    { "orderType", STEAM_PAY_ORDER_TYPE },
                    { "steamId", steamId },
                    { "steamLanguage", steamLanguage },
                };
                if (string.IsNullOrEmpty(ext)) {
                    data["remark"] = ext;
                }
                CreateOrderResponse response = await XDHttpClient.Post<CreateOrderResponse>(XDG_CREATE_ORDER, data: data);
                // 创建订单成功
                AliyunTrack.CreateOrderSuccess();
                return response.Result;
            } catch (XDException e) {
                // 创建订单失败
                AliyunTrack.CreateOrderFailure(e.code);
                if (e.code == 40023 || e.code == 42902) {
                    // 防沉迷错误
                    TaskCompletionSource<CreateOrderResult> tcs = new TaskCompletionSource<CreateOrderResult>();
                    LocalizableString localizable = Localization.GetCurrentLocalizableString();
                    Dictionary<string, object> config = new Dictionary<string, object> {
                        { Dialog.TITLE_KEY, e.extra_data["title"] },
                        { Dialog.CONTENT_KEY, e.extra_data["content"] },
                        { Dialog.PRIMARY_TEXT_KEY, localizable.Confirm2 }
                    };
                    UIManager.ShowUI<Dialog>("Dialog", config, (code, data) => {
                        UIManager.Dismiss();
                        tcs.TrySetException(e);
                    });
                    return await tcs.Task;
                } else {
                    throw e;
                }
            } catch (Exception e) {
                // 创建订单失败
                AliyunTrack.CreateOrderFailure(XDException.DEFAULT_CODE);
                throw e;
            }
        }

        private static async Task<ReportOrderResult> ReportSteamOrder(string productId, ulong tradeNo, string microTxn) {
            // 上报票据
            AliyunTrack.ReportOrder(tradeNo.ToString());
            try {
                Dictionary<string, object> data = new Dictionary<string, object> {
                    { "productId", productId },
                    { "tradeNo", tradeNo },
                    { "rawCallback", microTxn },
                    { "channelType", STEAM_CHANNEL_TYPE },
                    { "paymentType", STEAM_PAYMENT_TYPE },
                    { "orderType", STEAM_PAY_ORDER_TYPE },
                };
                ReportOrderResponse response = await XDHttpClient.Post<ReportOrderResponse>(XDG_REPORT_ORDER, data: data);
                // 上报票据成功
                AliyunTrack.ReportOrderSuccess(tradeNo.ToString());
                return response.Result;
            } catch (XDException e) {
                // 上报票据失败
                AliyunTrack.ReportOrderFailure(tradeNo.ToString(), e.code, e.error_msg);
                throw e;
            }
        }

        public static async Task<List<SkuDetailBean>> QuerySteamProducts(string[] productIds) {
            if (productIds == null || productIds.Length == 0) {
                return new List<SkuDetailBean>();
            }

            string steamId = SteamUtils.Instance.GetSteamId();
            Dictionary<string, object> queryParams = new Dictionary<string, object> {
                { "skus", string.Join(",", productIds) },
                { "steamId", steamId }
            };
            QueryProductsResponse response = await XDHttpClient.Get<QueryProductsResponse>(XDG_QUERY_PRODUCTS,
                queryParams: queryParams);
            return response.Products;
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
