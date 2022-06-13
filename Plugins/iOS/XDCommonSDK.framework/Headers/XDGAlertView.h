//
//  XDGAlertView.h
//  TDSGlobalSDKCommonKit
//
//  Created by jessy on 2021/11/8.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
@class XDGAlertView;
    
NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSUInteger,XDGAlertViewType) {
    XDGAlertViewTypeAlert = 0,                      /// 警告类型
    XDGAlertViewTypeActionSheet                     /// 菜单类型
};
typedef void(^XDGAlertViewCancelClickCallback)(void);

typedef void(^XDGAlertViewClickCallback)(NSString *title,NSInteger buttonIndex);

typedef void(^XDGAlertViewDismissCallback)(void);

/// 两种方式回调,都设置会都生效
@protocol XDGAlertViewDelegate <NSObject>

@optional
- (void)clickAlertView:(XDGAlertView *)alertView buttonTitle:(NSString *)title index:(NSInteger)buttonIndex;
- (void)cancelAlertView:(XDGAlertView *)alertView;

@end

@interface XDGAlertView : NSObject
/// 点击取消按钮时回调
@property (nonatomic,copy) XDGAlertViewCancelClickCallback cancelCallback;
/// 点击其他按钮时回调
@property (nonatomic,copy) XDGAlertViewClickCallback callback;
/// 代理回调
@property (nonatomic,weak) id<XDGAlertViewDelegate> delegate;
/// 当样式为菜单时，ipad需要
@property (nonatomic,weak) UIView *sourceView;
/// 标记
@property (nonatomic) NSInteger tag;

+ (XDGAlertView *)alertWithTitle:(nullable NSString *)title message:(NSString *)message cancelTitle:(nullable NSString *)cancelTitle otherButton:(nullable NSString *)otherButton, ... NS_REQUIRES_NIL_TERMINATION;

/// 创建一个弹窗
/// @param title 弹窗标题
/// @param message 弹窗说明
/// @param type 弹窗类型，警告和菜单
/// @param cancelTitle 取消按钮标题
/// @param otherButton 其他按钮标题
+ (XDGAlertView *)alertWithTitle:(nullable NSString *)title message:(NSString *)message alertType:(XDGAlertViewType)type cancelTitle:(nullable NSString *)cancelTitle otherButton:(nullable NSString *)otherButton, ... NS_REQUIRES_NIL_TERMINATION;

/// 展示弹窗
- (void)show;

/// 定时消失
/// @param second 弹窗展示时间，秒
/// @param callback 消失时回调
- (void)dismissAfterTime:(NSTimeInterval)second callback:(XDGAlertViewDismissCallback)callback;

@end

NS_ASSUME_NONNULL_END
