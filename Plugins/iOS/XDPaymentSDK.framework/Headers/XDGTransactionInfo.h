
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGTransactionInfo : NSObject
// The unique server-provided identifier
@property (nonatomic,strong,readonly) NSString *transactionIdentifier;

@property (nonatomic,strong,readonly) NSString *productIdentifier;

@end

NS_ASSUME_NONNULL_END
