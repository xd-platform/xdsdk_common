//
//  DeviceInfoTool.h
//  XdComPlatform
//
//  Created by JiangJiahao on 2018/4/19.
//  Copyright © 2018年 dyy. All rights reserved.
//

#import <Foundation/Foundation.h>

extern NSUInteger const DEVICEID_LENGTH;

@interface TDSGlobalDeviceInfoTool : NSObject

/**
 取出设备ID
 1、先取出UserDefaults字段，若有直接使用，若无，去KeyChain中字段。
 2、都无，生成一个UUID，存入UserDefaults和KeyChain。
 
 @return 设备ID
 */
+ (NSString *)getDeviceId;

+ (NSString *)getKeychainDataByKey:(NSString *)key;

+ (void)setKeychainDataByKey:(NSString *)key data:(NSString *)data;


+ (NSString *)getDeviceModel;
+ (NSString *)getDeviceModelNoWhiteSpace;

+ (NSString *)getCpuFrequency;

+ (NSString *)getUUID;

+ (void)requestWithCompletionHandler:(void(^)(NSUInteger status))completion;
@end
