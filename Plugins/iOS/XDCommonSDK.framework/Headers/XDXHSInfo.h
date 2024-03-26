//
//  XDXHSInfo.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/22.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDXHSInfo : NSObject
@property (nonatomic, copy) NSString *appId;
@property (nonatomic, copy) NSString *universalLink;

+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;
@end

NS_ASSUME_NONNULL_END
