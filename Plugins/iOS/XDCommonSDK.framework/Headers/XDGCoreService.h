//
//  XDGCoreService.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2020/11/23.
//

/// unity 桥文件

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGCoreService : NSObject
/// 获取版本号
+ (NSString *)getSDKVersionName;
/// 获取当前deviceid
+ (NSString *)getDid;
/// 获取当前用户位置
+ (void)getRegionInfo:(void (^)(NSString *result))callback;
/// 设置 debug 模式
+ (void)setDebugMode:(NSNumber *)setDebugMode;
/// 设置语言类型
+ (void)langType:(NSNumber *)langType;
/// 初始化SDK
+ (void)initSDK:(void (^)(NSString *result))callback;
/// 是否初始化
+ (NSNumber *)isInitialized;
/// 展示 loading
+ (void)showLoading;
/// 关闭 loading
+ (void)hideLoading;
/// 上报
+ (void)serverId:(NSString *)serverId roleId:(NSString *)roleId roleName:(NSString *)roleName;
/// 商店评分
+ (void)storeReview;

/// trackUser
+ (void)userId:(NSString *)userId;

/// set AF customerId
+ (void)customerId:(NSString *)customerId;

/// trackEvent
+ (void)eventName:(NSString *)eventName;

/// trackEvent with properties
+ (void)eventName:(NSString *)eventName properties:(NSString *)propertiesJsonStr;

/// 开启当前用户的推送
+ (void)enable:(NSNumber *)enable;

// trackRole
+ (void)serverId:(NSString *)serverId
          roleId:(NSString *)roleId
        roleName:(NSString *)roleName
           level:(NSNumber *)level;
///
+ (void)trackAchievement;

+ (void)eventCompletedTutorial;

+ (void)eventCreateRole;

#pragma mark - 推送
/// Open or close firebase push service for current user. Call after user logged in.
/// @param enable enable

/// The user need push service or not. Call after user logged in.
+ (NSNumber *)isCurrentUserPushServiceEnable;
/// 登陆成功埋点
+ (void)loginSuccessEvent;
/// 登录失败埋点
+ (void)loginFailMsg:(NSString *)loginFailMsg;

/// 设置SDK期望地区，DF, EN, KR等
+ (void)setTargetCountryOrRegion:(NSString *)setTargetCountryOrRegion;
/// 设置关闭回调
+ (void)setExitHandler:(void (^)(NSString *_Nonnull))callback;
/// 获取协议列表
+ (void)getAgreementList:(void (^)(NSString *result))callback;
/// 展示协议
+ (void)showDetailAgreement:(NSString *)url;

//不要协议弹框，默认有的
+ (void)disableAgreementUI;

//配置文件名，默认海外XDConfig, 国内 XDConfig-cn
+ (void)updateConfigFileName:(NSString *)updateConfigFileName;

@end

NS_ASSUME_NONNULL_END
