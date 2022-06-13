using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common;

namespace XD.SDK.Payment{
    public class XDGPaymentImpl : IPaymentAPI{
        private readonly string XDG_PAYMENT_SERVICE = "XDGPaymentService"; //注意要和iOS本地桥接类名一样

        private XDGPaymentImpl(){
            EngineBridge.GetInstance()
                .Register(XDGUnityBridge.PAYMENT_SERVICE_NAME, XDGUnityBridge.PAYMENT_SERVICE_IMPL);
        }

        private static volatile XDGPaymentImpl _instance;
        private static readonly object Locker = new object();

        public static XDGPaymentImpl GetInstance(){
            lock (Locker){
                if (_instance == null){
                    _instance = new XDGPaymentImpl();
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
            var dic = new Dictionary<string, object>{
                {"orderId", orderId},
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

        public void PayWithWeb(string serverId, string roleId, string productId, string extras,
            Action<XDGError> callback){
            var dic = new Dictionary<string, object>{
                {"serverId", serverId},
                {"roleId", roleId},
                {"productId", productId},
                {"extras", extras},
            };
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("payWithWeb")
                .Args(dic)
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance()
                .CallHandler(command, result => {
                    XDGTool.Log("PayWithWeb 方法结果: " + result.ToJSON());
                    callback(!XDGTool.checkResultSuccess(result)
                        ? new XDGError(result.code, result.message)
                        : new XDGError(result.content));
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
            var dic = new Dictionary<string, object>{
                {"purchaseToken", purchaseToken},
                {"productId", productId},
                {"orderId", orderId},
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

        public void CheckRefundStatus(Action<XDGRefundResultWrapper> callback){
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("checkRefundStatus")
                .Callback(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, (result) => {
                XDGTool.Log("CheckRefundStatus result: " + result.ToJSON());
                callback(new XDGRefundResultWrapper(result.content));
            });
        }

        public void CheckRefundStatusWithUI(Action<XDGRefundResultWrapper> callback){
            var command = new Command.Builder()
                .Service(XDG_PAYMENT_SERVICE)
                .Method("checkRefundStatusWithUI")
                .Callback(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command,
                result => {
                    XDGTool.Log("CheckRefundStatusWithUI result: " + result.ToJSON());
                    callback(new XDGRefundResultWrapper(result.content));
                });
        }

        public void InlinePay(string orderId, string productId, string productName, string region, string serverId,
            string roleId,
            string ext, Action<XDGInlinePayResult> callback){
            // var dic = new Dictionary<string, object>
            // {
            //     { "orderId", orderId },
            //     { "productId", productId },
            //     { "productName", productName },
            //     { "region", region },
            //     { "serverId", serverId },
            //     { "roleId", roleId },
            //     { "ext", ext }
            // };
            // var command = new Command(XDG_PAYMENT_SERVICE, "inlinePay", true, dic);
            // EngineBridge.GetInstance().CallHandler(command, (result) =>
            // {
            //     Debug.Log("InlinePay bridge result:" + result.ToJSON());
            //     if (!XDGTool.checkResultSuccess(result))
            //     {
            //         var payResult = new XDGInlinePayResult(GlobalUnKnowError.UN_KNOW,
            //             $"InlinePay Failed:{result.message}");
            //
            //         callback(payResult);
            //         return;
            //     }
            //
            //     callback(new XDGInlinePayResult(result.content));
            // });
        }

        public void QueryProductList(string[] productIds, Action<ProductSkuWrapper> callback){
            // var dic = new Dictionary<string, object>
            // {
            //     { "productIds", productIds }
            // };
            //
            // var command = new Command.Builder()
            //     .Service(XDG_PAYMENT_SERVICE)
            //     .Method("queryProductList")
            //     .Args(dic)
            //     .Callback(true)
            //     .CommandBuilder();
            //
            // EngineBridge.GetInstance().CallHandler(command, result =>
            // {
            //     XDGTool.Log("queryWithProductIds 方法结果: " + result.ToJSON());
            //     callback(new ProductSkuWrapper(result.content));
            // });
        }
    }
}