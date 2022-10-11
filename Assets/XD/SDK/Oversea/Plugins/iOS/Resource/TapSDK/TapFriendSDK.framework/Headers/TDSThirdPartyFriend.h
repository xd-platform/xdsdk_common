//
//  TDSThirdPartyFriend.h
//  TapFriendSDK
//
//  Created by pzheng on 2021/11/15.
//

#import <Foundation/Foundation.h>

@class TDSFriendInfo;

NS_ASSUME_NONNULL_BEGIN

/// The object of third party friend.
@interface TDSThirdPartyFriend : NSObject

/// The ID of this friend.
@property (nonatomic, nullable) NSString *userId;

/// The name of this friend.
@property (nonatomic, nullable) NSString *userName;

/// The avatar of this friend.
@property (nonatomic, nullable) NSString *userAvatar;

/// If this friend has bound `TDSUser`, then this property is the relevant information, if not, this property is `nil`.
@property (nonatomic, nullable) TDSFriendInfo *tdsFriendInfo;

@end

/// Result of third party friend query.
@interface TDSThirdPartyFriendResult : NSObject

/// The list of the third party friend.
@property (nonatomic, nullable, readonly) NSArray<TDSThirdPartyFriend *> *friendList;

/// The location of the last object in `TDSThirdPartyFriendResult.friendList`.
@property (nonatomic, nullable, readonly) NSString *cursor;

@end

NS_ASSUME_NONNULL_END
