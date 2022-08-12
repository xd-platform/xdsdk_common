//
//  XDLineInfo.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2020/11/10.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDLineInfo : NSObject
@property (nonatomic, copy) NSString *channelId;
+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;
@end

NS_ASSUME_NONNULL_END
