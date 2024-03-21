//
//  XDGLogger+Account.h
//  XDAccountSDK
//
//  Created by Fattycat on 2023/4/14.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGLogger.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGLogger (Account)

+ (void)accountLog:(NSString *)content;

+ (void)accountSecureLog:(NSString *)content;

@end

NS_ASSUME_NONNULL_END
