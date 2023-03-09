#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using XD.SDK.Common;
using XD.SDK.Payment.Internal;

namespace XD.SDK.Payment{
    public class XDGPaymentMobile : IXDGPayment{
        public static void PayWithProduct(string orderId, string productId, string roleId, string serverId, string ext,
            Action<XDGOrderInfoWrapper> callback){
            XDGPaymentMobileImpl.GetInstance().PayWithProduct(orderId, productId, roleId, serverId, ext, callback);
        }

        public static void PayWithWeb(
            string orderId,
            string productId,
            string productName,
            double payAmount,
            string roleId, 
            string serverId,
            string extras,
            Action<WebPayResultType, string> callback){
            XDGPaymentMobileImpl.GetInstance().PayWithWeb(orderId, productId, productName, payAmount, roleId, serverId, extras, callback);
        }

        public static void QueryWithProductIds(string[] productIds, Action<XDGSkuDetailInfo> callback){
            XDGPaymentMobileImpl.GetInstance().QueryWithProductIds(productIds, callback);
        }

        public static void QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback){
            XDGPaymentMobileImpl.GetInstance().QueryRestoredPurchase(callback);
        }

        public static void RestorePurchase(string purchaseToken, string orderId, string productId, string roleId,
            string serverId, string ext, Action<XDGOrderInfoWrapper> callback){
            XDGPaymentMobileImpl.GetInstance()
                .RestorePurchase(purchaseToken, orderId, productId, roleId, serverId, ext, callback);
        }

        public static void CheckRefundStatus(Action<XDGRefundResultWrapper> callback){
            XDGPaymentMobileImpl.GetInstance().CheckRefundStatus(callback);
        }

        public static void CheckRefundStatusWithUI(Action<XDGRefundResultWrapper> callback){
            XDGPaymentMobileImpl.GetInstance().CheckRefundStatusWithUI(callback);
        }

        #region Interface
        void IXDGPayment.PayWithProduct(string orderId, string productId, string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback)
        {
            PayWithProduct(orderId, productId, roleId, serverId, ext, callback);
        }

        void IXDGPayment.PayWithWeb(string orderId, string productId, string productName, double payAmount, string roleId, string serverId,
            string extras, Action<WebPayResultType, string> callback)
        {
            PayWithWeb(orderId, productId, productName, payAmount, roleId, serverId, extras, callback);
        }

        void IXDGPayment.QueryWithProductIds(string[] productIds, Action<XDGSkuDetailInfo> callback)
        {
            QueryWithProductIds(productIds, callback);
        }

        void IXDGPayment.QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback)
        {
            QueryRestoredPurchase(callback);
        }

        void IXDGPayment.RestorePurchase(string purchaseToken, string orderId, string productId, string roleId, string serverId, string ext,
            Action<XDGOrderInfoWrapper> callback)
        {
            RestorePurchase(purchaseToken, orderId, productId, roleId, serverId, ext, callback);
        }

        void IXDGPayment.CheckRefundStatus(Action<XDGRefundResultWrapper> callback)
        {
            CheckRefundStatus(callback);
        }

        void IXDGPayment.CheckRefundStatusWithUI(Action<XDGRefundResultWrapper> callback)
        {
            CheckRefundStatusWithUI(callback);
        }
        #endregion
    }
}
#endif