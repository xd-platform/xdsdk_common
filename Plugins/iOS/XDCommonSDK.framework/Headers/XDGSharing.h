//
//  XDGSharing.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/25.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGShareParamBase.h>
#import <XDCommonSDK/XDGQQShareParam.h>
#import <XDCommonSDK/XDGWeChatShareParam.h>
#import <XDCommonSDK/XDGWeiboShareParam.h>
#import <XDCommonSDK/XDGXHSShareParam.h>
#import <XDCommonSDK/XDGDouYinShareParam.h>
#import <XDCommonSDK/XDGFacebookShareParam.h>
#import <XDCommonSDK/XDGTwitterShareParam.h>
#import <XDCommonSDK/XDGLineShareParam.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGSharing : NSObject

+ (void)shareWithParam:(XDGShareParamBase *)platformParam completionHandler:(XDGShareHandler)handler;

+ (BOOL)isAppInstalled:(XDGSharePlatformType)platform;

@end

NS_ASSUME_NONNULL_END
