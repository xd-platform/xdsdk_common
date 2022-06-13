using System;
using System.Collections.Generic;
using XD.SDK.Common;

namespace XD.SDK.Payment
{
    public interface IPaymentAPI
    {
        void PayWithProduct(string orderId, string productId, string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback);
        void PayWithWeb(string serverId, string roleId, string productId, string extras,Action<XDGError> callback);
        void QueryWithProductIds(string[] productIds, Action<XDGSkuDetailInfo> callback);
        void QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback);
        void RestorePurchase(string purchaseToken, string orderId, string productId, string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback);
        void CheckRefundStatus(Action<XDGRefundResultWrapper> callback);
        void CheckRefundStatusWithUI(Action<XDGRefundResultWrapper> callback);
        
        void InlinePay(string orderId, string productId, string productName, string region, string serverId,
            string roleId, string ext, Action<XDGInlinePayResult> callback);
    }
}