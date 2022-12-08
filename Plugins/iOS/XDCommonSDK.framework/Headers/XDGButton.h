//
//  XDGButton.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/22.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger, XDGButtonType) {
    XDGButtonTypeMedium,
    XDGButtonTypeMediumColor,
    XDGButtonTypeLarge,
    XDGButtonTypeLargeColor,
    XDGButtonTypeBack,
    XDGButtonTypeClose,
    XDGButtonTypeCloseThin
};

@interface XDGButton : UIButton
- (XDGButton *)initWithType:(XDGButtonType)type;

- (void)setButtonEnabled:(BOOL)enabled;
@end

NS_ASSUME_NONNULL_END
