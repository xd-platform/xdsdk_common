//
//  XDGConstants.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/18.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, XDGNumberAuthCode) {
    XDGNumberAuthCodeNotSupport = 0, // 不支持该登录方式
    XDGNumberAuthCodeSuccess,        // 操作成功
    XDGNumberAuthCodeCancelled,      // 操作被取消
    XDGNumberAuthCodeFailure,        // 操作失败
    XDGNumberAuthCodeSwitchToPhone,  // 切换到电话
    XDGNumberAuthCodeSwitchToEmail,  // 切换到电子邮件
};

NS_ASSUME_NONNULL_END
