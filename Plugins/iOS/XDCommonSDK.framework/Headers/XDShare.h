//
//  XDShare.h
//  XDCommonSDK
//
//  Created by 黄驿峰 on 2023/2/10.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, XDShareTarget) {
    XDShareTargetWeChat = 0,
    XDShareTargetQQ,
    XDShareTargetWeibo,
};

typedef NS_ENUM(NSInteger, XDShareSceneType) {
    XDShareSceneTypeSession = 0,    // 聊天会话
    XDShareSceneTypeTimeline,       // 朋友圈、QQ空间等
};

typedef NS_ENUM(NSInteger, XDShareErrorCode) {
    XDShareErrorCodeInvalidParams           = -1,
    XDShareErrorCodeNotInit                 = -2,
    XDShareErrorCodeTargetNotInstalled      = -3,
    XDShareErrorCodeOther                   = -4,
};

// 当cancel为NO，error为nil时，表示分享成功
typedef void (^XDShareResultHandler)(NSError *_Nullable error, BOOL cancel);

/**
 * WebPage 实体信息，
 * 包括标题、摘要、缩略图、点击后跳转 URL
 */
@interface XDShareWebPage : NSObject

/// 标题，必选
@property (nonatomic, strong) NSString *title;
/// 摘要，可选
@property (nonatomic, strong, nullable) NSString *summary;
/// 缩略图路径，可选
@property (nonatomic, strong, nullable) NSString *thumbUri;
/// 缩略图数据，可选，如果和thumbUri同时存在，优先取thumbData
@property (nonatomic, strong, nullable) NSData *thumbData;
/// 跳转 URL。必选
@property (nonatomic, strong) NSString *url;

@end

NS_CLASS_DEPRECATED_IOS(10.0, 11.0, "Use XDGSharing instead")
@interface XDShare : NSObject

+ (BOOL)isTargetInstalled:(XDShareTarget)target DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing isAppInstalled instead");

+ (void)shareWithTarget:(XDShareTarget)type sceneType:(XDShareSceneType)sceneType text:(NSString *)text completeHandler:(XDShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

+ (void)shareWithTarget:(XDShareTarget)type sceneType:(XDShareSceneType)sceneType imageUri:(NSString *)imageUri completeHandler:(XDShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

+ (void)shareWithTarget:(XDShareTarget)type sceneType:(XDShareSceneType)sceneType imageData:(NSData *)imageData completeHandler:(XDShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

+ (void)shareWithTarget:(XDShareTarget)type sceneType:(XDShareSceneType)sceneType webPage:(XDShareWebPage *)webPage completeHandler:(XDShareResultHandler)completeHandler DEPRECATED_MSG_ATTRIBUTE("Use XDGSharing instead");

+ (NSData *)GetImageDataFromData:(NSData *)imageData MaxSize:(NSInteger)size;
@end

NS_ASSUME_NONNULL_END
