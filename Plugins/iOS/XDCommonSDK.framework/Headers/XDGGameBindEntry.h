//
//  XDGGameBindEntry.h
//  XDGCommonSDK
//
//  Created by JiangJiahao on 2021/5/19.
//

#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGEntryType.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGGameBindEntry : NSObject
@property (nonatomic, copy) NSString *entryName;
@property (nonatomic, assign) LoginEntryType type;
@property (nonatomic, copy) NSString *openid;
@property (nonatomic, assign) BOOL canUnbind;
@property (nonatomic, assign) BOOL canBind;

@end

NS_ASSUME_NONNULL_END
