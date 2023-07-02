

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGLocalizeManager.h>

NS_ASSUME_NONNULL_BEGIN

/// SDK Settings
@interface XDGSDKSettings : NSObject

/// 设置调试模式，debug 会输出SDK日志
/// @param debug 是否 debug 模式。默认 NO
+ (void)setDebugMode:(BOOL)debug;

/// 获取是否是调试模式
+ (bool)isDebug;

/// 设置SDK显示语言
/// @param locale 语言，在 XDGLanguageLocale 枚举中查看
+ (void)setLanguage:(XDGLanguageLocale)locale;

/// 设置SDK期望地区
/// @param targetCountryOrRegion 期望地区
+ (void)setTargetCountryOrRegion:(NSString *)targetCountryOrRegion;

/// 设置是否打开SDK的协议窗口
/// @param enable 是否打开，默认YES
+ (void)setAgreementUIEnable:(BOOL)enable;

/// 更新初始化文件名
/// @param fileName 初始化文件名
+ (void)updateConfigFileName:(NSString *)fileName;

/// 设置退出游戏的回调，游戏可以收到回调后自行处理
/// @param handler 回调
+ (void)setExitHandler:(void (^)(void))handler;

@end

NS_ASSUME_NONNULL_END
