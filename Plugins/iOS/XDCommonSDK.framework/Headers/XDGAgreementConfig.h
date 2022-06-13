//
//  XDGAgreementConfig.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/5/31.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGAgreementConfig : NSObject
@property (nonatomic, strong) NSString *firstUrl;
@property (nonatomic, strong) NSString *updateUrl;
@property (nonatomic, strong) NSString *version;
@property (nonatomic, strong) NSString *region;
@property (nonatomic, assign) BOOL isKRPushServiceSwitchEnable;

+ (XDGAgreementConfig *)defaultConfig:(BOOL)isCN;

+ (XDGAgreementConfig *)instanceWithDic:(NSDictionary *)dic;

@end

NS_ASSUME_NONNULL_END
