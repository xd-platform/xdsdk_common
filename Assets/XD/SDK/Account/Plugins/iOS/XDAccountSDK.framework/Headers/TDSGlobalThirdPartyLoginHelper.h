//
//  TDSGlobalThirdPartyLoginHelper.h
//  XDGAccountSDK
//
//  Created by JiangJiahao on 2021/3/4.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <XDCommonSDK/XDGEntryType.h>

NS_ASSUME_NONNULL_BEGIN

typedef void(^TDSGlobalAuthSuccessCalback)(LoginEntryType authType,NSDictionary *result);
typedef void(^TDSGlobalAuthCancelCallback)(void);
typedef void(^TDSGlobalAuthWatingCallback)(void);
typedef void(^TDSGlobalAuthErrorCallback)(NSError *_Nullable error);

typedef NS_ENUM(NSInteger,TDSGlobalAuthErrorCode) {
    TDSGlobalAuthErrorCodeNormalFail           = 0x3001,                       // fail
    TDSGlobalAuthErrorCodeNetworkOffline       = 0x3002,                       // no network
    TDSGlobalAuthErrorCodeUserCancel           = 0x3003,                       // cancel
    TDSGlobalAuthErrorCodeNotSupport           = 0x3004,                       // not support

};

//  后台登录方式枚举
typedef NS_ENUM(NSInteger,XDGLoginInfoType) {
    XDGLoginInfoTypeGuest = 0,
    XDGLoginInfoTypeWeChat,
    XDGLoginInfoTypeApple,
    XDGLoginInfoTypeGoogle,
    XDGLoginInfoTypeFacebook,
    XDGLoginInfoTypeTapTap,
    XDGLoginInfoTypeLine,
    XDGLoginInfoTypeTwitter
    
};

@interface TDSGlobalThirdPartyLoginHelper : NSObject

+ (void)authByType:(LoginEntryType)authType
fromViewController:(nullable UIViewController *)viewController
   successCallback:(TDSGlobalAuthSuccessCalback)successCallback
    cancelCallback:(TDSGlobalAuthCancelCallback)cancelCallback
     errorCallback:(TDSGlobalAuthErrorCallback)errorCallback;

+ (void)authByType:(LoginEntryType)authType
fromViewController:(nullable UIViewController *)viewController
   successCallback:(TDSGlobalAuthSuccessCalback)successCallback
    cancelCallback:(TDSGlobalAuthCancelCallback)cancelCallback
    watingCallback:(nullable TDSGlobalAuthWatingCallback)watingCallback
     errorCallback:(TDSGlobalAuthErrorCallback)errorCallback;

+ (void)logout;

+ (void)clearInstance;

@end

NS_ASSUME_NONNULL_END
