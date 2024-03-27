//
//  XDGHttpUtils.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/4/10.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGHttpUtils : NSObject
+ (NSString *)connectUrl:(NSString *)url params:(NSDictionary *)params;
@end

NS_ASSUME_NONNULL_END
