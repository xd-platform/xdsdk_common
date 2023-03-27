using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using XD.SDK.Payment.Internal;

[assembly: Preserve]
[assembly: AlwaysLinkAssembly]
namespace XD.SDK.Payment{
    public class XDGPayment{
        public static IXDGPayment platformWrapper;
        
        static XDGPayment() 
        {
            var interfaceType = typeof(IXDGPayment);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz)&& clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDGPayment;
            }
        }
        
        public static void PayWithProduct(string orderId, string productId, string roleId, string serverId, string ext,
            Action<XDGOrderInfoWrapper> callback){
            platformWrapper.PayWithProduct(orderId, productId, roleId, serverId, ext, callback);
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
            platformWrapper.PayWithWeb(orderId, productId, productName, payAmount, roleId, serverId, extras, callback);
        }

        public static void QueryWithProductIds(string[] productIds, Action<XDGSkuDetailInfo> callback){
            platformWrapper.QueryWithProductIds(productIds, callback);
        }

        public static void QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback){
            platformWrapper.QueryRestoredPurchase(callback);
        }

        public static void RestorePurchase(string purchaseToken, string orderId, string productId, string roleId,
            string serverId, string ext, Action<XDGOrderInfoWrapper> callback){
            platformWrapper.RestorePurchase(purchaseToken, orderId, productId, roleId, serverId, ext, callback);
        }

        public static void CheckRefundStatus(Action<XDGRefundResultWrapper> callback){
            platformWrapper.CheckRefundStatus(callback);
        }

        public static void CheckRefundStatusWithUI(Action<XDGRefundResultWrapper> callback){
            platformWrapper.CheckRefundStatusWithUI(callback);
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
        public static void PayWithUI(string orderId, string productId, string productName, double payAmount,
            string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback)
        {
            platformWrapper.PayWithUI(orderId, productId, productName, payAmount, roleId, serverId, ext, callback);
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
        public static void Pay(PayType payType, string orderId, string productId, string productName, double payAmount,
            string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback)
        {
            platformWrapper.Pay(payType, orderId, productId, productName, payAmount, roleId, serverId, ext, callback);
        }
    }
}