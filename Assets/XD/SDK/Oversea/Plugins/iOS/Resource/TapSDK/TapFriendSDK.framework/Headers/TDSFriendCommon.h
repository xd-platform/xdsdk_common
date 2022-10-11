//
//  TDSFriendCommon.h
//  TapFriendSDK
//
//  Created by pzheng on 2021/12/30.
//

#import <Foundation/Foundation.h>
#import <LeanCloudObjc/Foundation.h>
#import <TapBootstrapSDK/TapBootstrapSDK.h>
#import <TapFriendSDK/TDSFriendQuery.h>

@class TDSFriendInfo;
@class TDSFriendResult;
@class TDSFriendshipRequest;

NS_ASSUME_NONNULL_BEGIN

/// Notification Event.
@protocol TDSFriendNotificationDelegate <NSObject>

@optional

/// New request is coming.
/// @param request The friendship request.
- (void)onNewRequestComing:(TDSFriendshipRequest *)request;

/// The request sent from current user is accepted by other.
/// @param request The friendship request.
- (void)onRequestAccepted:(TDSFriendshipRequest *)request;

/// The request sent from current user is declined by other.
/// @param request The friendship request.
- (void)onRequestDeclined:(TDSFriendshipRequest *)request;

/// New friend.
/// @param friendInfo The info of the new friend.
- (void)onFriendAdded:(TDSFriendInfo *)friendInfo;

/// Friend is online.
/// @param userId The object id of the user.
- (void)onFriendOnline:(NSString *)userId;

/// Friend is offline.
/// @param userId The object id of the user.
- (void)onFriendOffline:(NSString *)userId;

/// The rich presence of the user has been changed.
/// @param userId The object id of the user.
/// @param dictionary The rich presence of the user.
- (void)onRichPresenceChanged:(NSString *)userId dictionary:(NSDictionary * _Nullable)dictionary;

/// The connection has been established.
- (void)onConnected;

/// The connection has been disconnected.
/// The common scenario in which this event occurs:
///     1. network unreachable or interface changed
///     2. app in background for a long time
/// When the app environment back to normal, the connection will resume automatically.
/// @param error The cause of disconnection.
- (void)onDisconnectedWithError:(NSError * _Nullable)error;

/// Connecting failed or the connection encountered error and will not try resume automatically.
/// @param error The error.
- (void)onConnectionError:(NSError *)error;

@end

@interface TDSFriendCommon : NSObject

// MARK: State

/// Current user online.
+ (void)online;

/// Current user offline.
+ (void)offline;

// MARK: Notification

/// Register notification delegate.
/// @param delegate See `TDSFriendsNotificationDelegate`.
+ (void)registerNotificationDelegate:(id <TDSFriendNotificationDelegate>)delegate;

/// Unregister notification delegate.
+ (void)unregisterNotificationDelegate;

// MARK: Search

/// Search user by nickname.
/// @param nickname Nickname to be searched.
/// @param callback Result callback.
+ (void)searchUserWithNickname:(NSString *)nickname
                      callback:(void (^)(NSArray<TDSFriendInfo *> * _Nullable friendInfos, NSError * _Nullable error))callback;

/// Search user by nickname.
/// @param nickname Nickname to be searched.
/// @param option See `TDSFriendsQueryOption`.
/// @param callback Result callback.
+ (void)searchUserWithNickname:(NSString *)nickname
                        option:(TDSFriendQueryOption * _Nullable)option
                      callback:(void (^)(NSArray<TDSFriendInfo *> * _Nullable friendInfos, NSError * _Nullable error))callback;

/// Search user by short code.
/// @param shortCode Short code to be searched.
/// @param callback Result callback.
+ (void)searchUserWithShortCode:(NSString *)shortCode
                       callback:(void (^)(TDSFriendInfo * _Nullable friendInfo, NSError * _Nullable error))callback;

/// Search user by ID.
/// @param objectId Object ID to be searched.
/// @param callback Result callback.
+ (void)searchUserWithObjectId:(NSString *)objectId
                      callback:(void (^)(TDSFriendInfo * _Nullable friendInfo, NSError * _Nullable error))callback;

// MARK: Rich Presence

/// Set rich presence of the current user with key-value pair.
/// @param key Key.
/// @param value Value.
/// @param callback Result callback.
+ (void)setRichPresenceWithKey:(NSString *)key
                         value:(NSString *)value
                      callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Set rich presence of the current user with key-value pairs.
/// @param dictionary Multiple key-value pairs.
/// @param callback Result callback.
+ (void)setRichPresencesWithDictionary:(NSDictionary *)dictionary
                              callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Clear rich presence of the current user with key.
/// @param key Key
/// @param callback Result callback.
+ (void)clearRichPresenceWithKey:(NSString *)key
                        callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

/// Clear rich presence of the current user with keys.
/// @param keys The array of keys.
/// @param callback Result callback.
+ (void)clearRichPresencesWithKeys:(NSArray<NSString *> *)keys
                          callback:(void (^)(BOOL succeeded, NSError * _Nullable error))callback;

// MARK: Link

/// Set share link.
/// @param link Link.
+ (void)setShareLink:(NSString *)link;

@end

NS_ASSUME_NONNULL_END
