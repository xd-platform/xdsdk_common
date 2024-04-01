//
//  XDAccountBridge.m
//  Unity-iPhone
//
//  Created by oneRain on 2024/3/6.
//

#import <Foundation/Foundation.h>
#import "XDCommonSDK/XDGHttpRequest.h"

const char* XDAccountBridgeGetAuthorization(const char* url, const char* method, long timestamp) {
    NSString *auth = [XDGHttpRequest getMactoken:[NSURL URLWithString:[NSString stringWithUTF8String:url]] method:[NSString stringWithUTF8String:method] time:[[NSNumber numberWithLong:timestamp] stringValue]];
    return [auth UTF8String];
}
