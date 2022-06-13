//
//  XDPlateWebController.h
//  TDSGlobalSDKCommonKit
//
//  Created by jessy on 2021/12/15.
//

#import <UIKit/UIKit.h>
#import <XDCommonSDK/TDSGlobalViewControllerBase.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDPlateWebController : TDSGlobalViewControllerBase

@property (nonatomic,copy) NSString *WebControllerTitle;

+ (XDPlateWebController *)createWebController:(NSString *)url;

@property (nonatomic,assign) BOOL fromAccountCancellation;

@end

NS_ASSUME_NONNULL_END
