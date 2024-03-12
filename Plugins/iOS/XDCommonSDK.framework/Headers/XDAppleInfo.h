//
//  XDAppleInfo.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/9/29.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDAppleInfo : NSObject
@property (nonatomic, copy) NSString *serviceId;
+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;
@end

NS_ASSUME_NONNULL_END
