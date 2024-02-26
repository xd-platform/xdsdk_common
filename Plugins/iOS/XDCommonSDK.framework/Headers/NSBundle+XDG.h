//
//  NSBundle+XDG.h
//  XDCommonSDK
//
//  Created by Fattycat on 2023/4/23.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSBundle (XDG)

+ (instancetype)xdgBundle;

+ (NSString *)xdg_localizedStringForKey:(NSString *)key;

+ (NSString *)xdg_localizedStringForKey:(NSString *)key value:(NSString *_Nullable)value;
@end

NS_ASSUME_NONNULL_END
