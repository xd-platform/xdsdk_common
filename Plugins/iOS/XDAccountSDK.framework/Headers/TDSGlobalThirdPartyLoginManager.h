//
//  TDSGlobalThirdPartyLoginManager.h
//  
//
//  Created by JiangJiahao on 2020/8/19.
//  第三方登录管理

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGEntryType.h>
#import <UIKit/UIKit.h>

@class XDGUser;
@class TDSGlobalViewControllerBase;
NS_ASSUME_NONNULL_BEGIN

extern NSString *const TDSG_LOGIN_ERROR_DOMAIN;

typedef void (^XDGLoginManagerRequestCallback)( XDGUser * _Nullable result,NSError * _Nullable error);

typedef void(^XDGLoginSyncCallback)(NSDictionary * _Nullable result,NSError * _Nullable error);

typedef NS_ENUM(NSInteger,TDSLoginErrorCode) {
    TDSLoginErrorCodeFail           = 0x1002,                       // fail
    TDSLoginErrorCodeCancel         = 0x1003,                       // user cancel
    TDSLoginErrorCodeNetworkOffline = 0x1004,                       // no network
    TDSLoginErrorCodeTokenExpired   = 0x1005,                       // fail
    TDSLoginErrorCodeNeedPayback    = 412,                          // need payback
};

@interface TDSGlobalThirdPartyLoginManager : NSObject

+ (void)loginByType:(LoginEntryType)loginType fromViewController:(nullable UIViewController *)viewController handler:(XDGLoginManagerRequestCallback)handler;

+ (void)failHandler:(NSError *)error handler:(XDGLoginManagerRequestCallback)handler;

+ (void)successHandler:(NSDictionary *)dataDic handler:(XDGLoginManagerRequestCallback)handler loginType:(LoginEntryType)loginType;

+ (void)getUserProfile:(XDGLoginManagerRequestCallback)handler;

+ (void)logout;

+ (void)syncNetConnect:(BOOL)connect syncCallback:(XDGLoginSyncCallback)handle;

@end

NS_ASSUME_NONNULL_END
