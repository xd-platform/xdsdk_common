//
//  TDSFriendQuery.h
//  TapFriendSDK
//
//  Created by pzheng on 2021/12/31.
//

#import <Foundation/Foundation.h>

/// Cache policy for query.
typedef NS_ENUM(NSUInteger, TDSFriendCachePolicy) {
    
    /// Query from cache first, if not found, then query from server.
    TDSFriendCachePolicyCacheElseNetwork,
    
    /// Query from server.
    TDSFriendCachePolicyOnlyNetwork,
};

NS_ASSUME_NONNULL_BEGIN

/// Platform of third party friend.
typedef NSString * TDSThirdPartyFriendPlatform NS_STRING_ENUM;
/// Taptap.
FOUNDATION_EXPORT TDSThirdPartyFriendPlatform const TDSThirdPartyFriendPlatformTaptap;
/// Facebook.
FOUNDATION_EXPORT TDSThirdPartyFriendPlatform const TDSThirdPartyFriendPlatformFacebook;

/// The sort of the query result.
typedef NSString * TDSFriendQuerySortType NS_STRING_ENUM;
/// Unspecific sort.
FOUNDATION_EXPORT TDSFriendQuerySortType const TDSFriendQuerySortTypeNone;
/// Sort by online state.
FOUNDATION_EXPORT TDSFriendQuerySortType const TDSFriendQuerySortTypeOnline;
/// Sort by the rich presence field with prefix.
FOUNDATION_EXPORT TDSFriendQuerySortType const TDSFriendQuerySortTypeRichPresencePrefix;

/// Sort Condition
@interface TDSFriendQuerySortCondition : NSObject

/// Append a sort type.
/// @param type See `TDSFriendQuerySortType`.
/// @param error The pointer of the error.
- (BOOL)append:(NSString *)type error:(NSError * __autoreleasing *)error;

/// Append a rich presence sort type.
/// @param field The key of the rich presence field.
- (void)appendRichPresenceField:(NSString *)field;

@end

/// Query Option.
@interface TDSFriendQueryOption : NSObject

/// Start index of the results.
@property (nonatomic) NSInteger from;

/// Count of the results.
@property (nonatomic) NSInteger limit;

@property (nonatomic, nullable) TDSFriendQuerySortCondition *sortCondition;

@end

/// Option for third party friend query.
@interface TDSThirdPartyFriendQueryOption : TDSFriendQueryOption

/// Specifies which platform to query.
@property (nonatomic, nullable) TDSThirdPartyFriendPlatform platform;
/// Specifies the limit of the results for each query, default is `50` and max is `500`.
@property (nonatomic) NSUInteger pageSize;
/// Specifies the starting location of the query, if it is `nil`, means always query the latest data from server, default is `nil`.
@property (nonatomic, nullable) NSString *cursor;
/// Specifies cache policy, default is `TDSFriendCachePolicyCacheElseNetwork`.
@property (nonatomic) TDSFriendCachePolicy cachePolicy;

@end

NS_ASSUME_NONNULL_END
