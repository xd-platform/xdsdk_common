
#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDCommonSDK.h>
#import <XDAccountSDK/XDGLoginEntriesConfig.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger,XDGUserStateChangeCode) {
    XDGUserStateChangeCodeLogout          = 0x9001,   // user logout
    XDGUserStateChangeCodeBindSuccess     = 0x1001,   // user bind success,msg = entry type in string,eg: @"TAPTAP"
    XDGUserStateChangeCodeUnBindSuccess   = 0x1002,   // user unbind success,msg = entry type in string
};

/**
  Describes the call back to the TDSGlobalLoginManager
 @param result the result of the login request
 @param error error, if any.
 */
typedef void (^XDGLoginManagerRequestCallback)(XDGUser * _Nullable result, NSError * _Nullable error);

typedef void(^XDGLoginSyncCallback)(NSDictionary * _Nullable result,NSError * _Nullable error);

/**
  Describes the call back of state of current user
 @param userStateChangeCode user state change type code.
 */
typedef void (^XDGUserStatusChangeCallback)(XDGUserStateChangeCode userStateChangeCode,NSString *_Nullable message);

@interface XDGAccount : NSObject

/// Get callback when user state changed
/// @param handler handler
+ (void)addUserStatusChangeCallback:(XDGUserStatusChangeCallback)handler;

/**
 You can customize login buttons in your own ways,and call this methods to login an user.
 Steps:
    1. use LoginEntryTypeDefault ,check if there was an user logged last time,you will get a result.
    2. if step 1 failed, show login buttons ,and call with corresponding type when user tapped.
 */
+ (void)loginByType:(LoginEntryType)loginType loginHandler:(XDGLoginManagerRequestCallback)handler;

/// Open login Window
/// @param config login entried config
/// @param handler login result handler
+ (void)loginWithConfig:(XDGLoginEntriesConfig *)config handler:(XDGLoginManagerRequestCallback)handler;

/// Logout current user
+ (void)logout;

/// Get current user
+ (void)getUser:(XDGLoginManagerRequestCallback)handler;

/// Open usercenter view
+ (void)openUserCenter;

/// Open accountCancellation
+ (void)accountCancellation;

+ (void)loginSync:(XDGLoginSyncCallback)handler;

@end

NS_ASSUME_NONNULL_END
