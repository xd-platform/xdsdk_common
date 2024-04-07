//
//  XDGXHSShareParam.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/29.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGShareParamBase.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGXHSShareParam : XDGShareParamBase

/// 分享的文案
@property (nonatomic, strong, nullable) NSString *contentText;

/// 分享的图片
@property (nonatomic, strong, nullable) UIImage *image;

/// 分享的图片的本地沙盒路径
@property (nonatomic, strong, nullable) NSString *imageUrl;

/// 分享的图片 Data
@property (nonatomic, strong, nullable) NSData *imageData;

/// 分享的视频的 localIdentifier
@property (nonatomic, strong, nullable) NSString *videoId;

/// 分享的视频 url
@property (nonatomic, strong, nullable) NSString *videoUrl;

/// 分享的标题
@property (nonatomic, strong, nullable) NSString *title;

/// 检查参数是否可用
- (NSError *)checkValid;
@end

NS_ASSUME_NONNULL_END
