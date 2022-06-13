//
//  XDGLoginLogger.h
//  XDGCommonSDK
//
//  Created by Fattycat on 2022/4/14.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGCloudLogHelper.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGLoginLogger : NSObject
+ (instancetype)shareInstance;

- (NSString *)getCurrentSessionId;

- (void)clearCurrentSessionId;

// 开始登录
- (void)loginStart;

// 登录封控拦截成功
- (void)loginRiskSuccess:(NSError *)error;

// 跳转授权
- (void)login2Authorize;

// 授权成功
- (void)login2AuthorizeSuccess;

// 授权失败
- (void)login2AuthorizeFailed:(NSString *)reason;

// SDK 登陆成功
- (void)loginPreLoginSuccess;

// SDK 登录失败
- (void)loginPreLoginFailed:(NSString *)reason;

// 开始实名
- (void)loginAntiAddictionStartup;

// 实名结果
- (void)loginAntiAddictionResult:(NSString *)result;

// 点击实名弹窗按钮
- (void)loginExitAntiAddictionPopup:(NSInteger)buttonType;

// 登陆成功
- (void)loginSuccess;

// 登录失败
- (void)loginFailed:(NSString *)reason;
@end

NS_ASSUME_NONNULL_END
