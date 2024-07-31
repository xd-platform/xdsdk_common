//
//  XDGPaymentService.h
//  XDGPaymentSDK
//
//  Created by JiangJiahao on 2020/11/23.
// unity 桥接

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGPaymentService : NSObject
// 查询  queryWithProductIds ：
+ (void)productIds:(NSArray *)productIds bridgeCallback:(void (^)(NSString *result))callback;

// 支付  payWithProduct
+ (void)orderId:(NSString *)orderId
             productId:(NSString *)productId
                roleId:(NSString *)roleId
              serverId:(NSString *)serverId
                   ext:(NSString *)ext
     bridgeCallback:(void (^)(NSString *result))callback;

+ (void)queryRestoredPurchases:(void (^)(NSString *result))callback;

+ (void)purchaseToken:(NSString *)transactionIdentifier
              productId:(NSString *)productIdentifier
                orderId:(NSString *)orderId
                 roleId:(NSString *)roleId
               serverId:(NSString *)serverId
                    ext:(NSString *)ext
      bridgeCallback:(void (^)(NSString *result))callback;

///  不带界面的补款
+ (void)checkRefundStatus:(void (^)(NSString *result))callback;

//带UI界面的补款
+ (void)checkRefundStatusWithUI:(void (^)(NSString *result))callback;


@end

NS_ASSUME_NONNULL_END
