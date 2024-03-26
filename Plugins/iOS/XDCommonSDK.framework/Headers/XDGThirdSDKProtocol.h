//
//  XDGThirdSDKProtocol.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/19.
//

@protocol XDGThirdSDKCommonProtocol <NSObject>

+ (BOOL)initThirdPartySDK;

@end

@protocol XDGThirdSDKApplicationProtocol <NSObject>

+ (BOOL)handleOpenURL:(NSDictionary *_Nonnull)params;

+ (BOOL)handleUserActivity:(NSUserActivity *_Nonnull)userActivity restorationHandler:(nullable void (^)(NSArray<id<UIUserActivityRestoring>> *_Nullable))restorationHandler;
@end

@protocol XDGThirdSDKShareProtocol <NSObject>

+ (BOOL)isAppInstalled;

@end

@protocol XDGThirdSDKTrackProtocol <NSObject>

+ (void)event:(NSString *_Nonnull)eventName properties:(NSDictionary *_Nullable)properties;

+ (void)setLevel:(NSInteger)level;

+ (void)eventCreateRole;

+ (void)eventCompletedTutorial;

+ (void)eventUnlockedAchievement;

+ (void)eventRate;

+ (void)paymentSuccess:(NSDictionary *_Nonnull)paymentParams;

+ (void)loginSuccess;
@end
