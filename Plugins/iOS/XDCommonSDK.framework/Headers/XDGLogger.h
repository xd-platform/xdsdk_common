//
//  XDGLogger.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/3/30.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef void (^XDGLoggerCallback) (NSString *content);

@interface XDGLogger : NSObject

@property (nonatomic, assign) BOOL needConfigSubmit;

+ (XDGLogger *)sharedInstance;

+ (void)setLoggerCallback:(XDGLoggerCallback)callback;

+ (void)commonLog:(NSString *)content;

+ (void)commonSecureLog:(NSString *)content;

+ (void)logWithTag:(NSString *)tag sdkVersion:(NSString *)sdkVersion content:(NSString *)content secure:(BOOL)secure;

+ (void)submitCurrentConfig;

@end

NS_ASSUME_NONNULL_END
