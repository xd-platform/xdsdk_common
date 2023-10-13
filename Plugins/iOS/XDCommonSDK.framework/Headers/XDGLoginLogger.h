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

// 跳转 Tap 权限页面
- (void)loginMobileTapPermissionPageOpen;

// 跳转 TapTap web 登录
- (void)loginMobileTapWebOpen:(NSString *)enterFrom reason:(nullable NSString *)reason;

// 跳转一键登录页面
- (void)loginMobileAliPageOpen:(NSString *)enterFrom;

// 跳转手机登录页面
- (void)loginMobileSMSPageOpen:(NSString *)enterFrom reason:(nullable NSString *)reason;

// 输入手机号页面点击按钮
- (void)loginMobileButtonClick:(NSObject *)type;

// 唤起输入验证码页面
- (void)loginMobileCodeInput:(NSString *)phone;

// 手机登录发送验证码成功
- (void)loginMobileCodeSendSuccess:(NSString *)phone;

// 手机登录发送验证码失败
- (void)loginMobileCodeSendFail:(NSString *)phone reason:(NSString *)reason;

// 手机登录验证码校验通过
- (void)loginMobileCodeVerifySuccess:(NSString *)phone;

// 手机登录验证码校验失败
- (void)loginMobileCodeVerifyFail:(NSString *)phone reason:(NSString *)reason;

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

// 退出登录
- (void)logout:(NSString *)reason;
@end

NS_ASSUME_NONNULL_END
