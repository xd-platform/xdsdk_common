//
//  XDGProgressHUD.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/28.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger, XDGProgressHUDMode) {
    XDGProgressHUDModeIndicator,
    XDGProgressHUDModeText
};

typedef void (^XDGProgressHUDCompletionBlock)(void);

@interface XDGProgressHUD : UIView
+ (instancetype)showHUDAddedTo:(UIView *)view animated:(BOOL)animated;

+ (instancetype)showHUDAddedTo:(UIView *)view text:(NSString *_Nullable)text animated:(BOOL)animated;

+ (instancetype)showHUDAddedTo:(UIView *)view text:(NSString *_Nullable)text animated:(BOOL)animated duration:(NSTimeInterval)duration;

+ (BOOL)hideHUDForView:(UIView *)view animated:(BOOL)animated;

+ (XDGProgressHUD *)HUDForView:(UIView *)view;
/**
 * XDGProgressHUD operation mode. The default is XDGProgressHUDModeIndicator.
 */
@property (nonatomic, assign) XDGProgressHUDMode mode;
/**
 * Called after the HUD is hidden.
 */
@property (copy, nullable) XDGProgressHUDCompletionBlock completionBlock;
/**
 * Grace period is the time (in seconds) that the invoked method may be run without
 * showing the HUD. If the task finishes before the grace time runs out, the HUD will
 * not be shown at all.
 * This may be used to prevent HUD display for very short tasks.
 * Defaults to 0 (no grace time).
 * @note The graceTime needs to be set before the hud is shown. You thus can't use `showHUDAddedTo:animated:`,
 * but instead need to alloc / init the HUD, configure the grace time and than show it manually.
 */
@property (assign, nonatomic) NSTimeInterval graceTime;

- (instancetype)initWithView:(UIView *)view;

- (void)showAnimated:(BOOL)animated;

- (void)hideAnimated:(BOOL)animated;

- (void)hideAnimated:(BOOL)animated afterDelay:(NSTimeInterval)delay;

- (void)setLabelText:(NSString *)text;
@end

NS_ASSUME_NONNULL_END
