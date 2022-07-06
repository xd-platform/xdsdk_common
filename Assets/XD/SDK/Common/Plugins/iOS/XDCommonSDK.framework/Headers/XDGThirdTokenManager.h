//
//  XDGThirdTokenManager.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/6/23.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGEntryType.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGThirdTokenManager : NSObject
+ (BOOL)tokenActiveWithType:(LoginEntryType)type;
+ (void)updateThirdPlatformTokenWithCallback:(void(^)(BOOL result))handler;
+ (void)updateThirdPlatformToken;
@end

NS_ASSUME_NONNULL_END
