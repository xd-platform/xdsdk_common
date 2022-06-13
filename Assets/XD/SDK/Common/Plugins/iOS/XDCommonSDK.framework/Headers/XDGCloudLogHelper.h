//
//  XDGCloudLogHelper.h
//  XDGCommonSDK
//
//  Created by Fattycat on 2022/4/12.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGCloudLogProperties.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGCloudLogHelper : NSObject
@property (nonatomic, strong, nullable) NSString *currentLoginType;

+ (XDGCloudLogHelper *)shareInstance;

+ (void)logEvent:(XDGCloudLogProperties *)properties;

- (void)generateStaticPresetPropertiesWithDeviceInfo;
@end

NS_ASSUME_NONNULL_END
