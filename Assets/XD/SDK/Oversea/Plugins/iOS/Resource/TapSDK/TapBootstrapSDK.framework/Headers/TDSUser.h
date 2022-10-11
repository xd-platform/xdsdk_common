//
//  TDSUser.h
//  TapBootstrapSDK
//
//  Created by pzheng on 2021/07/06.
//

#import <LeanCloudObjc/Foundation.h>
#import <LeanCloudObjc/Realtime.h>

NS_ASSUME_NONNULL_BEGIN

typedef NSString * TDSUserFriendshipRequestStatus NS_STRING_ENUM;
/// The request is pending yet.
FOUNDATION_EXPORT TDSUserFriendshipRequestStatus const TDSUserFriendshipRequestStatusPending;
/// The request is accepted by user.
FOUNDATION_EXPORT TDSUserFriendshipRequestStatus const TDSUserFriendshipRequestStatusAccepted;
/// The request is declined by user.
FOUNDATION_EXPORT TDSUserFriendshipRequestStatus const TDSUserFriendshipRequestStatusDeclined;
/// No matter status, all of requests are wanted.
FOUNDATION_EXPORT TDSUserFriendshipRequestStatus const TDSUserFriendshipRequestStatusAny;

@protocol TDSFriendshipNotificationDelegate <NSObject>

@optional

/// New request is coming.
/// @param request The friendship request.
- (void)onNewRequestComing:(LCFriendshipRequest *)request;

/// The request sent from current user is accepted by other.
/// @param request The friendship request.
- (void)onRequestAccepted:(LCFriendshipRequest *)request;

/// The request sent from current user is declined by other.
/// @param request The friendship request.
- (void)onRequestDeclined:(LCFriendshipRequest *)request;

@end

@interface TDSUser : LCUser

// MARK: Login

/// Login or register by taptap.
/// @param permissions The permissions.
/// @param callback Result callback.
+ (void)loginByTapTapWithPermissions:(NSArray * _Nullable)permissions callback:(void (^)(TDSUser * _Nullable user, NSError * _Nullable error))callback;

/// Login anonymously.
/// @param callback Result callback.
+ (void)loginAnonymously:(void (^)(TDSUser * _Nullable user, NSError * _Nullable error))callback;

// MARK: Friendship

/// Apply new friendship to someone.
/// @param userId The ID of the target user.
/// @param attributes The additional attributes.
/// @param callback Result callback.
+ (void)applyFriendshipWithUserId:(NSString *)userId
                       attributes:(NSDictionary * _Nullable)attributes
                         callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Accept a friendship.
/// @param request The friendship request.
/// @param attributes The additional attributes.
/// @param callback Result callback.
+ (void)acceptFriendshipRequest:(LCFriendshipRequest *)request
                     attributes:(NSDictionary * _Nullable)attributes
                       callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Decline a friendship.
/// @param request The friendship request.
/// @param callback Result callback.
+ (void)declineFriendshipRequest:(LCFriendshipRequest *)request
                        callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Delete friendship request directly.
/// @param request The friendship request.
/// @param callback Result callback.
+ (void)deleteFriendshipRequest:(LCFriendshipRequest *)request
                       callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Get query for friendship request of current user.
/// @param status See `TDSUserFriendshipRequestStatus`.
/// @param includeSourceUser Indicating that need to include all data of the source user or not.
+ (LCQuery *)friendshipRequestQueryWithStatus:(TDSUserFriendshipRequestStatus)status includeSourceUser:(BOOL)includeSourceUser;

/// Get friendship query of current user.
+ (nullable LCQuery *)friendshipQuery;

// MARK: Friendship Notification

/// Register friendship notification delegate.
/// @param delegate See `FriendshipNotificationDelegate` protocol.
+ (void)registerFriendshipNotificationDelegate:(id <TDSFriendshipNotificationDelegate>)delegate
                                      callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unregister friendship notification delegate.
+ (void)unregisterFriendshipNotificationDelegateWithCallback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

// MARK: Misc

- (NSMutableDictionary *)userInfoDic;

// MARK: Unavailable

+ (void)requestEmailVerify:(NSString *)email withBlock:(LCBooleanResultBlock)block NS_UNAVAILABLE;
- (BOOL)signUp:(NSError *__autoreleasing  _Nullable *)error NS_UNAVAILABLE;
- (BOOL)signUpAndThrowsWithError:(NSError *__autoreleasing  _Nullable *)error NS_UNAVAILABLE;
- (void)signUpInBackgroundWithBlock:(LCBooleanResultBlock)block NS_UNAVAILABLE;
- (void)updatePassword:(NSString *)oldPassword newPassword:(NSString *)newPassword block:(LCIdResultBlock)block NS_UNAVAILABLE;
+ (instancetype)logInWithUsername:(NSString *)username password:(NSString *)password error:(NSError *__autoreleasing  _Nullable *)error NS_UNAVAILABLE;
+ (void)logInWithUsernameInBackground:(NSString *)username password:(NSString *)password block:(LCUserResultBlock)block NS_UNAVAILABLE;
+ (void)loginWithEmail:(NSString *)email password:(NSString *)password block:(LCUserResultBlock)block NS_UNAVAILABLE;
+ (void)logInWithMobilePhoneNumberInBackground:(NSString *)phoneNumber password:(NSString *)password block:(LCUserResultBlock)block NS_UNAVAILABLE;
+ (instancetype)logInWithMobilePhoneNumber:(NSString *)phoneNumber password:(NSString *)password error:(NSError *__autoreleasing  _Nullable *)error NS_UNAVAILABLE;
+ (void)logInWithMobilePhoneNumberInBackground:(NSString *)phoneNumber smsCode:(NSString *)code block:(LCUserResultBlock)block NS_UNAVAILABLE;
+ (instancetype)logInWithMobilePhoneNumber:(NSString *)phoneNumber smsCode:(NSString *)code error:(NSError *__autoreleasing  _Nullable *)error NS_UNAVAILABLE;
+ (void)requestLoginSmsCode:(NSString *)phoneNumber withBlock:(LCBooleanResultBlock)block NS_UNAVAILABLE;
+ (void)requestLoginCodeForPhoneNumber:(NSString *)phoneNumber options:(nullable LCUserShortMessageRequestOptions *)options callback:(LCBooleanResultBlock)callback NS_UNAVAILABLE;
+ (void)signUpOrLoginWithMobilePhoneNumberInBackground:(NSString *)phoneNumber smsCode:(NSString *)code block:(LCUserResultBlock)block NS_UNAVAILABLE;
+ (void)signUpOrLoginWithMobilePhoneNumberInBackground:(NSString *)phoneNumber smsCode:(NSString *)smsCode password:(NSString *)password block:(LCUserResultBlock)block NS_UNAVAILABLE;
+ (instancetype)signUpOrLoginWithMobilePhoneNumber:(NSString *)phoneNumber smsCode:(NSString *)code error:(NSError *__autoreleasing  _Nullable *)error NS_UNAVAILABLE;
+ (BOOL)requestPasswordResetForEmail:(NSString *)email error:(NSError *__autoreleasing  _Nullable *)error NS_UNAVAILABLE;
+ (void)requestPasswordResetForEmailInBackground:(NSString *)email block:(LCBooleanResultBlock)block NS_UNAVAILABLE;
+ (void)requestPasswordResetWithPhoneNumber:(NSString *)phoneNumber block:(LCBooleanResultBlock)block NS_UNAVAILABLE;
+ (void)requestPasswordResetCodeForPhoneNumber:(NSString *)phoneNumber options:(nullable LCUserShortMessageRequestOptions *)options callback:(LCBooleanResultBlock)callback NS_UNAVAILABLE;
+ (void)resetPasswordWithSmsCode:(NSString *)code newPassword:(NSString *)password block:(LCBooleanResultBlock)block NS_UNAVAILABLE;
+ (void)requestMobilePhoneVerify:(NSString *)phoneNumber withBlock:(void (^)(BOOL, NSError * _Nullable))block NS_UNAVAILABLE;
+ (void)requestVerificationCodeForPhoneNumber:(NSString *)phoneNumber options:(nullable LCUserShortMessageRequestOptions *)options callback:(void (^)(BOOL, NSError * _Nullable))callback NS_UNAVAILABLE;
+ (void)verifyCodeForPhoneNumber:(NSString *)phoneNumber code:(NSString *)code block:(void (^)(BOOL, NSError * _Nullable))block NS_UNAVAILABLE;
+ (void)requestVerificationCodeForUpdatingPhoneNumber:(NSString *)phoneNumber options:(nullable LCUserShortMessageRequestOptions *)options block:(void (^)(BOOL, NSError * _Nullable))block NS_UNAVAILABLE;
+ (void)verifyCodeToUpdatePhoneNumber:(NSString *)phoneNumber code:(NSString *)code block:(void (^)(BOOL, NSError * _Nullable))block NS_UNAVAILABLE;
+ (void)loginAnonymouslyWithCallback:(void (^)(LCUser * _Nullable, NSError * _Nullable))callback NS_UNAVAILABLE;

@end

NS_ASSUME_NONNULL_END
