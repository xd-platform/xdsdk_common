//
//  XDGPhoneAuthManager.h
//  XDAccountSDK
//
//  Created by Fattycat on 2023/2/11.
//

#import <Foundation/Foundation.h>
#import <XDAccountSDK/TDSGlobalThirdPartyLoginManager.h>
#import <XDAccountSDK/TDSGlobalThirdPartyLoginHelper.h>
#import <XDCommonSDK/TDSGlobalHttpResult.h>

NS_ASSUME_NONNULL_BEGIN

typedef void (^XDGPhoneAuthSMSHanlder)(TDSGlobalHttpResult *_Nullable result);

@interface XDGPhoneAuthManager : NSObject

+ (XDGPhoneAuthManager *)sharedInstance;

+ (void)startPhoneLoginWithHandler:(XDGLoginManagerRequestCallback)handler fromViewController:viewController successCallback:(TDSGlobalAuthSuccessCalback)successCallback cancelCallback:(TDSGlobalAuthCancelCallback)cancelCallback errorCallback:(TDSGlobalAuthErrorCallback)errorCallback;

+ (void)bindPhoneFromViewController:viewController successCallback:(TDSGlobalAuthSuccessCalback)successCallback cancelCallback:(TDSGlobalAuthCancelCallback)cancelCallback errorCallback:(TDSGlobalAuthErrorCallback)errorCallback;

+ (void)unbindPhone:(NSString *)phoneNumber;

- (void)sendSMS:(NSString *)phoneNumber verify:(NSString *_Nullable)verify fromSMS:(BOOL)fromSMS;

- (void)showPhoneLogin:(BOOL)downgrade;

@end

NS_ASSUME_NONNULL_END
