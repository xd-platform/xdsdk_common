
//  处理wkwebview的cookie问题

#import <Foundation/Foundation.h>
#import <WebKit/WebKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalWKCookieWebview : WKWebView
- (id)initWithFrame:(CGRect)frame configuration:(WKWebViewConfiguration *)configuration useRedirectCookie:(BOOL)useRedirectCookie;

@end

NS_ASSUME_NONNULL_END
