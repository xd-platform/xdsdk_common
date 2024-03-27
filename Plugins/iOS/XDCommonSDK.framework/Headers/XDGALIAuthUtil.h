//
//  XDGALIAuthUtil.h
//  XDAccountSDK
//
//  Created by Fattycat on 2023/2/11.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGALIAuthUtil : NSObject
@property (nonatomic, assign) BOOL usable;
@property (nonatomic, strong, nullable) NSString *failReason;
@property (nonatomic, assign) BOOL accelerateSuccess;
+ (XDGALIAuthUtil *)sharedInstance;
@end

NS_ASSUME_NONNULL_END
