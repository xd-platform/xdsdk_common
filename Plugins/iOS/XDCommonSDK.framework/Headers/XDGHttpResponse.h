//
//  XDGHttpResponse.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/3/30.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGHttpResponse : NSObject
@property (nonatomic, strong, nullable) NSData *data;
@property (nonatomic, strong, nullable) NSURLResponse *response;
@property (nonatomic, strong, nullable) NSError *error;

@property (nonatomic, strong, nullable) NSString *requestUrl;
@property (nonatomic, strong, nullable) NSDictionary *resultDic;
@property (nonatomic, assign) BOOL success;

+ (XDGHttpResponse *)responseWithData:(NSData *)data response:(NSURLResponse *)response error:(NSError *)error requestUrl:(NSString *)requestUrl;

@end

NS_ASSUME_NONNULL_END
