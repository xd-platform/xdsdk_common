//
//  XDAdjustInfo.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2020/11/4.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDAdjustInfo : NSObject
@property (nonatomic) NSString *appToken;

@property (nonatomic) NSDictionary *evensTokenDic;

+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;

- (NSString *)eventTokenWithName:(NSString *)eventName;
@end

NS_ASSUME_NONNULL_END
