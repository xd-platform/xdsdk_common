
#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGEntryType.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGTrackerManager : NSObject
/// 跟踪用户
/// @param userId 用户唯一ID，非角色ID
+ (void)trackUser:(NSString *)userId;

//设置AF customer id
+ (void)setAFCustomerId:(NSString *)customerId;

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

/// 跟踪商店评分
+(void)eventRate;

@end

NS_ASSUME_NONNULL_END
