//
//  TapFriendLeaderBoard.h
//  TapBootstrapSDK
//
//  Created by SeraphLi on 2022/5/24.
//

#import <Foundation/Foundation.h>
#import "TDSUser.h"

NS_ASSUME_NONNULL_BEGIN

@interface TDSLeaderBoardRanking : NSObject

@property (nonatomic, copy, nonnull) NSString *statisticName;
@property (nonatomic, assign) int statisticValue;
@property (nonatomic, assign) int rank;
@property (nonatomic, readonly) TDSUser *user;

- (instancetype)initWithDictionary:(NSDictionary *)dictionary;

+ (void) queryTapFriendsLeaderBoardWithName: (NSString *) name
                      from:(int) from
                      limit: (int) limit
                      callback:(void (^)(NSArray<TDSLeaderBoardRanking *> * _Nullable leaderboards, NSError * _Nullable error)) callback;

@end

NS_ASSUME_NONNULL_END
