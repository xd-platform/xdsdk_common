//
//  XDGSharingService.h
//  XDCommonSDK
//
//  Created by Fattycat on 2024/2/1.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGSharingService : NSObject
+ (void)shareWithType:(NSNumber *)type
                scene:(NSNumber *)scene
          contentText:(NSString *)contentText
             videoUrl:(NSString *)videoUrl
             imageUrl:(NSString *)imageUrl
            imageData:(NSString *)imageData
                title:(NSString *)title
              linkUrl:(NSString *)linkUrl
            linkTitle:(NSString *)linkTitle
          linkSummary:(NSString *)linkSummary
       bridgeCallback:(void (^)(NSString *result))callback;

+ (BOOL)isAppInstalled:(NSNumber *)type;
@end

NS_ASSUME_NONNULL_END