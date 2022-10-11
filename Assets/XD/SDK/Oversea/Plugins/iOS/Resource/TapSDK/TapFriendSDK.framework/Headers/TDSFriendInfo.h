//
//  TDSFriendInfo.h
//  TapFriendSDK
//
//  Created by pzheng on 2021/09/23.
//

#import <Foundation/Foundation.h>

@class TDSUser;
@class LCFriendshipRequest;

NS_ASSUME_NONNULL_BEGIN

/// Friend Info.
@interface TDSFriendInfo : NSObject

/// User.
@property (nonatomic, readonly) TDSUser *user;

/// The rich presence of the user.
@property (nonatomic, nullable) NSDictionary *richPresence;

/// The user whether online.
@property (nonatomic) BOOL online;

@end

/// Result of third party friend query.
@interface TDSFriendResult : NSObject

/// The list of the third party friend.
@property (nonatomic, nullable, readonly) NSArray<TDSFriendInfo *> *friendList;
/// The location of the last object in `TDSThirdPartyFriendResult.friendList`.
@property (nonatomic, nullable, readonly) NSString *cursor;

@end

/// Link Info
@interface TDSFriendLinkInfo : NSObject

/// The custom name as user's title show in invitation page.
@property (nonatomic, nullable, readonly) NSString *roleName;

/// ID of user.
@property (nonatomic, nullable, readonly) NSString *identity;

/// The custom URL query.
@property (nonatomic, nullable, readonly) NSDictionary *queries;

/// Info of user.
@property (nonatomic, nullable, readonly) NSDictionary *userInfo;

@end

/// Link Option
@interface TDSFriendLinkOption : NSObject

/// The custom name as user's title show in invitation page; if it is `nil`, then use current user's nickname field.
@property (nonatomic, nullable) NSString *roleName;

/// Game want to add extra query's data.
@property (nonatomic, nullable) NSDictionary *queries;

@end

/// Friendship Request
@interface TDSFriendshipRequest : NSObject

/// `LCFriendshipRequest`.
@property (nonatomic, readonly) LCFriendshipRequest *lcFriendshipRequest;

/// `TDSFriendInfo`.
@property (nonatomic, readonly) TDSFriendInfo *friendInfo;

@end

/// Handle Result
@interface TDSFriendHandleResult : NSObject

/// Code.
@property (nonatomic, readonly) NSInteger code;

/// Message.
@property (nonatomic, nullable, readonly) NSString *message;

@end

NS_ASSUME_NONNULL_END
