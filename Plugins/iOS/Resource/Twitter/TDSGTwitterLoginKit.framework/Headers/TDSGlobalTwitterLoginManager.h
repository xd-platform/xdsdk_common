/**
 封装twitter api  API VERSION : 1.1
 */

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "TDSGlobalTwitterAuthToken.h"
#import <AuthenticationServices/AuthenticationServices.h>

NS_ASSUME_NONNULL_BEGIN
typedef void(^TDSGlobalTwitterLoginHandler)(TDSGlobalTwitterAuthToken * _Nullable token,NSError * _Nullable error);

@interface TDSGlobalTwitterLoginManager : NSObject

+ (void)enableWithConsumerKey:(NSString *)consumerKey consumerSecret:(NSString *)consumerSecret;

+ (NSString *)getConsumerKey;

+ (NSString *)getConsumerSecret;

+ (BOOL)isAuthed;

+(void)twitterAuthWithProvider:(id <ASWebAuthenticationPresentationContextProviding> _Nonnull)provider handler:(TDSGlobalTwitterLoginHandler)handler API_AVAILABLE(ios(13.0));

+(void)twitterAuthWithViewController:(UIViewController *)viewController handler:(TDSGlobalTwitterLoginHandler)handler NS_DEPRECATED_IOS(10_0, 12_0);
;

+ (void)handleOpenUrl:(NSURL *)url;
@end

NS_ASSUME_NONNULL_END
