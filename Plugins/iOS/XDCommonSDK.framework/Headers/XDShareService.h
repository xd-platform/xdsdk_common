//
//  XDShareService.h
//  XDThirdLoginCNSDK
//
//  Created by 黄驿峰 on 2023/2/14.
//

#import <Foundation/Foundation.h>


@interface XDShareService : NSObject

+ (void)shareTextWithTarget:(NSNumber *)target scene:(NSNumber *)scene text:(NSString *)text bridgeCallback:(void (^)(NSString *result))callback;

+ (void)shareImageWithTarget:(NSNumber *)target scene:(NSNumber *)scene image:(NSString *)image bridgeCallback:(void (^)(NSString *result))callback;

+ (void)shareImageDataWithTarget:(NSNumber *)target scene:(NSNumber *)scene imageData:(NSString *)image bridgeCallback:(void (^)(NSString *result))callback;

+ (void)shareWebPageWithTarget:(NSNumber *)target scene:(NSNumber *)scene webPageConfig:(NSString *)webPageConfig bridgeCallback:(void (^)(NSString *result))callback;

+ (void)isTargetInstalled:(NSNumber *)target bridgeCallback:(void (^)(NSString *result))callback;


@end

