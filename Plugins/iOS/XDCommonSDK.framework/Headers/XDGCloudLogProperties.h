//
//  XDGCloudLogProperties.h
//  XDGCommonSDK
//
//  Created by Fattycat on 2022/4/12.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

extern NSString *const XDGCloudLogProperties_TAG;
extern NSString *const XDGCloudLogProperties_NAME;
extern NSString *const XDGCloudLogProperties_DEVICE_ID;
extern NSString *const XDGCloudLogProperties_ACCOUNT;
extern NSString *const XDGCloudLogProperties_LOG_ID;
extern NSString *const XDGCloudLogProperties_SOURCE;
extern NSString *const XDGCloudLogProperties_TIME;
extern NSString *const XDGCloudLogProperties_OS;
extern NSString *const XDGCloudLogProperties_APP_VERSION;
extern NSString *const XDGCloudLogProperties_CHANNEL;
extern NSString *const XDGCloudLogProperties_SDK_VERSION;
extern NSString *const XDGCloudLogProperties_OS_VERSION;
extern NSString *const XDGCloudLogProperties_BRAND;
extern NSString *const XDGCloudLogProperties_MODEL;
extern NSString *const XDGCloudLogProperties_IDFA;
extern NSString *const XDGCloudLogProperties_IDFV;
extern NSString *const XDGCloudLogProperties_ORIENTATION;
extern NSString *const XDGCloudLogProperties_WIDTH;
extern NSString *const XDGCloudLogProperties_HEIGHT;
extern NSString *const XDGCloudLogProperties_LANG;
extern NSString *const XDGCloudLogProperties_STRUCTURE;
extern NSString *const XDGCloudLogProperties_BATTERY;
extern NSString *const XDGCloudLogProperties_LOGIN_TYPE;
extern NSString *const XDGCloudLogProperties_NETWORK;
extern NSString *const XDGCloudLogProperties_PROVIDER;
extern NSString *const XDGCloudLogProperties_SESSION_UUID;
extern NSString *const XDGCloudLogProperties_EVENT_INDEX;

@interface XDGCloudLogProperties : NSObject
- (NSMutableDictionary *)getLogProperties;

- (void)addPropertyValue:(NSObject *)value forKey:(NSString *)key;
@end

NS_ASSUME_NONNULL_END
