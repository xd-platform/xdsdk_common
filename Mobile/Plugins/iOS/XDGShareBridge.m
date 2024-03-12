//
//  XDGShareBridge.m
//  Unity-iPhone
//
//  Created by oneRain on 2024/2/5.
//

#import <Foundation/Foundation.h>
#import "XDCommonSDK/XDGSharingService.h"

const bool XDShareBridgeIsAppInstalled(int platformType) {
    return [XDGSharingService isAppInstalled: [NSNumber numberWithInt:platformType]];
}
