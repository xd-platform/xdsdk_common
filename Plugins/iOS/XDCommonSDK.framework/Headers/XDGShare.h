//
//  XDGShare.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/12/13.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, XDGShareType) {
    XDGShareTypeFacebook = 0,
    XDGShareTypeLine,
    XDGShareTypeTwitter,
};

typedef NS_ENUM(NSInteger, XDGShareErrorCode) {
    XDGShareErrorCodeNone = 0,
    XDGShareErrorCodeFail = 0x5001,
    XDGShareErrorCodeNotSupport = 0x5002
};

typedef void (^XDGShareResultHandler)(NSError *_Nullable error, BOOL cancel);

NS_CLASS_DEPRECATED_IOS(10.0, 11.0, "Use XDGSharing instead")
@interface XDGShare : NSObject

/// 是否安装了分享平台
/// @param target 分享平台类型
+ (BOOL)isTargetInstalled:(XDGShareType)target DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing isAppInstalled instead");

// 分享图片
/// @param type 分享平台类型
/// @param image 待分享图片
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type image:(UIImage *)image completeHandler:(XDGShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

/// 分享图片
/// @param type 分享平台类型
/// @param imagePath 待分享图片沙盒路径
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type imagePath:(NSString *)imagePath completeHandler:(XDGShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

/// 分享图片
/// @param type 分享平台类型
/// @param imageData 待分享图片的二进制数据
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type imageData:(NSData *)imageData completeHandler:(XDGShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

/// 分享URL
/// @param type 分享平台类型
/// @param url 待分享URL
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type url:(NSString *)url completeHandler:(XDGShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

/// 分享URL
/// @param type 分享平台类型
/// @param url 待分享URL
/// @param message 文字说明
/// @param completeHandler 分享结果回调
+ (void)shareWithType:(XDGShareType)type url:(NSString *)url message:(nullable NSString *)message completeHandler:(XDGShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

+ (NSData *)compressImage:(UIImage *)image toByte:(NSUInteger)maxLength;

@end

NS_ASSUME_NONNULL_END
