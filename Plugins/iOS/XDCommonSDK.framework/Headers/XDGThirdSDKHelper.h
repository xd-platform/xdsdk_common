//
//  XDGThirdSDKHelper.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/1/18.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGThirdSDKHelper : NSObject

+ (void)initThirdPartySDK;

//+ (void)setupThirdPartySDK:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions;

+ (void)openURL:(NSDictionary *)params;

+ (void)handleUserActivity:(NSUserActivity *)userActivity restorationHandler:(nullable void (^)(NSArray<id<UIUserActivityRestoring>> *_Nullable))restorationHandler;

+ (void)didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler;

+ (void)logout;

//+ (void)trackEvent:(NSString *)eventName properties:(nullable NSDictionary *)properties;
//
//+ (void)setLevel:(NSInteger)level;
//
//+ (void)eventCreateRole;
//
//+ (void)eventCompletedTutorial;
//
//+ (void)eventUnlockedAchievement;
//
//+ (void)eventRate;
//
//+ (void)paymentSuccess:(NSDictionary *)params;
@end

NS_ASSUME_NONNULL_END
