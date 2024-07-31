
#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGUser.h>
#import <XDCommonSDK/XDGEntryType.h>

NS_ASSUME_NONNULL_BEGIN
@interface XDGUserDataManager : NSObject

+ (void)setCurrentUser:(XDGUser *)user;
+ (XDGUser *)getCurrentUserData;

+ (void)updateLoginState:(BOOL)loggedIn;
+ (BOOL)isUserLoggedIn;
+ (BOOL)isUserTokenValid;

+ (LoginEntryType)getCurLoginType;

+ (void)userLoginSuccess;
+ (void)userLogout;
+ (BOOL)hasOldVersionAgreementSigned;
+ (void)saveCurrentUser;

/// 推送开关
+ (BOOL)needPushService;
+ (void)updatePushServiceStatu:(BOOL)statu;

@end

NS_ASSUME_NONNULL_END
