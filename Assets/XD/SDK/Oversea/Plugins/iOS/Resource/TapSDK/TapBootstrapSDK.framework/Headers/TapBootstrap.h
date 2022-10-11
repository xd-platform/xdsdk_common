//
//  TapBootstrap.h
//  TapBootstrapSDK
//
//  Created by Bottle K on 2021/2/18.
//

#import <Foundation/Foundation.h>
#import <TapCommonSDK/TapCommonSDK.h>

#define TapSDK                @"TapSDK"
#define TapSDK_VERSION_NUMBER @"31300001"
#define TapSDK_VERSION        @"3.13.0"

NS_ASSUME_NONNULL_BEGIN

@interface TapBootstrap : NSObject

+ (instancetype)new NS_UNAVAILABLE;
- (instancetype)init NS_UNAVAILABLE;

/// 初始化
/// @param config 配置项
+ (void)initWithConfig:(TapConfig *)config;

/// 当前是否初始化
+ (BOOL)isInitialized;

/// 设定游戏倾向语言
/// @param lang TapLanguageType 0-自动 1-简中 2-英文
+ (void)setPreferredLanguage:(TapLanguageType)lang;

/// 处理系统协议
/// @param url url
+ (BOOL)handleOpenURL:(NSURL *)url __attribute__((deprecated("Please use [TDSHandleUrl handleOpenURL:]")));
@end

NS_ASSUME_NONNULL_END
