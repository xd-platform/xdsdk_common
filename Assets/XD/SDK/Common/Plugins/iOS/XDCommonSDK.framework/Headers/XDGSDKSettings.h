

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN
typedef NS_ENUM(NSInteger,XDGLanguageLocale) {
    XDGLanguageLocaleSimplifiedChinese = 0,                 // 简体中文
    XDGLanguageLocaleTraditionalChinese,                    // 繁体中文
    XDGLanguageLocaleEnglish,                               // 英文
    XDGLanguageLocaleThai,                                  // 泰文
    XDGLanguageLocaleBahasa,                                // 印尼文
    XDGLanguageLocaleKorean,                                // 韩文
    XDGLanguageLocaleJapanese,                              // 日文
    XDGLanguageLocaleGerman,                                // 德语
    XDGLanguageLocaleFrench,                                // 法语
    XDGLanguageLocalePortuguese,                            // 葡萄牙语
    XDGLanguageLocaleSpanish,                               // 西班牙语
    XDGLanguageLocaleTurkish,                               // 土耳其语
    XDGLanguageLocaleRussian,                               // 俄语
};

/// SDK Settings
@interface XDGSDKSettings : NSObject

/// 设置调试模式，debug 会输出SDK日志
/// @param debug 是否 debug 模式。默认 NO
+ (void)setDebugMode:(BOOL)debug;

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
@end

NS_ASSUME_NONNULL_END
