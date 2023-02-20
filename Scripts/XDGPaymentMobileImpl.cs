using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common;

namespace XD.SDK.Payment{
    public class XDGPaymentMobileImpl{
        private readonly string XDG_PAYMENT_SERVICE = "XDGPaymentService"; //注意要和iOS本地桥接类名一样

        private XDGPaymentMobileImpl(){
            EngineBridge.GetInstance()
                .Register(XDGUnityBridge.PAYMENT_SERVICE_NAME, XDGUnityBridge.PAYMENT_SERVICE_IMPL);
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
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("queryWithProductIds 方法结果: " + result.ToJSON());
                callback(new XDGSkuDetailInfo(result.content));
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
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command,
                (result) => {
                    XDGTool.Log("PayWithProduct 方法结果: " + result.ToJSON());

                    callback(new XDGOrderInfoWrapper(result.content));
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
            XDGTool.Log("PayWithWeb 参数: " + JsonUtility.ToJson(dic));
            
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("payWithWeb")
                .Args(dic)
                .Callback(true)
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
                            XDGTool.LogError($"安卓网页支付失败, 参数：{JsonUtility.ToJson(dic)} 结果：{result.ToJSON()}");
                        }
                    } else{
                        callback(WebPayResultType.Error, "支付结果解析失败");
                        XDGTool.LogError($"支付结果解析失败, 参数：{JsonUtility.ToJson(dic)} 结果：{result.ToJSON()}");
                    }
                });
        }

        public void QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback){
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("queryRestoredPurchases")
                .Callback(true)
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
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, (result) => {
                XDGTool.Log("RestorePurchase 方法结果: " + result.ToJSON());
                callback(new XDGOrderInfoWrapper(result.content));
            });
        }

        public void CheckRefundStatus(Action<XDGRefundResultWrapperMobile> callback){
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("checkRefundStatus")
                .Callback(true)
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
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command,
                result => {
                    XDGTool.Log("CheckRefundStatusWithUI result: " + result.ToJSON());
                    callback(new XDGRefundResultWrapperMobile(result.content));
                });
        }
    }
}