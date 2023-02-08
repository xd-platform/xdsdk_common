//
//  NSString+TDSGlobalTools.h
//  TDSGlobalSDKCommonKit
//
//  Created by JiangJiahao on 2021/2/25.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSString (TDSGlobalTools)
- (NSString *)URLEncodedString;
- (NSString *)URLDecodedString;
///反转字符串
- (NSString *)reverse;

// MD5 hash of the file on the filesystem specified by path
+ (NSString *) stringWithMD5OfFile: (NSString *) path;
// The string's MD5 hash
- (NSString *) MD5Hash;

// base64
- (NSString *)base64Encode;
- (NSString *)base64Dencode;

+ (BOOL)isEmptyForXD:(NSString *)string;
@end

NS_ASSUME_NONNULL_END
