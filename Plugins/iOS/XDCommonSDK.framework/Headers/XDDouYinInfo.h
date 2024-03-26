//
//  XDDouYinInfo.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/22.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDDouYinInfo : NSObject
@property (nonatomic, copy) NSString *appId;

+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;
@end

NS_ASSUME_NONNULL_END
