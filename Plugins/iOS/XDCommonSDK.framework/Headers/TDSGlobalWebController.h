
//  简单的网页浏览器

#import <UIKit/UIKit.h>
#import <XDCommonSDK/TDSGlobalViewControllerBase.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalWebController : TDSGlobalViewControllerBase

@property (nonatomic,copy) NSString *WebControllerTitle;

+ (TDSGlobalWebController *)createWebController:(NSString *)url;

@end

NS_ASSUME_NONNULL_END
