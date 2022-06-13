//
//  XDGoogleInfo.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2020/11/4.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGoogleInfo : NSObject

@property (nonatomic) NSString *clientId;
@property (nonatomic) NSString *apiKey;
@property (nonatomic) BOOL isSigninEnabled;
@property (nonatomic) NSString *appId;
@property (nonatomic) BOOL isGCMEnabled;
@property (nonatomic) NSString *reversedClientId;
@property (nonatomic) NSString *gcmSenderId;
@property (nonatomic) NSString *bundleId;
@property (nonatomic) BOOL isAppInviteEnbaled;
@property (nonatomic) NSString *databaseUrl;
@property (nonatomic) BOOL isAnalyticsEnabled;
@property (nonatomic) NSString *projectId;
@property (nonatomic) BOOL isAdsEnabled;
@property (nonatomic) NSString *plistVersion;
@property (nonatomic) NSString *storageBucket;

+ (instancetype)instanceWithInfoDic:(NSDictionary *)infoDic;
@end

NS_ASSUME_NONNULL_END
