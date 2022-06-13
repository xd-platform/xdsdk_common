//
//  TDSGlobalSDKLoginService.h
//  XDGAccountSDK
//
//  Created by JiangJiahao on 2020/11/23.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGLoginService : NSObject

//方法名 login 弹框登录
+ (void)login:(NSString*)login  bridgeCallback:(void (^)(NSString * _Nonnull))callback;
+ (void)logout;

// TDS authentication
+ (void)loginSync:(void(^)(NSString *result))callback;

+ (void)addUserStatusChangeCallback:(void (^)(NSString *result))callback;

+ (void)getUser:(void (^)(NSString *result))callback;

+ (void)openUserCenter;

+ (void)loginType:(NSString *)loginType bridgeCallback:(void (^)(NSString * _Nonnull))callback;

+ (void)accountCancellation;

@end

NS_ASSUME_NONNULL_END
