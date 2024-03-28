//
//  XDGConfig.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/5/16.
//

#import <Foundation/Foundation.h>
#import <TapCommonSDK/TapConfig.h>
#import <XDCommonSDK/XDAppleInfo.h>
#import <XDCommonSDK/XDFacebookInfo.h>
#import <XDCommonSDK/XDLineInfo.h>
#import <XDCommonSDK/XDTwitterInfo.h>
#import <XDCommonSDK/XDGoogleInfo.h>
#import <XDCommonSDK/XDFirebaseInfo.h>
#import <XDCommonSDK/XDAdjustInfo.h>
#import <XDCommonSDK/XDAppsFlyerInfo.h>
#import <XDCommonSDK/XDGGameBindEntry.h>
#import <XDCommonSDK/XDGAgreementConfig.h>
#import <XDCommonSDK/XDWeChatInfo.h>
#import <XDCommonSDK/XDWeiboInfo.h>
#import <XDCommonSDK/XDQQInfo.h>
#import <XDCommonSDK/XDDouYinInfo.h>
#import <XDCommonSDK/XDXHSInfo.h>

NS_ASSUME_NONNULL_BEGIN

typedef void (^XDGInitCallback)(BOOL success, NSString *_Nullable msg);

typedef NS_ENUM(NSInteger, XDSDKRegionType) {
    XDSDKRegionTypeCN = 0,
    XDSDKRegionTypeGlobal
};

/// XDGSDK Config
@interface XDGConfig : NSObject

#pragma mark 以下内容必须配置
/// 区域选择，可选 XDSDKRegionTypeCN、XDSDKRegionTypeGlobal
@property (nonatomic, assign) XDSDKRegionType regionType;
/// XDGSDK client id
@property (nonatomic, copy) NSString *clientId;
/// 是否开启 IDFA
@property (nonatomic, assign) BOOL idfaEnabled;
/// TapSDK 配置
@property (nonatomic, strong) TapConfig *tapConfig;
/// TapTap 授权权限
@property (nonatomic, strong) NSArray *tapLoginPermissions;
/// 游戏对外名称 显示在 Facebook 登录和客服页面中
@property (nonatomic, copy) NSString *gameName;

#pragma mark 以下内容按需配置
/// Apple 网页登录配置信息
@property (nonatomic, strong) XDAppleInfo *appleInfo;
/// Facebook 配置信息
@property (nonatomic, strong) XDFacebookInfo *facebookInfo;
/// Facebook  授权权限，如果使用 Facebook 登录，必须配置
@property (nonatomic, strong) NSArray *facebookLoginPersmissions;
/// Line 配置信息
@property (nonatomic, strong) XDLineInfo *lineInfo;
/// Twitter 配置信息
@property (nonatomic, strong) XDTwitterInfo *twitterInfo;
/// Google 配置信息
@property (nonatomic, strong) XDGoogleInfo *googleInfo;
/// Firebase 配置信息
@property (nonatomic, strong) XDFirebaseInfo *firebaseInfo;
/// Adjust 配置信息
@property (nonatomic, strong) XDAdjustInfo *adjustInfo;
/// AppsFlyer 配置信息
@property (nonatomic, strong) XDAppsFlyerInfo *appsflyerInfo;
/// 微信 配置信息
@property (nonatomic, strong) XDWeChatInfo *wechatInfo;
/// QQ 配置信息
@property (nonatomic, strong) XDQQInfo *qqInfo;
/// 微博 配置信息
@property (nonatomic, strong) XDWeiboInfo *weiboInfo;
/// 抖音 配置信息
@property (nonatomic, strong) XDDouYinInfo *douyinInfo;
/// 小红书 配置信息
@property (nonatomic, strong) XDXHSInfo *xhsInfo;
/// 统一登录窗口中的品牌向 logo 素材 URL 数组，需要3个，不配置的话默认https://res.xdcdn.net/TDS/Global/res/xd_logo.png
@property (nonatomic, copy) NSArray *logos;
/// 统一登录窗口中的登录入口选项
@property (nonatomic, strong) NSArray *loginEntries;
/// XDGSDK app id
@property (nonatomic, copy) NSString *appId;
/// 用户中心窗口中绑定入口选项
@property (nonatomic, strong) NSArray<XDGGameBindEntry *> *bindEntries;
/// 发行区域
@property (nonatomic, copy) NSString *region;
/// 客服链接，默认
@property (nonatomic, copy) NSString *reportUrl;
/// 注销账户链接，默认
@property (nonatomic, copy) NSString *logoutUrl;
/// 阿里云一键登录token
@property (nonatomic, copy) NSString *numberAuthToken;

- (BOOL)isCN;

- (NSMutableDictionary *)configDictionary;

@end

NS_ASSUME_NONNULL_END
