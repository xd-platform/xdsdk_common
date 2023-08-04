using System;
using System.Collections.Generic;
using LC.Newtonsoft.Json;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common;

namespace  XD.SDK.Payment{
    public class XDGPaymentMobileImpl{
        //Payment
        private static string PAYMENT_SERVICE_NAME = "com.xd.intl.payment.unityBridge.XDGPaymentService";
        private static string PAYMENT_SERVICE_IMPL = "com.xd.intl.payment.unityBridge.XDGPaymentServiceImpl";
        private readonly string XDG_PAYMENT_SERVICE = "XDGPaymentService"; //注意要和iOS本地桥接类名一样

        private XDGPaymentMobileImpl(){
            EngineBridge.GetInstance()
                .Register(PAYMENT_SERVICE_NAME, PAYMENT_SERVICE_IMPL);
        }

        private static volatile XDGPaymentMobileImpl _instance;
        private static readonly object Locker = new object();

        public static XDGPaymentMobileImpl GetInstance(){
            lock (Locker){
                if (_instance == null){
                    _instance = new XDGPaymentMobileImpl();
                }
            }

            return _instance;
        }

        public void QueryWithProductIds(string[] productIds, Action<XDGSkuDetailInfo> callback){
            var dic = new Dictionary<string, object>{
                {"productIds", productIds}
            };

            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("queryWithProductIds")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("queryWithProductIds 方法结果: " + result.ToJSON());
                callback(new XDGSkuDetailInfoMobile(result.content) as XDGSkuDetailInfo);
            });
        }

        public void PayWithProduct(string orderId, string productId, string roleId, string serverId, string ext,
            Action<XDGOrderInfoWrapper> callback){

            var oid = orderId;
            if (string.IsNullOrEmpty(oid)){
                oid = XDGTool.GetRandomStr(12) + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + "";
            }

            var dic = new Dictionary<string, object>{
                {"orderId", oid},
                {"productId", productId},
                {"roleId", roleId},
                {"serverId", serverId},
                {"ext", ext}
            };

            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("payWithProduct")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command,
                (result) => {
                    XDGTool.Log("PayWithProduct 方法结果: " + result.ToJSON());

                    callback(new XDGOrderInfoWrapperMobile(result.content) as XDGOrderInfoWrapper);
                });
        }

        public void PayWithWeb(
            string orderId,
            string productId,
            string productName,
            double payAmount,
            string roleId,
            string serverId,
            string extras,
            Action<WebPayResultType, string> callback){
            
            var oid = orderId;
            if (string.IsNullOrEmpty(oid)){
                oid = XDGTool.GetRandomStr(12) + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + "";
            }
            
            var dic = new Dictionary<string, object>{
                {"orderId", oid},
                {"productId", productId},
                {"productName", productName},
                {"payAmount", payAmount},
                {"roleId", roleId},
                {"serverId", serverId},
                {"extras", extras},
            };
            XDGTool.Log("PayWithWeb 参数: " + JsonConvert.SerializeObject(dic));
            
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("payWithWeb")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance()
                .CallHandler(command, result => {
                    XDGTool.Log("PayWithWeb 方法结果: " + result.ToJSON());

                    if (XDGTool.checkResultSuccess(result)){
                        Dictionary<string, object> resultDic =
                            Json.Deserialize(result.content) as Dictionary<string, object>;
                        var code = SafeDictionary.GetValue<int>(resultDic, "code");
                        var message = SafeDictionary.GetValue<string>(resultDic, "message");

                        if (code == 0){
                            callback(WebPayResultType.OK, "支付完成");
                        } else if (code == 1){
                            callback(WebPayResultType.Cancel, "支付取消");
                        } else if (code == 2){
                            callback(WebPayResultType.Processing, "支付处理中");
                        } else{
                            callback(WebPayResultType.Error, message);
                            XDGTool.LogError($"安卓网页支付失败, 参数：{JsonConvert.SerializeObject(dic)} 结果：{result.ToJSON()}");
                        }
                    } else{
                        callback(WebPayResultType.Error, "支付结果解析失败");
                        XDGTool.LogError($"支付结果解析失败, 参数：{JsonConvert.SerializeObject(dic)} 结果：{result.ToJSON()}");
                    }
                });
        }

        public void QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback){
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("queryRestoredPurchases")
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command,
                (result) => {
                    XDGTool.Log("QueryRestoredPurchase 方法结果: " + result.ToJSON());

                    if (!XDGTool.checkResultSuccess(result)){
                        callback(null);
                        return;
                    }

                    callback(new XDGRestoredPurchaseWrapper(result.content).transactionList);
                });
        }

        public void RestorePurchase(string purchaseToken, string orderId, string productId, string roleId,
            string serverId, string ext, Action<XDGOrderInfoWrapper> callback){
            
            var oid = orderId;
            if (string.IsNullOrEmpty(oid)){
                oid = XDGTool.GetRandomStr(12) + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + "";
            }

            var dic = new Dictionary<string, object>{
                {"purchaseToken", purchaseToken},
                {"productId", productId},
                {"orderId", oid},
                {"roleId", roleId},
                {"serverId", serverId},
                {"ext", ext}
            };
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("restorePurchase")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, (result) => {
                XDGTool.Log("RestorePurchase 方法结果: " + result.ToJSON());
                callback(new XDGOrderInfoWrapperMobile(result.content) as XDGOrderInfoWrapper);
            });
        }

        public void CheckRefundStatus(Action<XDGRefundResultWrapperMobile> callback){
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("checkRefundStatus")
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, (result) => {
                XDGTool.Log("CheckRefundStatus result: " + result.ToJSON());
                callback(new XDGRefundResultWrapperMobile(result.content));
            });
        }

        public void CheckRefundStatusWithUI(Action<XDGRefundResultWrapperMobile> callback){
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("checkRefundStatusWithUI")
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command,
                result => {
                    XDGTool.Log("CheckRefundStatusWithUI result: " + result.ToJSON());
                    callback(new XDGRefundResultWrapperMobile(result.content));
                });
        }
        
        /// <summary>
        /// 微信支付和支付宝支付的原生实现,带 UI 方式
        /// </summary>
        /// <param name="orderId">游戏订单 ID，可为空，为空时采用随机字符串 ID 作为游戏 ID</param>
        /// <param name="productId">游戏商品 ID，不可为空</param>
        /// <param name="productName">游戏商品名称，使用 SDK 自带 UI 方式进行支付时作为弹窗预显示的游戏名称，可为空</param>
        /// <param name="payAmount">商品价格，单位（元），使用 SDK 自带 UI 方式进行支付时作为弹窗预显示的商品价格，可为 0</param>
        /// <param name="roleId">游戏角色 ID，支付角色 ID，服务端支付回调会包含该字段</param>
        /// <param name="serverId">游戏服务器 ID，所在服务器 ID，不能有特殊字符，服务端支付回调会包含该字段</param>
        /// <param name="ext">附加信息。服务端支付回调会包含该字段</param>
        public void PayWithUI(string orderId, string productId, string productName, double payAmount,
            string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback)
        {
            var oid = orderId;
            if (string.IsNullOrEmpty(oid)){
                oid = XDGTool.GetRandomStr(12) + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + "";
            }
            
            var dic = new Dictionary<string, object>{
                {"orderId", oid ?? ""},
                {"productId", productId ?? ""},
                {"productName", productName ?? ""},
                {"payAmount", payAmount},
                {"roleId", roleId ?? ""},
                {"serverId", serverId ?? ""},
                {"ext", ext ?? ""}
            };
            
            XDGTool.Log("PayWithUI 参数: " + JsonConvert.SerializeObject(dic));
            
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("payWithUI")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command,
                (result) => {
                    XDGTool.Log("PayWithUI 方法结果: " + result.ToJSON());
                    callback(new XDGOrderInfoWrapperMobile(result.content) as XDGOrderInfoWrapper);
                });
        }
        
        /// <summary>
        /// 微信支付和支付宝支付的原生实现,不带 UI 方式
        /// </summary>
        /// <param name="orderId">游戏订单 ID，可为空，为空时采用随机字符串 ID 作为游戏 ID</param>
        /// <param name="productId">游戏商品 ID，不可为空</param>
        /// <param name="productName">游戏商品名称，使用 SDK 自带 UI 方式进行支付时作为弹窗预显示的游戏名称，可为空</param>
        /// <param name="payAmount">商品价格，单位（元），使用 SDK 自带 UI 方式进行支付时作为弹窗预显示的商品价格，可为 0</param>
        /// <param name="roleId">游戏角色 ID，支付角色 ID，服务端支付回调会包含该字段</param>
        /// <param name="serverId">游戏服务器 ID，所在服务器 ID，不能有特殊字符，服务端支付回调会包含该字段</param>
        /// <param name="ext">附加信息。服务端支付回调会包含该字段</param>
        public void Pay(PayType payType, string orderId, string productId, string productName, double payAmount,
            string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback)
        {
            var oid = orderId;
            if (string.IsNullOrEmpty(oid)){
                oid = XDGTool.GetRandomStr(12) + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + "";
            }

            string payTypeString = null;
            switch (payType)
            {
                case PayType.Alipay:
                    payTypeString = PaymentConstants.PayTypeInternal.ALIPAY;
                    break;
                case PayType.WechatPay:
                    payTypeString = PaymentConstants.PayTypeInternal.WECHAT_PAY;
                    break;
            }
            
            var dic = new Dictionary<string, object>{
                {"payType", payTypeString},
                {"orderId", oid ?? ""},
                {"productId", productId ?? ""},
                {"productName", productName ?? ""},
                {"payAmount", payAmount},
                {"roleId", roleId ?? ""},
                {"serverId", serverId ?? ""},
                {"ext", ext ?? ""}
            };
            XDGTool.Log("Pay 参数: " + JsonConvert.SerializeObject(dic));
            
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("pay")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command,
                (result) => {
                    XDGTool.Log("Pay 方法结果: " + result.ToJSON());

                    callback(new XDGOrderInfoWrapperMobile(result.content) as XDGOrderInfoWrapper);
                });
        }
    }
}