//
//  XDGTapTapShareParam.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/6/11.
//

#import <XDCommonSDK/XDCommonSDK.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGTapTapShareParam : XDGShareParamBase

/// 游戏的 App ID
@property (nonatomic, strong) NSString *appId;

/// 游戏论坛的论坛标签 ID
@property (nonatomic, strong, nullable) NSString *groupLabelId;

/// 活动或者 hastTag ID, 逗号分割
@property (nonatomic, strong, nullable) NSString *hashtagIds;

/// 分享的文案
@property (nonatomic, strong, nullable) NSString *contentText;

/// 分享的图片
@property (nonatomic, strong, nullable) UIImage *image;

/// 分享的图片的本地沙盒路径
@property (nonatomic, strong, nullable) NSString *imageUrl;

/// 分享的图片 Data
@property (nonatomic, strong, nullable) NSData *imageData;

/// 分享的标题
@property (nonatomic, strong, nullable) NSString *title;

/// 无法唤起分享时配置的浏览器链接，不配置则为应用详情页
@property (nonatomic, strong, nullable) NSString *failUrl;

/// 检查参数是否可用
- (NSError *)checkValid;

@end

NS_ASSUME_NONNULL_END
