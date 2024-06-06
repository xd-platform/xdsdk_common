
///  工具类 公用方法

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

/**
 Dispatches the specified block on the main thread.
 @param block the block to dispatch
 */
extern void tdsg_dispatch_on_main_thread(dispatch_block_t block);

/**
 Dispatches the specified block on the default thread.
 @param block the block to dispatch
 */
extern void tdsg_dispatch_on_default_thread(dispatch_block_t block);

@interface TDSGlobalCommonUtility : NSObject

/// 获取自定义错误
/// @param domain 错误domain
/// @param errorCode 错误码
/// @param errorDesc 错误描述
+ (NSError *)customError:(NSString * _Nullable)domain code:(NSInteger)errorCode desc:(NSString *)errorDesc;

+ (NSError *)customError:(NSString * _Nullable)domain code:(NSInteger)errorCode desc:(NSString *)errorDesc extraData:(NSDictionary *)extraMap;

@end

NS_ASSUME_NONNULL_END
