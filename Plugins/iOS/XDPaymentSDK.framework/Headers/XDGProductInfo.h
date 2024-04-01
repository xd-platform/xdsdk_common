
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGProductInfo : NSObject
@property(nonatomic,strong,readonly) NSString *localizedDescription;

@property(nonatomic,strong,readonly) NSString *localizedTitle;

@property(nonatomic,strong,readonly) NSDecimalNumber *price;

@property(nonatomic,strong,readonly) NSLocale *priceLocale;

@property(nonatomic,strong,readonly) NSString *productIdentifier;

@end

NS_ASSUME_NONNULL_END
