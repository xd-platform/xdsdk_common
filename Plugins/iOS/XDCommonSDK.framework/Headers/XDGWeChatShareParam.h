//
//  XDGWeChatShareParam.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/26.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGShareParamBase.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGWeChatShareParam : XDGShareParamBase

/// 分享的目标平台
/// Session : 聊天会话
/// Timeline : 朋友圈
@property (nonatomic, assign) XDGShareSceneType sceneType;

/// 分享的文案
@property (nonatomic, strong, nullable) NSString *contentText;

/// 分享的图片
@property (nonatomic, strong, nullable) UIImage *image;

/// 分享的图片的本地沙盒路径
@property (nonatomic, strong, nullable) NSString *imageUrl;

/// 分享的图片 Data
@property (nonatomic, strong, nullable) NSData *imageData;

/// 分享的链接
@property (nonatomic, strong, nullable) NSString *linkUrl;

/// 分享的链接标题
@property (nonatomic, strong, nullable) NSString *linkTitle;

/// 分享的链接简介，可选
@property (nonatomic, strong, nullable) NSString *linkSummary;

/// 检查参数是否可用
- (NSError *)checkValid;
@end

NS_ASSUME_NONNULL_END
