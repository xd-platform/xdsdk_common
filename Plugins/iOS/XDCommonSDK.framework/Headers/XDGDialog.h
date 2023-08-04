//
//  XDGDialog.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/24.
//

#import <UIKit/UIKit.h>

#import <XDCommonSDK/XDGCardView.h>
NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger, XDGDialogState) {
    XDGDialogClickLeft,
    XDGDialogClickRight,
    XDGDialogClickClose,
};
typedef void (^XDGDialogHandler)(XDGDialogState state);

@interface XDGDialog : UIView
@property (nonatomic, strong) XDGCardView *dialogCard;
@property (nonatomic, strong) NSLayoutConstraint *dialogCenterYContraint;
@property (nonatomic, copy) XDGDialogHandler dialogHandler;

+ (XDGDialog *)dialogWithTitle:(NSString *)title
                       content:(NSString *)content
                     rightText:(NSString *)rightText
                      leftText:(NSString *_Nullable)leftText;

+ (XDGDialog *)dialogWithTitle:(NSString *)title
             attributedContent:(NSAttributedString *)content
                     rightText:(NSString *)rightText
                      leftText:(NSString *_Nullable)leftText;

- (void)setTitle:(NSString *)title
         content:(NSString *)content
       rightText:(NSString *)rightText
        leftText:(NSString *_Nullable)leftText;

- (void)showInWindow;
@end

NS_ASSUME_NONNULL_END
