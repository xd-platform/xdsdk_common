//
//  TDSGlobalTwitterShareManager.h
//  TDSGTwitterLoginKit
//
//  Created by JiangJiahao on 2021/4/23.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef void(^TDSGlobalShareHandler)(NSString * _Nullable jsonString,NSError * _Nullable error);
@interface TDSGlobalTwitterShareManager : NSObject

+ (void)postTweetWithUrl:(NSString *)url message:(NSString *)message handler:(TDSGlobalShareHandler)handler;

+ (void)postTweetWithImageData:(NSData *)imageData handler:(TDSGlobalShareHandler)handler;
@end

NS_ASSUME_NONNULL_END
