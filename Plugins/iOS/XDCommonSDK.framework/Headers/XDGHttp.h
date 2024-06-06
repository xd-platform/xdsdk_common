//
//  XDGHttp.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/3/30.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGHttpRequest.h>
#import <XDCommonSDK/XDGHttpResponse.h>

NS_ASSUME_NONNULL_BEGIN

typedef  void(^XDGHttpCallback)(XDGHttpResponse *response);

@interface XDGHttp : NSObject
- (void)requestWithConfig:(XDGHttpRequest *)config callback:(nullable XDGHttpCallback)callback;
@end

NS_ASSUME_NONNULL_END
