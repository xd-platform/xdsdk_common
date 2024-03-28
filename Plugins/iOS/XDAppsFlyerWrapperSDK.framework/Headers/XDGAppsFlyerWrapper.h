//
//  XDGAppsFlyerWrapper.h
//  XDAppsFlyerWrapperSDK
//
//  Created by Fattycat on 2024/3/5.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGAppsFlyerWrapper : NSObject
+ (void)registerUninstall:(NSData * _Nullable)deviceToken;
@end

NS_ASSUME_NONNULL_END
