
//  简单的网页浏览器

#import <UIKit/UIKit.h>
#import <XDCommonSDK/TDSGlobalViewControllerBase.h>
#import <XDCommonSDK/XDGNavicationBar.h>
#import <XDCommonSDK/TDSGlobalWKCookieWebview.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalWebController : TDSGlobalViewControllerBase
@property (nonatomic, strong) XDGNavicationBar *navigationBar;
@property (nonatomic, strong) TDSGlobalWKCookieWebview *webView;
@property (nonatomic, copy) NSString *url;
@property (nonatomic, copy) NSString *webControllerTitle;

+ (TDSGlobalWebController *)createWebController:(NSString *)url;

@end

NS_ASSUME_NONNULL_END
