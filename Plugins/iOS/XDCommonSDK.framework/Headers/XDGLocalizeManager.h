//
//  XDGLocalizeManager.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/4/23.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, XDGLanguageLocale) {
    XDGLanguageLocaleDefault = -1,          // 默认
    XDGLanguageLocaleSimplifiedChinese = 0, // 简体中文
    XDGLanguageLocaleTraditionalChinese,    // 繁体中文
    XDGLanguageLocaleEnglish,               // 英文
    XDGLanguageLocaleThai,                  // 泰文
    XDGLanguageLocaleBahasa,                // 印尼文
    XDGLanguageLocaleKorean,                // 韩文
    XDGLanguageLocaleJapanese,              // 日文
    XDGLanguageLocaleGerman,                // 德语
    XDGLanguageLocaleFrench,                // 法语
    XDGLanguageLocalePortuguese,            // 葡萄牙语
    XDGLanguageLocaleSpanish,               // 西班牙语
    XDGLanguageLocaleTurkish,               // 土耳其语
    XDGLanguageLocaleRussian,               // 俄语
    XDGLanguageLocaleVietnamese,            // 越南语
};

@interface XDGLocalizeManager : NSObject

+ (void)setCurrentLanguage:(XDGLanguageLocale)lang;

+ (XDGLanguageLocale)currentLanguage;

+ (NSString *)getCurrentLocalizedRegionString;

+ (NSString *)getLocalizedRegionString:(XDGLanguageLocale)langType;

+ (NSString *)getReportLanguageTypeString;

+ (NSString *)getLanguageTypeString;
@end

NS_ASSUME_NONNULL_END
