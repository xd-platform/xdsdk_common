//
//  XDGInputCheckDialog.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/24.
//

#import <XDCommonSDK/XDGDialog.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGInputCheckDialog : XDGDialog
@property (nonatomic, strong) NSString *checkText;

+ (XDGInputCheckDialog *)dialogWithTitle:(NSString *)title
                                 content:(NSString *)content
                               rightText:(NSString *)rightText
                                leftText:(NSString *_Nullable)leftText;
@end

NS_ASSUME_NONNULL_END
