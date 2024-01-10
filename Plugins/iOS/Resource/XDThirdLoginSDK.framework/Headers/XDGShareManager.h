//
//  XDGShareManager.h
//  XDThirdLoginSDK
//
//  Created by Fattycat on 2022/12/13.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <XDCommonSDK/XDGShare.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGShareManager : NSObject

/// 是否安装了分享平台
/// @param target 分享平台类型
+ (BOOL)isTargetInstalled:(XDGShareType)target;

/// 分享图片
/// @param type 分享平台类型
/// @param imageData 待分享图片数据
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type imageData:(NSData *)imageData completeHandler:(XDGShareResultHandler)completeHandler;

/// 分享图片
/// @param type 分享平台类型
/// @param image 待分享图片
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type image:(UIImage *)image completeHandler:(XDGShareResultHandler)completeHandler;

/// 分享图片
/// @param type 分享平台类型
/// @param imagePath 待分享图片沙盒路径
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type imagePath:(NSString *)imagePath completeHandler:(XDGShareResultHandler)completeHandler;

/// 分享URL
/// @param type 分享平台类型
/// @param url 待分享URL
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type url:(NSString *)url completeHandler:(XDGShareResultHandler)completeHandler;

/// 分享URL
/// @param type 分享平台类型
/// @param url 待分享URL
/// @param message 文字说明
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type url:(NSString *)url message:(nullable NSString *)message completeHandler:(XDGShareResultHandler)completeHandler;

@end

NS_ASSUME_NONNULL_END
