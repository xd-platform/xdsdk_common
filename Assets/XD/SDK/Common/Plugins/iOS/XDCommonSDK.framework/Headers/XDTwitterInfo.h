//
//  XDTwitterInfo.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2021/3/30.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDTwitterInfo : NSObject

@property (nonatomic) NSString *consumerKey;
@property (nonatomic) NSString *consumerSecret;

+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;

@end

NS_ASSUME_NONNULL_END
