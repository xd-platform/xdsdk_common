//
//  XDGAgreementConfig.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/5/31.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGAgreement.h>
NS_ASSUME_NONNULL_BEGIN

@interface XDGAgreementConfig : NSObject
@property (nonatomic, strong) NSString *url;
@property (nonatomic, strong) NSString *version;
@property (nonatomic, strong) NSString *region;
@property (nonatomic, strong) NSString *dataCollectionAgreementUrl;
@property (nonatomic, assign) BOOL isKRPushServiceSwitchEnable;
@property (nonatomic, strong) NSArray<XDGAgreement *> *agreements;
@property (nonatomic, strong) NSDictionary *originData;

+ (XDGAgreementConfig *)defaultConfig:(BOOL)isCN;

+ (XDGAgreementConfig *)instanceWithDic:(NSDictionary *)dic;

@end

NS_ASSUME_NONNULL_END
