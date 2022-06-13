
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef void(^TDSTitleCloseCallback)(void);
typedef void(^TDSTitleBackCallback)(void);


@interface TDSGlobalTitleView : UIView

@property (nonatomic) TDSTitleCloseCallback closeAction;
@property (nonatomic) TDSTitleBackCallback backAction;


+ (TDSGlobalTitleView *)createTitleView:(NSString *)title;

- (void)setBackButtonHidden:(BOOL)hidden;
- (void)setTitleViewTitle:(NSString *)title;


@end

NS_ASSUME_NONNULL_END
