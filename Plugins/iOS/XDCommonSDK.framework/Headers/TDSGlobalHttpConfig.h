//
//  TDSGlobalHttpConfig.h
//  TDSGlobalSDKCommonKit
//
//  Created by JiangJiahao on 2020/12/25.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalHttpConfig : NSObject

@property (nonatomic,copy) NSString *clientId;
@property (nonatomic,copy) NSString *appId;
@property (nonatomic,copy) NSString *userKid;
@property (nonatomic,copy) NSString *userMackey;
@property (nonatomic,copy) NSString *sdkVersion;
@property (nonatomic,copy) NSString *sdkChannel;
@property (nonatomic,copy) NSString *sdkDomain;
@property (nonatomic,copy) NSString *sdkLanguage;
@property (nonatomic,copy) NSString *region;

@property (nonatomic,copy) NSString *city;
@property (nonatomic,copy) NSString *timeZone;
@property (nonatomic,copy) NSString *countryCode;
@property (nonatomic,copy) NSString *locationInfoType;

//@property (nonatomic, assign, getter=isAdvertiserIDCollectionEnabled) BOOL advertiserIDCollectionEnabled;


+ (TDSGlobalHttpConfig *)configWithClientId:(NSString * _Nullable)clientId
                                    appId:(NSString * _Nullable)appId
                                    userKid:(NSString * _Nullable)kid
                                 userMackey:(NSString * _Nullable)mackey
                                 sdkVersion:(NSString * _Nullable)version
                                 sdkChannel:(NSString * _Nullable)channel
                                  sdkDomain:(NSString * _Nullable)domain
                                sdkLanguage:(NSString * _Nullable)language
                                     region:(NSString * _Nullable)region;

+ (TDSGlobalHttpConfig *)shareInstance;

@end

NS_ASSUME_NONNULL_END
