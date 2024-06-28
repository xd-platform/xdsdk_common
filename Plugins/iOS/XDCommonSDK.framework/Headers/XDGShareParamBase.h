//
//  XDGShareParamBase.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/25.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, XDGSharePlatformType) {
    XDGSharePlatformTypeNone     = -1, // 内部定义
    XDGSharePlatformTypeQQ       = 0,
    XDGSharePlatformTypeWeChat   = 1,
    XDGSharePlatformTypeWeibo    = 2,
    XDGSharePlatformTypeDouYin   = 3,
    XDGSharePlatformTypeXHS      = 4,
    XDGSharePlatformTypeFacebook = 5,
    XDGSharePlatformTypeTwitter  = 6,
    XDGSharePlatformTypeLine     = 7,
    XDGSharePlatformTypeTapTap   = 8
};

typedef NS_ENUM(NSInteger, XDGShareSceneType) {
    XDGShareSceneTypeSession  = 0,    // 聊天会话
    XDGShareSceneTypeTimeline = 1,       // 朋友圈、QQ空间等
};

typedef NS_ENUM(NSInteger, XDGShareResultCode) {
    XDGShareResultCodeSuccess             =  0,     /** 成功 */
    XDGShareResultCodeInvalidParam        = -1,     /** 参数错误 */
    XDGShareResultCodeNotInit             = -2,     /** 未初始化*/
    XDGShareResultCodeAppNotInstall       = -3,     /** 应用未安装*/
    XDGShareResultCodeCommon              = -4,     /** 通用错误 */
    XDGShareResultCodeUserCanceled        = -100,     /** 用户点击取消并返回 */
};

typedef void (^XDGShareHandler)(NSError *_Nullable error, BOOL cancel);

@interface XDGShareParamBase : NSObject

- (XDGSharePlatformType)getPlatform;

- (NSError *)checkValid;

- (NSDictionary *)generateDictionaryWithHandler:(XDGShareHandler)handler;

- (NSString *)removeFilePrefix:(NSString *)path;

+ (NSError *)errorWithCode:(XDGShareResultCode)code description:(NSString *_Nullable)description;

+ (void)shareResultWithHandler:(XDGShareHandler)handler shareSuccess:(bool)success isCancel:(bool)isCancel errorCode:(NSInteger)errorCode errorMsg:(NSString *_Nullable)errorMsg;

+ (void)saveImageToPhotoLibrary:(NSData *)imageData completion:(void (^)(NSString * _Nullable localIdentifier, NSError * _Nullable error))completion;

+ (void)saveVideoToPhotoLibrary:(NSString *)videoPath completion:(void (^)(NSString *_Nullable localIdentifier, NSError *_Nullable error))completion;
@end

NS_ASSUME_NONNULL_END
