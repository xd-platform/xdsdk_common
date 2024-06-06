//
//  XDCommonBridge.m
//  Unity-iPhone
//
//  Created by oneRain on 2024/3/6.
//

#import <Foundation/Foundation.h>
#import "XDCommonSDK/XDGHttpRequest.h"
#import "XDCommonSDK/XDGHttpUtils.h"
#import "XDCommonSDK/XDConfigManager.h"

const bool XDCommonBridgeIsCN() {
    return [XDConfigManager isCN];
}

const char* XDCommonBridgeGetCommonQueryString(const char* url, long timestamp) {
    NSString *timeString = [[NSNumber numberWithLong:timestamp] stringValue];
    NSMutableDictionary *params = [XDGHttpRequest appendGetCommonParams];
    [params setObject:timeString forKey:@"time"];
    
    return [[XDGHttpUtils connectUrl:[NSString stringWithUTF8String:url] params:params] UTF8String];
}
