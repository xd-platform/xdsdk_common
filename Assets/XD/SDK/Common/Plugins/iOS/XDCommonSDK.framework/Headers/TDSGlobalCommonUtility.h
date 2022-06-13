
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
+ (NSBundle *)bundle;

/// 计算字符串长度
/// @param string 字符串
/// @param fontSize 字体大小
/// @param height VIEW高度
+ (CGFloat)calculateRowWidth:(NSString *)string fontSize:(CGFloat)fontSize height:(CGFloat)height;

/// 获取自定义错误
/// @param domain 错误domain
/// @param errorCode 错误码
/// @param errorDesc 错误描述
+ (NSError *)customError:(NSString * _Nullable)domain code:(NSInteger)errorCode desc:(NSString *)errorDesc;

+ (void)addShadowToView:(UIView *)targetView innerShadowColor:
(int)innerShadowColor outerShadowColor:(int)outerShadowColor shadowOffset:(CGSize)shadowOffset;

/**
  Creates a timer using Grand Central Dispatch.
 @param interval The interval to fire the timer, in seconds.
 @param block The code block to execute when timer is fired.
 @return The dispatch handle.
 */
+ (dispatch_source_t)startGCDTimerWithInterval:(double)interval block:(dispatch_block_t)block;

/**
 Stop a timer that was started by startGCDTimerWithInterval.
 @param timer The dispatch handle received from startGCDTimerWithInterval.
 */
+ (void)stopGCDTimer:(dispatch_source_t)timer;

@end

NS_ASSUME_NONNULL_END
