//
//  XDGAgreementManager.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/10/8.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGAgreementConfig.h>

NS_ASSUME_NONNULL_BEGIN
typedef void (^XDGAgreementHandler)(BOOL success, XDGAgreementConfig *_Nullable config, NSString *_Nullable msg);
typedef void (^XDGExitHandler)(void);
@interface XDGAgreementManager : NSObject

@property (nonatomic) XDGExitHandler exitHandler;

+ (XDGAgreementManager *)sharedInstance;

+ (XDGAgreementConfig *)currentAgreement;

+ (void)startAgreementWithHandler:(void (^)(void))handler;

+ (void)checkAgreementWithHandler:(void (^)(void))handler;

+ (void)logout;

+ (NSArray<XDGAgreement *> *)getAgreementList;

+ (void)showDetailAgreement:(NSString *)type;

+ (void)recordKRPushSetting:(BOOL)pushOn;

+ (void)recordKRPushDaySetting:(BOOL)pushOn;

+ (void)recordKRPushNightSetting:(BOOL)pushOn;

+ (void)recordKRPushSettingWithDay:(BOOL)dayPushOn withNight:(BOOL)nightPushOn;

+ (BOOL)getKRPushSetting;

+ (BOOL)getKRPushDaySetting;

+ (BOOL)getKRPushNightSetting;
@end

NS_ASSUME_NONNULL_END
