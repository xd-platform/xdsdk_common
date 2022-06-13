
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

//typedef NS_ENUM(NSInteger,XDGLoginEntryType) {
//    XDGLoginEntryTypeDefault = 0,
//    XDGLoginEntryTypeTapTap,
//    XDGLoginEntryTypeApple,
//    XDGLoginEntryTypeGoogle,
//    XDGLoginEntryTypeFacebook,
//    XDGLoginEntryTypeLine,
//    XDGLoginEntryTypeTwitter,
//    XDGLoginEntryTypeGuest
//};


typedef NSString * LoginEntryType NS_STRING_ENUM;

FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeDefault;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeTapTap;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeApple;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeGoogle;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeFacebook;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeLine;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeTwitter;
FOUNDATION_EXPORT LoginEntryType const LoginEntryTypeGuest;




/** Enum of entryType in string */
extern NSString *const TDSGLOBAL_TAPTAP_ENTRY;              // @"TAPTAP"
extern NSString *const TDSGLOBAL_GOOGLE_ENTRY;              // @"GOOGLE"
extern NSString *const TDSGLOBAL_FACEBOOK_ENTRY;            // @"FACEBOOK"
extern NSString *const TDSGLOBAL_APPLE_ENTRY;               // @"APPLE"
extern NSString *const TDSGLOBAL_LINE_ENTRY;                // @"LINE"
extern NSString *const TDSGLOBAL_TWITTER_ENTRY;             // @"TWITTER"
extern NSString *const TDSGLOBAL_GUEST_ENTRY;               // @"GUEST"

/// Login or bind entry type
@interface XDGEntryType : NSObject


+ (NSString *)accountstringByEntryType:(LoginEntryType)entryType;
//+ (NSString *)accountPurestringByEntryType:(XDGLoginEntryType)entryType;
//
+ (NSString *)logoImageByEntryType:(LoginEntryType)entryType;
+ (NSString *)buttonImageByEntryType:(LoginEntryType)entryType;
//
///// convert integer to string
///// @param entryType entryType in integer
//+ (NSString *)stringByEntryType:(XDGLoginEntryType)entryType;

/// convert string to integer enum
/// @param entryTypeInString entryType in string
+ (LoginEntryType)entryTypeByString:(NSString *)entryTypeInString;

+ (NSNumber *)numberByEntryType:(LoginEntryType)entryType;


@end

NS_ASSUME_NONNULL_END
