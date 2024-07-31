//
//  XDGAgreementConfig.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/5/31.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGAgreement.h>
NS_ASSUME_NONNULL_BEGIN

@interface XDGAgreementConfigOption : NSObject
@property (nonatomic, strong) NSString *name;
@property (nonatomic, strong) NSString *title;
@property (nonatomic, assign) bool required;

+ (XDGAgreementConfigOption *)instanceWithDic:(NSDictionary *)dic;
@end

@interface XDGAgreementConfig : NSObject
@property (nonatomic, strong) NSString *version;
@property (nonatomic, strong) NSString *region;

@property (nonatomic, strong) NSString *titleFirst;
@property (nonatomic, strong) NSString *summaryFirst;
@property (nonatomic, strong) NSString *promptFirst;

@property (nonatomic, strong) NSString *title;
@property (nonatomic, strong) NSString *summary;
@property (nonatomic, strong) NSString *prompt;

@property (nonatomic, strong) NSString *agree;
@property (nonatomic, strong) NSString *disagree;

@property (nonatomic, strong) NSString *allOptions;
@property (nonatomic, strong) NSArray<XDGAgreementConfigOption *> *_Nullable options;

@property (nonatomic, assign) BOOL isFirstCheck;

+ (XDGAgreementConfig *)instanceWithDic:(NSDictionary *)dic;

- (NSString *)getTitle;

- (NSString *)getSummary;

- (NSString *)getPrompt;

@end

NS_ASSUME_NONNULL_END
