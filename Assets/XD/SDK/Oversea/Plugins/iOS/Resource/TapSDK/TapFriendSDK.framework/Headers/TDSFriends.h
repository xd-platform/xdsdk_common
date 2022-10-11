//
//  TDSFriends.h
//  TapFriendSDK
//
//  Created by pzheng on 2021/09/23.
//

#import <Foundation/Foundation.h>
#import <TapFriendSDK/TDSFriendCommon.h>

@class TDSThirdPartyFriendResult;
@class TDSFriendLinkInfo;
@class TDSFriendLinkOption;
@class TDSFriendshipRequest;

NS_ASSUME_NONNULL_BEGIN

/// Friends Model
@interface TDSFriends : TDSFriendCommon

// MARK: Friend

/// Apply new friendship to someone.
/// @param userId The ID of the target user.
/// @param callback Result callback.
+ (void)addFriendWithUserId:(NSString *)userId
                   callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Apply new friendship to someone.
/// @param userId The ID of the target user.
/// @param attributes The additional attributes.
/// @param callback Result callback.
+ (void)addFriendWithUserId:(NSString *)userId
                 attributes:(NSDictionary * _Nullable)attributes
                   callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Apply new friendship to someone by short code.
/// @param shortCode The short code of the target user.
/// @param callback Result callback.
+ (void)addFriendWithShortCode:(NSString *)shortCode
                      callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Apply new friendship to someone by short code.
/// @param shortCode The short code of the target user.
/// @param attributes The additional attributes.
/// @param callback Result callback.
+ (void)addFriendWithShortCode:(NSString *)shortCode
                    attributes:(NSDictionary * _Nullable)attributes
                      callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Query friendship request.
/// @param status See `TDSUserFriendshipRequestStatus`.
/// @param option See `TDSFriendsQueryOption`.
/// @param callback Result callback.
+ (void)queryFriendRequestWithStatus:(TDSUserFriendshipRequestStatus)status
                              option:(TDSFriendQueryOption * _Nullable)option
                            callback:(void (^)(NSArray<LCFriendshipRequest *> * _Nullable requests, NSError * _Nullable error))callback;

/// Query friendship request and state.
/// @param status See `TDSUserFriendshipRequestStatus`.
/// @param option See `TDSFriendsQueryOption`.
/// @param callback Result callback.
+ (void)queryFriendRequestAndStateWithStatus:(TDSUserFriendshipRequestStatus)status
                                      option:(TDSFriendQueryOption * _Nullable)option
                                    callback:(void (^)(NSArray<TDSFriendshipRequest *> * _Nullable requests, NSError * _Nullable error))callback;

/// Accept a friendship.
/// @param request The friendship request.
/// @param callback Result callback.
+ (void)acceptFriendRequest:(LCFriendshipRequest *)request
                   callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Accept a friendship.
/// @param request The friendship request.
/// @param attributes The additional attributes.
/// @param callback Result callback.
+ (void)acceptFriendRequest:(LCFriendshipRequest *)request
                 attributes:(NSDictionary * _Nullable)attributes
                   callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Decline a friendship.
/// @param request The friendship request.
/// @param callback Result callback.
+ (void)declineFriendRequest:(LCFriendshipRequest *)request
                    callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Delete friendship request directly.
/// @param request The friendship request.
/// @param callback Result callback.
+ (void)deleteFriendRequest:(LCFriendshipRequest *)request
                   callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Delete friendship directly.
/// @param userId The ID of the target user.
/// @param callback Result callback.
+ (void)deleteFriendWithUserId:(NSString *)userId
                      callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Query friends.
/// @param option See `TDSFriendsQueryOption`.
/// @param callback Result callback.
+ (void)queryFriendWithOption:(TDSFriendQueryOption * _Nullable)option
                     callback:(void (^)(NSArray<TDSFriendInfo *> * _Nullable friendInfos, NSError * _Nullable error))callback;

/// Check if the target user is the friend of the current user.
/// @param userId The ID of the target user.
/// @param callback Result callback. When `isFriend.boolValue` is `true`, means the target user is the friend of the current user.
+ (void)checkFriendshipWithUserId:(NSString *)userId
                         callback:(void (^)(NSNumber * _Nullable isFriend, NSError * _Nullable error))callback;

/// Query third party friend list.
/// @param option See `TDSThirdPartyFriendQueryOption`.
/// @param callback Result callback.
+ (void)queryThirdPartyFriendListWithOption:(TDSThirdPartyFriendQueryOption *)option
                                   callback:(void (^)(TDSThirdPartyFriendResult * _Nullable result, NSError * _Nullable error))callback;

// MARK: Block List

/// Block a friend by user id.
/// @param userId The `objectId` of the user.
/// @param callback Result callback.
+ (void)blockFriendWithUserId:(NSString *)userId
                     callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Unblock a friend by user id.
/// @param userId The `objectId` of the user.
/// @param callback Result callback.
+ (void)unblockFriendWithUserId:(NSString *)userId
                       callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Query block list.
/// @param option See `TDSFriendsQueryOption`.
/// @param callback Result callback.
+ (void)queryBlockListWithOption:(TDSFriendQueryOption * _Nullable)option
                        callback:(void (^)(NSArray<TDSFriendInfo *> * _Nullable blockedUserInfos, NSError * _Nullable error))callback;

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
                          callback:(void (^ _Nullable)(BOOL succeeded, TDSFriendLinkInfo * _Nullable linkInfo, NSError * _Nullable error))callback;

/// Parsing invitation parameters.
/// @param link Link.
+ (TDSFriendLinkInfo * _Nullable)parseFriendInvitationLink:(NSURL *)link;

@end

NS_ASSUME_NONNULL_END
