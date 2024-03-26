//
//  XDGBaseDialog.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/2/11.
//

#import <UIKit/UIKit.h>
#import <XDCommonSDK/XDGCardView.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGBaseDialog : UIView
@property (nonatomic, strong) XDGCardView *dialogCard;
@property (nonatomic, strong) NSLayoutConstraint *dialogCenterYContraint;

- (void)showInWindow;
@end

NS_ASSUME_NONNULL_END
