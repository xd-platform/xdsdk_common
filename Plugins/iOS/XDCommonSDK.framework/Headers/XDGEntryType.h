
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NSString * LoginEntryType NS_STRING_ENUM;

FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeDefault;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeTapTap;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeApple;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeGoogle;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeFacebook;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeLine;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeTwitter;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeGuest;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeSteam;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypePhone;

@interface XDGEntryType : NSObject

+ (NSString *)accountstringByEntryType:(LoginEntryType)entryType;

+ (NSString *)logoImageByEntryType:(LoginEntryType)entryType;

+ (NSString *)logoImagePressedByEntryType:(LoginEntryType)entryType;

+ (NSString *)buttonImageByEntryType:(LoginEntryType)entryType;

+ (NSString *)buttonImagePressedByEntryType:(LoginEntryType)entryType;

+ (LoginEntryType)entryTypeByString:(NSString *)entryTypeInString;

+ (LoginEntryType)entryTypeByInteger:(NSInteger )entryTypeInt;

+ (NSNumber *)numberByEntryType:(LoginEntryType)entryType;

@end

NS_ASSUME_NONNULL_END
