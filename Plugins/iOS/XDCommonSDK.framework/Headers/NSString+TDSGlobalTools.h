//
//  NSString+TDSGlobalTools.h
//  TDSGlobalSDKCommonKit
//
//  Created by JiangJiahao on 2021/2/25.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSString (TDSGlobalTools)
- (NSDictionary *)xdg_toDictionary;
- (NSString *)URLEncodedString;
- (NSString *)URLDecodedString;
// The string's MD5 hash
- (NSString *) MD5Hash;

+ (BOOL)isEmptyForXD:(NSString *)string;

+ (BOOL)semanticVer:(NSString *)first greaterThan:(NSString *)second;
@end

NS_ASSUME_NONNULL_END
