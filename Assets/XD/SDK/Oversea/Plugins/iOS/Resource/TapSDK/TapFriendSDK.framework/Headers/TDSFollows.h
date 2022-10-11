//
//  TDSFollows.h
//  TapFriendSDK
//
//  Created by pzheng on 2021/12/31.
//

#import <Foundation/Foundation.h>
#import <TapFriendSDK/TDSFriendCommon.h>

@class TDSThirdPartyFriendQueryOption;
@class TDSThirdPartyFriendResult;
@class TDSFriendLinkInfo;
@class TDSFriendLinkOption;
@class TDSFriendHandleResult;

NS_ASSUME_NONNULL_BEGIN

/// Follows Model
@interface TDSFollows : TDSFriendCommon

// MARK: Relation Operation

/// Follow user by ID.
/// @param userId User's objectId.
/// @param callback Result callback.
+ (void)followWithUserId:(NSString *)userId
                callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Follow user by ID.
/// @param userId User's objectId.
/// @param attributes The custom attributes.
/// @param callback Result callback.
+ (void)followWithUserId:(NSString *)userId
              attributes:(NSDictionary * _Nullable)attributes
                callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Unfollow user by ID.
/// @param userId User's objectId.
/// @param callback Result callback.
+ (void)unfollowWithUserId:(NSString *)userId
                  callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Follow user by short code.
/// @param shortCode User's shortCode.
/// @param callback Result callback.
+ (void)followWithShortCode:(NSString *)shortCode
                   callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Follow user by short code.
/// @param shortCode User's shortCode.
/// @param attributes The custom attributes.
/// @param callback Result callback.
+ (void)followWithShortCode:(NSString *)shortCode
                 attributes:(NSDictionary * _Nullable)attributes
                   callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Unfollow user by short code.
/// @param shortCode User's shortCode.
/// @param callback Result callback.
+ (void)unfollowWithShortCode:(NSString *)shortCode
                     callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Block user by objectId, if you have relationships with user before, it's will be clear.
/// @param objectId User's objectId.
/// @param callback Result callback.
+ (void)blockWithObjectId:(NSString *)objectId
                 callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Unblock user by objectId, then you can create relationship with user normally.
/// @param objectId User's objectId.
/// @param callback Result callback.
+ (void)unblockWithObjectId:(NSString *)objectId
                   callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Block user by shortCode, if you have relationships with user before, it's will be clear.
/// @param shortCode User's shortCode.
/// @param callback Result callback.
+ (void)blockWithShortCode:(NSString *)shortCode
                  callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

/// Unblock user by shortCode, then you can create relationship with user normally.
/// @param shortCode User's shortCode.
/// @param callback Result callback.
+ (void)unblockWithShortCode:(NSString *)shortCode
                    callback:(void (^)(TDSFriendHandleResult * _Nullable result, NSError * _Nullable error))callback;

// MARK: Query

/// Query followee list by option.
/// @param option See `TDSFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryFolloweeWithOption:(TDSFriendQueryOption * _Nullable)option
                       callback:(void (^)(TDSFriendResult * _Nullable result, NSError * _Nullable error))callback;

/// Query follower list by option.
/// @param option See `TDSFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryFollowerWithOption:(TDSFriendQueryOption * _Nullable)option
                       callback:(void (^)(TDSFriendResult * _Nullable result, NSError * _Nullable error))callback;

/// Query mutual list by option.
/// @param option See `TDSFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryMutualListWithOption:(TDSFriendQueryOption * _Nullable)option
                         callback:(void (^)(TDSFriendResult * _Nullable result, NSError * _Nullable error))callback;

/// Query block list by option.
/// @param option See `TDSFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryBlockListWithOption:(TDSFriendQueryOption *)option
                        callback:(void (^)(TDSFriendResult * _Nullable result, NSError * _Nullable error))callback;

// MARK: Tap

/// Follow the Taptap User bound to this user.
/// @param user The User which has bound Taptap User.
/// @param callback Result callback.
+ (void)followTapUser:(TDSUser *)user
             callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unfollow the Taptap User bound to this user.
/// @param user The User which has bound Taptap User.
/// @param callback Result callback.
+ (void)unfollowTapUser:(TDSUser *)user
               callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Block the Taptap User bound to this user.
/// @param user The User which has bound Taptap User.
/// @param callback Result callback.
+ (void)blockTapUser:(TDSUser *)user
            callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unblock the Taptap User bound to this user.
/// @param user The User which has bound Taptap User.
/// @param callback Result callback.
+ (void)unblockTapUser:(TDSUser *)user
              callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Follow the Taptap User bound to this user by object id.
/// @param objectId The object id of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)followTapUserWithObjectId:(NSString *)objectId
                         callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unfollow the Taptap User bound to this user by object id.
/// @param objectId The object id of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)unfollowTapUserWithObjectId:(NSString *)objectId
                           callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Block the Taptap User bound to this user by object id.
/// @param objectId The object id of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)blockTapUserWithObjectId:(NSString *)objectId
                        callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unblock the Taptap User bound to this user by object id.
/// @param objectId The object id of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)unblockTapUserWithObjectId:(NSString *)objectId
                          callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Follow the Taptap User bound to this user by short code.
/// @param shortCode The short code of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)followTapUserWithShortCode:(NSString *)shortCode
                          callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unfollow the Taptap User bound to this user by short code.
/// @param shortCode The short code of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)unfollowTapUserWithShortCode:(NSString *)shortCode
                            callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Block the Taptap User bound to this user by short code.
/// @param shortCode The short code of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)blockTapUserWithShortCode:(NSString *)shortCode
                         callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unblock the Taptap User bound to this user by short code.
/// @param shortCode The short code of the user which has bound Taptap User.
/// @param callback Result callback.
+ (void)unblockTapUserWithShortCode:(NSString *)shortCode
                           callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

// MARK: Third Party

/// Query third-party's mutual list by option.
/// @param option See `TDSThirdPartyFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryThirdPartyMutualListWithOption:(TDSThirdPartyFriendQueryOption *)option
                                   callback:(void (^)(TDSThirdPartyFriendResult * _Nullable result, NSError * _Nullable error))callback;

/// Query third-party's followee list by option.
/// @param option See `TDSThirdPartyFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryThirdPartyFolloweeListWithOption:(TDSThirdPartyFriendQueryOption *)option
                                     callback:(void (^)(TDSThirdPartyFriendResult * _Nullable result, NSError * _Nullable error))callback;

/// Query third-party's follower list by option.
/// @param option See `TDSThirdPartyFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryThirdPartyFollowerListWithOption:(TDSThirdPartyFriendQueryOption *)option
                                     callback:(void (^)(TDSThirdPartyFriendResult * _Nullable result, NSError * _Nullable error))callback;

/// Query third-party's block list by option.
/// @param option See `TDSThirdPartyFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryThirdPartyBlockListWithOption:(TDSThirdPartyFriendQueryOption *)option
                                  callback:(void (^)(TDSThirdPartyFriendResult * _Nullable result, NSError * _Nullable error))callback;

// MARK: Link

/// Generate friendship invitation link of the current user.
/// @param error The cause of the fail.
+ (NSString * _Nullable)generateFriendInvitationLinkWithError:(NSError * __autoreleasing *)error;

/// Generate friendship invitation link of the current user.
/// @param option See `TDSFriendLinkOption`.
/// @param error The cause of the fail.
+ (NSString * _Nullable)generateFriendInvitationLinkWithOption:(TDSFriendLinkOption * _Nullable)option
                                                         error:(NSError * __autoreleasing *)error;

/// Handle other's friendship invitation link, if link is invalid then will return `false`.
/// @param link Link.
/// @param callback Result callback.
+ (BOOL)handleFriendInvitationLink:(NSURL *)link
                          callback:(void (^ _Nullable)(TDSFriendHandleResult * _Nullable result, TDSFriendLinkInfo * _Nullable linkInfo, NSError * _Nullable error))callback;

/// Parsing invitation parameters.
/// @param link Link.
+ (TDSFriendLinkInfo * _Nullable)parseFriendInvitationLink:(NSURL *)link;

@end

NS_ASSUME_NONNULL_END
