//
//  XDGErrorView.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/28.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN
typedef void(^XDGErrorViewTouchHandler)(void);

@interface XDGErrorView : UIView
@property(nonatomic)XDGErrorViewTouchHandler handler;
- (instancetype)initWithTitle:(NSString *)title forceShowIcon:(BOOL)forceShowIcon;
@end

NS_ASSUME_NONNULL_END
