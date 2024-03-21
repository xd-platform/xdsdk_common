//
//  XDFirebaseInfo.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/7/8.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDFirebaseInfo : NSObject
@property (nonatomic, assign) BOOL enableTrack;
+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;
@end

NS_ASSUME_NONNULL_END
