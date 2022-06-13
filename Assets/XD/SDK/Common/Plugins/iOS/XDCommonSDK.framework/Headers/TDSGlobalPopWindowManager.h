
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalPopWindowManager : NSObject
+ (UIViewController *)getTopViewController;
// 弹窗界面
+ (void)show:(UIViewController *)targetController;
// 全屏界面,浏览器等
+ (void)showFullScreenController:(UIViewController *)targetController;
+ (void)showFullScreenController:(UIViewController *)targetController animate:(BOOL)animate;

+ (void)dismiss;
+ (void)dismiss:(BOOL)animate;

/// 全局提示
/// @param msg 提示内容
+ (void)showHud:(NSString *)msg;
+ (void)showAutoHud:(NSString *)text;
+ (void)showAutoHud:(NSString *)text completion:(nullable void (^)(void))completion;
+ (void)showAutoHud:(NSString *)text duration:(CGFloat)duration completion:(nullable void (^)(void))completion;
/// 全局加载中
+ (void)showLoading;
+ (void)removeLoading;

#pragma mark - window
+ (UIWindow *)generateWindow;
+ (void)destroyedWindow:(UIWindow *)window;
@end

NS_ASSUME_NONNULL_END
