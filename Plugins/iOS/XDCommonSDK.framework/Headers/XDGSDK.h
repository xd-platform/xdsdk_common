
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <XDCommonSDK/XDGConfig.h>
#import <XDCommonSDK/XDConfigManager.h>
#import <XDCommonSDK/XDGEntryType.h>
#import <XDCommonSDK/XDGRegionInfo.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGSDK : NSObject
/// 获取当前 SDK 版本
+ (NSString *)getSDKVersionName;

/// 初始化
/// @param handler 结果回调
+ (void)initSDK:(XDGInitCallback)handler;

/// 初始化
/// @param config XDGSDK config
/// @param handler 结果回调
+ (void)initSDKWithConfig:(XDGConfig*)config handler:(XDGInitCallback)handler;

/// 是否已经初始化
+ (BOOL)isInitialized;

+ (NSArray<XDGAgreement *> *)getAgreementList;

+ (void)showDetailAgreement:(NSString *)url;

/// 当前登录用户，打开客服中心
/// @param serverId 服务器 ID，可为空
/// @param roleId 角色 ID，可为空
/// @param roleName 角色名，可为空
+ (void)reportWithServerId:(NSString *)serverId roleId:(NSString *)roleId roleName:(NSString *)roleName;

/// 调起或跳转商店评分
+ (void)storeReview;

// 获取当前用户位置
+ (void)getRegionInfo:(void (^)(XDGRegionInfo *result))completeHandler;

#pragma mark -- traker
/// 跟踪用户
+ (void)trackUser;

/// @param userId 用户唯一ID，非角色ID
+ (void)trackUser:(NSString *)userId;

+ (void)trackUser:(NSString *)userId
        loginType:(LoginEntryType)loginType
       properties:(nullable NSDictionary *)properties;

/// 跟踪角色
/// @param roleId 角色ID
/// @param roleName 角色名
/// @param serverId 服务器ID
/// @param level 角色等级
+ (void)trackRoleWithRoleId:(NSString *)roleId
                   roleName:(NSString *)roleName
                   serverId:(NSString *)serverId
                      level:(NSInteger)level;

/// 跟踪自定义事件
/// @param eventName 事件名
+ (void)trackEvent:(NSString *)eventName;

/// 跟踪自定义事件
/// @param eventName 事件名
/// @param properties 属性
+ (void)trackEvent:(NSString *)eventName properties:(nullable NSDictionary *)properties;


/// 跟踪完成成就
+ (void)trackAchievement;

/// 跟踪完成新手引导接口
+ (void)eventCompletedTutorial;

/// 跟踪完成创角
+ (void)eventCreateRole;

#pragma  mark -- 推送
/// Open or close firebase push service for current user. Call after user logged in.
/// @param enable enable
+ (void)setCurrentUserPushServiceEnable:(BOOL)enable;

/// The user need push service or not. Call after user logged in.
+ (BOOL)isCurrentUserPushServiceEnable;

#pragma mark - Applicaiton Delegate
+ (void)application:(UIApplication *)application
didFinishLaunchingWithOptions:(nullable NSDictionary<UIApplicationLaunchOptionsKey, id> *)launchOptions;

+ (void)application:(UIApplication *)application openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options;

+ (void)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;

+ (void)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray<id<UIUserActivityRestoring>> * _Nullable))restorationHandler;

+ (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler;

+ (void)scene:(UIScene *)scene openURLContexts:(NSSet<UIOpenURLContext *> *)URLContexts API_AVAILABLE(ios(13.0));

+ (void)scene:(UIScene *)scene continueUserActivity:(NSUserActivity *)userActivity  API_AVAILABLE(ios(13.0));
@end

NS_ASSUME_NONNULL_END
