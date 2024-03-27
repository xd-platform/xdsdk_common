//
//  XDWeiboInfo.h
//  XDCommonSDK
//
//  Created by 黄驿峰 on 2023/2/10.
//

#import <Foundation/Foundation.h>


@interface XDWeiboInfo : NSObject

@property (nonatomic, copy) NSString *appId;
@property (nonatomic, copy) NSString *universalLink;

+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;

@end

