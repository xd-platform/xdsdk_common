//
//  TDSLoadingInfoView.h
//  TDSSDK
//
//  Created by JiangJiahao on 2020/9/15.
//  Copyright © 2020 JiangJiahao. All rights reserved.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger,TDSGlobalLoadingInfoViewType) {
    TDSGlobalLoadingInfoViewTypeInfo = 0,
    TDSGlobalLoadingInfoViewTypeLoading,
};

typedef void(^TDSGlobalLoadingInfoViewTouchHandler)(void);

@interface TDSLoadingInfoView : UIView

@property (nonatomic) TDSGlobalLoadingInfoViewTouchHandler touchHandler;

+ (TDSLoadingInfoView *)createInfoView:(nullable NSString *)image infoText:(NSString *)info type:(TDSGlobalLoadingInfoViewType)infoType;
+ (TDSLoadingInfoView *)createInfoView:(NSString *)info type:(TDSGlobalLoadingInfoViewType)infoType;

- (void)updateInfoView:(NSString *)info type:(TDSGlobalLoadingInfoViewType)infoType;

@end

NS_ASSUME_NONNULL_END
