
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN


@interface TDSGlobalLog : NSObject

+ (void)TDSGlobalLog:(NSString *)log;

+ (void)setDebugMode:(BOOL)debug;

+ (BOOL)isDebug;

@end

NS_ASSUME_NONNULL_END
