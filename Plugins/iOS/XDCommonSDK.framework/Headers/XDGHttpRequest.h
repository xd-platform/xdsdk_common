//
//  XDGHttpRequest.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/3/30.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGHttpRequest : NSObject
@property (nonatomic, strong) NSString *method;
@property (nonatomic, strong) NSString *urlString;
@property (nonatomic, strong) NSDictionary *queries;
@property (nonatomic, strong) NSDictionary *bodyDictionary;
@property (nonatomic, strong) NSData *body;
@property (nonatomic, strong) NSDictionary *headers;
@property (nonatomic, assign) NSTimeInterval timeoutInterval;
@property (nonatomic, assign) BOOL authentation;

+ (XDGHttpRequest *)getRequestWithUrl:(NSString *)url;

+ (XDGHttpRequest *)getRequestWithUrl:(NSString *)url authentation:(BOOL)authentation;

+ (XDGHttpRequest *)postRequestWithUrl:(NSString *)url;

+ (XDGHttpRequest *)postRequestWithUrl:(NSString *)url authentation:(BOOL)authentation;

+ (XDGHttpRequest *)postRequestWithUrl:(NSString *)url bodyDictionary:(NSDictionary *_Nullable)bodyDictionary;

+ (XDGHttpRequest *)postRequestWithUrl:(NSString *)url authentation:(BOOL)authentation bodyDictionary:(NSDictionary *_Nullable)bodyDictionary;

+ (NSMutableDictionary *)appendGetCommonParams;

+ (NSString *)getMactoken:(NSURL *)url method:(NSString *)method time:(NSString*)ts;

+ (NSString *)randomStringInLength:(int)length;
@end

NS_ASSUME_NONNULL_END
