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
+ (NSString *)getSDKVersionName;
+ (NSString *)getDid;

+ (void)enableIDFA:(NSNumber *)enableIDFA;

// 获取当前用户位置
+ (void)getRegionInfo:(void (^)(NSString *result))callback;

+ (void)setDebugMode:(NSNumber *)setDebugMode;

+ (void)langType:(NSNumber *)langType;

+ (void)initSDK:(void (^)(NSString *result))callback;

+ (NSNumber *)isInitialized;

+ (void)showLoading;

+ (void)hideLoading;

// report
+ (void)serverId:(NSString *)serverId roleId:(NSString *)roleId roleName:(NSString *)roleName;

+ (void)storeReview;

#pragma mark - 分享
//// shareWithImage
//+ (void)shareFlavors:(NSNumber *)type imagePath:(NSString *)imagePath bridgeCallback:(void (^)(NSString *result))callback;
//
////shareWithUriMessage
//+ (void)shareFlavors:(NSNumber *)type uri:(NSString *)uri message:(NSString *)message bridgeCallback:(void (^)(NSString *result))callback;


//trackUser
+ (void)userId:(NSString *)userId;

//set AF customerId
+ (void)customerId:(NSString *)customerId;

//trackEvent
+ (void)eventName:(NSString *)eventName;

//setCurrentUserPushServiceEnable
+ (void)enable:(NSNumber *)enable;


// trackRole
+ (void)serverId:(NSString *)serverId
          roleId:(NSString *)roleId
        roleName:(NSString *)roleName
           level:(NSNumber *)level;

+ (void)trackAchievement;

+ (void)eventCompletedTutorial;

+ (void)eventCreateRole;

#pragma mark - 推送
/// Open or close firebase push service for current user. Call after user logged in.
/// @param enable enable

/// The user need push service or not. Call after user logged in.
+ (NSNumber *)isCurrentUserPushServiceEnable;

+ (void)loginSuccessEvent;

+ (void)loginFailMsg:(NSString *)loginFailMsg;

+ (NSString *)getImagePath;


/// 设置SDK期望地区，DF, EN, KR等
+ (void)setTargetCountryOrRegion:(NSString *)setTargetCountryOrRegion;

//不要协议弹框，默认有的
+ (void)disableAgreementUI;

//配置文件名，默认海外XDConfig, 国内 XDConfig-cn
+ (void)updateConfigFileName:(NSString *)updateConfigFileName;

//设置测服环境
+ (void)setDevelopUrl;

//清除缓存
+ (void)clearAllUserDefaultsData;


@end

NS_ASSUME_NONNULL_END
