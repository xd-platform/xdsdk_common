//
//  XDGShareService.h
//  XDThirdLoginSDK
//
//  Created by Fattycat on 2023/8/10.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGShareService : NSObject

+ (void)shareImageWithTarget:(NSNumber *)target image:(NSString *)image bridgeCallback:(void (^)(NSString *result))callback;

+ (void)shareImageDataWithTarget:(NSNumber *)target imageData:(NSString *)image bridgeCallback:(void (^)(NSString *result))callback;

+ (void)shareUrlWithTarget:(NSNumber *)target urlText:(NSString *)url bridgeCallback:(void (^)(NSString *result))callback;

+ (void)shareUrlMessageWithTarget:(NSNumber *)target urlText:(NSString *)url message:(NSString *)message bridgeCallback:(void (^)(NSString *result))callback;

+ (void)isTargetInstalled:(NSNumber *)target bridgeCallback:(void (^)(NSString *result))callback;
@end

NS_ASSUME_NONNULL_END
