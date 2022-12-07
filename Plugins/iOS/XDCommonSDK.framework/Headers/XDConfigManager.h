//
//  XDConfigManager.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/5/17.
//


#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGConfig.h>
#import <XDCommonSDK/XDGRegionInfo.h>

NS_ASSUME_NONNULL_BEGIN

typedef void (^XDConfigHandler)(BOOL success, XDGConfig *_Nullable config, NSString *_Nullable msg);

@interface XDConfigManager : NSObject

@property (nonatomic, strong) NSString *targetRegion;

@property (nonatomic, assign) BOOL agreementUIEnable;

@property (nonatomic, strong) NSString *configFileName;

+ (XDConfigManager *)sharedInstance;

+ (XDGConfig *)currentConfig;

+ (void)setConfig:(XDGConfig *)config;

+ (BOOL)isCN;

+ (void)readLocalConfig:(XDConfigHandler)handler;

+ (void)updateConfigWithCache;

+ (void)requestServerConfig;

+ (void)initTapSDK;

+ (void)updateBindEntriesConfig:(NSArray *)config;

+ (void)updateHttpConfig;

+ (XDGRegionInfo *)getRegionInfo;

+ (void)getRegionInfo:(void (^)(XDGRegionInfo *result))completeHandler;

+ (void)setGameInited;
+ (BOOL)isGameInited;

+ (BOOL)googleEnable;
+ (BOOL)facebookEnable;
+ (BOOL)taptapEnable;
+ (BOOL)tapdbEnable;
+ (BOOL)firebaseEnable;
+ (BOOL)adjustEnable;
+ (BOOL)appsflyersEnable;
+ (BOOL)lineEnable;
+ (BOOL)twitterEnable;

+ (BOOL)needReportService;

+(void)updateConfig:(XDGConfig *)config;
@end

NS_ASSUME_NONNULL_END
