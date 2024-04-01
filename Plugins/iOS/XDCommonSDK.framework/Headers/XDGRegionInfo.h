//
//  XDGRegionInfo.h
//  XDGCommonSDK
//
//  Created by jessy on 2021/10/26.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGRegionInfo : NSObject

/**国家代码*/
@property (nonatomic,copy) NSString *countryCode;
/**时区*/
@property (nonatomic,copy) NSString *timeZone;
/**城市*/
@property (nonatomic,copy) NSString *city;
/**locationInfoType 区分获取位置信息，
      ip ：ip 获取，
      sim：sim卡获取,
      locale：手机系统获取
 */
@property (nonatomic,copy) NSString *locationInfoType;

@end

NS_ASSUME_NONNULL_END
