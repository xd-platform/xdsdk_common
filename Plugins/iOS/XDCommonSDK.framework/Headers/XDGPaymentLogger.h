//
//  XDGPaymentLogger.h
//  XDGCommonSDK
//
//  Created by Fattycat on 2022/4/14.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGPaymentLogger : NSObject

+ (instancetype)shareInstance;

- (NSString *)getCurrentSeventSessionId;

// 开始支付
- (void)paymentStart:(NSString *)productId;

// 创建订单
- (void)createOrder;

// 创建订单失败
- (void)orderCreateFailed:(NSString *)statusCode reason:(NSString*)reason;

// 创建订单成功
- (void)orderCreateSuccess;

// 唤起支付页面
- (void)callPaymentPage;

// 下发支付票据
- (void)receivePurchaseToken:(NSString *)tradeNo appleTradeNo:(NSString *)appleTradeNo;

// 票据上报
- (void)uploadReceipt:(NSString *)tradeNo appleTradeNo:(NSString *)appleTradeNo;

// 票据上传成功
- (void)uploadReceiptSuccess:(NSString *)tradeNo appleTradeNo:(NSString *)appleTradeNo;

// 票据上传失败
- (void)uploadReceiptfail:(NSString *)reason tradeNo:(NSString *)tradeNo appleTradeNo:(NSString *)appleTradeNo;

// 票据消费成功
- (void)comsumeReceiptSuccess:(NSString *)tradeNo appleTradeNo:(NSString *)appleTradeNo;

// 票据消费失败
- (void)comsumeReceiptFail:(NSString *)reason tradeNo:(NSString *)tradeNo appleTradeNo:(NSString *)appleTradeNo;

// 支付失败
- (void)paymentFailed:(NSString *)reason outTradeNo:(NSString *)outTradeNo;

@end

NS_ASSUME_NONNULL_END
