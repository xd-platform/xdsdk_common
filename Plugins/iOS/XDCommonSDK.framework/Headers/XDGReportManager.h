
// 客服

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGReportManager : NSObject

/// 当前登录用户，打开客服中心
/// @param serverId 玩家服务器ID
/// @param roleId 角色ID
/// @param roleName 角色名
+ (void)reportWithServerId:(NSString * _Nullable)serverId roleId:(NSString * _Nullable)roleId roleName:(NSString * _Nullable)roleName;

/// 打开客服中心
/// @param userId 用户在TDSGlobal平台ID
/// @param serverId 服务器ID
/// @param roleId 角色ID
/// @param roleName 角色名
+ (void)reportWithUserId:(NSString *)userId serverId:(NSString * _Nullable)serverId roleId:(NSString * _Nullable)roleId roleName:(NSString * _Nullable)roleName;

@end

NS_ASSUME_NONNULL_END
