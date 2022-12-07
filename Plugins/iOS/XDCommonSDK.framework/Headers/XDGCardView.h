//
//  XDGCardView.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/21.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGCardView : UIView

@property (nonatomic, strong) UIView *containerView;

+ (XDGCardView *)createCardViewWithView:(UIView *)view;

- (void)addContentView:(UIView *)view;
@end

NS_ASSUME_NONNULL_END
