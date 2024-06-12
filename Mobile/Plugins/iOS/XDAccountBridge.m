//
//  XDAccountBridge.m
//  Unity-iPhone
//
//  Created by oneRain on 2024/3/6.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGHttpRequest.h>
#import <XDCommonSDK/XDGUser.h>
#import <XDAccountSDK/XDGLoginService.h>
#import <XDCommonSDK/NSDictionary+TDSGlobalJson.h>

const char* XDAccountBridgeGetAuthorization(const char* url, const char* method, long timestamp) {
    NSString *auth = [XDGHttpRequest getMactoken:[NSURL URLWithString:[NSString stringWithUTF8String:url]] method:[NSString stringWithUTF8String:method] time:[[NSNumber numberWithLong:timestamp] stringValue]];
    return [auth UTF8String];
}

const char*XDAccountBridgeGetUser() {
    XDGUser *user = [XDGUser currentUser];
    if (user.userId == nil || user.userId.length == 0) {
        return nil;
    }
    NSDictionary *userDic = [XDGLoginService bridgeUserDic:user];
    return [userDic.tdsglobal_jsonString UTF8String];
}
