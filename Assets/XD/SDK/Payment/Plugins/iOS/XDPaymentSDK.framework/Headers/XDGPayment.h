
#import <Foundation/Foundation.h>


@class XDGProductInfo;
@class XDGTransactionInfo;
@class XDGOrderInfo;
NS_ASSUME_NONNULL_BEGIN


typedef NS_ENUM(NSInteger,XDGRepayMentCode) {
    XDGRepayMentCodeOk          = 0x00,                       // ok
    XDGRepayMentCodeError       = 0xff,                      // error
};
/**
  Describes the call back to the query of products
 @param result the result contains info of the products
 @param error error, if any.
 */
typedef void(^XDGQueryProductsCallback)(NSArray<XDGProductInfo *> *_Nullable result,NSError *_Nullable error);

/**
 Describes the call back to the query of unfinished transactions
@param result the result contains unfinished transactions
 */
typedef void(^XDGQueryRestoreProductsCallback)(NSArray<XDGTransactionInfo *> *result);

/**
 Describes the call back to the transaction
@param orderInfo info of the transaction
@param error error, if any.
 */
typedef void(^XDGPaymentCallback)(XDGOrderInfo *orderInfo,NSError *error);



typedef void(^XDGRePaymentCallback)(XDGRepayMentCode code,NSString * _Nullable msg,NSDictionary *_Nullable data);


@interface XDGPayment : NSObject
/// 查询商品价格,请等待回调之后再做下一次查询，否则可能造成数据错乱
/// @param productIds 商品ID集合
/// @param completionHandler 查询结果处理
+ (void)queryWithProductIds:(NSArray *)productIds completionHandler:(XDGQueryProductsCallback)completionHandler;

/// 支付商品
/// @param orderId 商品ID
/// @param productId 商品ID
/// @param roleId 角色ID
/// @param serverId 服务器ID
/// @param ext 支付额外信息EXT
/// @param completionHandler 支付结果处理
+ (void)payWithOrderId:(NSString *)orderId
             productId:(NSString *)productId
                roleId:(NSString *)roleId
              serverId:(NSString *)serverId
                   ext:(NSString *)ext
     completionHandler:(XDGPaymentCallback)completionHandler;

/// 查询当前是否有未处理订单或者礼包码
/// @param completionHandler 查询结果处理
+ (void)queryRestoredPurchases:(XDGQueryRestoreProductsCallback)completionHandler;

/// 恢复一笔订单/礼包码
/// @param restoreTransaction 需要回复的订单信息，queryRestoredProducts回调中返回
/// @param orderId 商品ID
/// @param roleId 角色ID
/// @param serverId 服务器ID
/// @param ext 支付EXT信息
/// @param completionHandler 回调处理
+ (void)restorePurchase:(XDGTransactionInfo *)restoreTransaction
                orderId:(NSString *)orderId
                 roleId:(NSString *)roleId
               serverId:(NSString *)serverId
                    ext:(NSString *)ext
      completionHandler:(XDGPaymentCallback)completionHandler;


///  不带界面的补款回调
/// @param  completionHandler 补款中的回调信息
///  code   错误码
///  msg    具体错误信心
///  data   数据
///  completionHandler 回调处理
+ (void)checkRefundStatus:(XDGRePaymentCallback)completionHandler;

//带UI界面的补款

+ (void)checkRefundStatusWithUI:(XDGRePaymentCallback)completionHandler;

@end

NS_ASSUME_NONNULL_END
