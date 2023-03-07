using System;
using System.Collections.Generic;

namespace XD.SDK.Payment.Internal
{
    public interface IXDGPayment
    {
        void PayWithProduct(string orderId, string productId, string roleId, string serverId, string ext,
            Action<XDGOrderInfoWrapper> callback);

        void PayWithWeb(
            string orderId,
            string productId,
            string productName,
            double payAmount,
            string roleId,
            string serverId,
            string extras,
            Action<WebPayResultType, string> callback);

        void QueryWithProductIds(string[] productIds, Action<XDGSkuDetailInfo> callback);

        void QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback);

        void RestorePurchase(string purchaseToken, string orderId, string productId, string roleId,
            string serverId, string ext, Action<XDGOrderInfoWrapper> callback);

        void CheckRefundStatus(Action<XDGRefundResultWrapper> callback);

        void CheckRefundStatusWithUI(Action<XDGRefundResultWrapper> callback);
    }
}