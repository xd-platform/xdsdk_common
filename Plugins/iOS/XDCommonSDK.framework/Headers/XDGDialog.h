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
typedef void (^XDGDialogContentClickHandler)(void);

@interface XDGDialog : UIView
@property (nonatomic, strong) XDGCardView *dialogCard;
@property (nonatomic, strong) NSLayoutConstraint *dialogCenterYContraint;
@property (nonatomic, copy) XDGDialogHandler dialogHandler;
@property (nonatomic, copy) XDGDialogContentClickHandler dialogContentClickHandler;

+ (XDGDialog *)dialogWithTitle:(NSString *)title
                       content:(NSString *)content
                     rightText:(NSString *)rightText
                      leftText:(NSString *_Nullable)leftText;

+ (XDGDialog *)dialogWithTitle:(NSString *)title
             attributedContent:(NSAttributedString *)content
                     rightText:(NSString *)rightText
                      leftText:(NSString *_Nullable)leftText;

+ (XDGDialog *)dialogWithTitle:(NSString *)title
               contentRichText:(NSString *)content
                     rightText:(NSString *)rightText
                      leftText:(NSString *_Nullable)leftText;

- (void)setTitle:(NSString *)title
         content:(NSString *)content
       rightText:(NSString *)rightText
        leftText:(NSString *_Nullable)leftText;

- (void)setTitle:(NSString *_Nullable)title
         content:(NSString *)content
       rightText:(NSString *)rightText
        leftText:(NSString *_Nullable)leftText
 contentRichText:(BOOL)contentRichText
  useAsyncRender:(BOOL)useAsyncRender;

- (void)showInWindow;
@end

NS_ASSUME_NONNULL_END
