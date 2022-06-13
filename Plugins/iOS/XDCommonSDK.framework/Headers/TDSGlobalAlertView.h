//  弹窗提示

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef void(^AlertLeftCallback)(void);
typedef void(^AlertRightCallback)(void);

@interface TDSGlobalAlertView : UIView

@property (nonatomic) AlertLeftCallback rightCallback;
@property (nonatomic) AlertRightCallback leftCallback;
// 点击背景取消，默认否
@property (nonatomic) BOOL touchBgDismiss;

+ (TDSGlobalAlertView *)createAlertView:(NSString *)title
                                content:(NSString *)content
                            rightText:(NSString *)rightText
                             leftText:(NSString * _Nullable)leftText;

- (void)showInView:(UIView *)targetView;
- (void)dismiss;
@end

NS_ASSUME_NONNULL_END
