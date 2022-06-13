#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGAccessToken : NSObject<NSCopying>
/// 唯一标志
@property (nonatomic, copy, readonly) NSString *kid;

/// 认证码类型
@property (nonatomic, copy, readonly) NSString *tokenType;

/// mac密钥
@property (nonatomic, copy, readonly) NSString *macKey;

/// mac密钥计算方式
@property (nonatomic, copy, readonly) NSString *macAlgorithm;

/// 用户授权的权限，多个时以逗号隔开 (预留字段)
@property (nonatomic, copy, readonly) NSString *scope;

+ (XDGAccessToken *)createToken:(NSString *)kid
                       macKey:(NSString *)macKey
                    tokenType:(NSString *)tokenType;

+ (XDGAccessToken *)createTokenByDataDic:(NSDictionary *)dataDic;

@end

NS_ASSUME_NONNULL_END
