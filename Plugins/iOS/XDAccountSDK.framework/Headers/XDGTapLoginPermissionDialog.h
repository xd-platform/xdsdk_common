//
//  XDGTapLoginPermissionDialog.h
//  XDAccountSDK
//
//  Created by Fattycat on 2023/10/7.
//

#import <UIKit/UIKit.h>
#import <XDCommonSDK/XDGBaseDialog.h>

NS_ASSUME_NONNULL_BEGIN

typedef void (^XDGTapLoginPermissionCheckHandler)(NSString *permission, bool checkd);
typedef void (^XDGTapLoginPermissionHandler)(NSArray *_Nullable permissionArray, bool close);

@interface XDGTapLoginPermissionDialog : XDGBaseDialog
@property (nonatomic, copy)XDGTapLoginPermissionHandler handler;
- (instancetype)initWithPermissions:(NSArray *)permissions handler:(XDGTapLoginPermissionHandler)handler;

+ (UIView *)generatePermissionViewWithCheckHandler:(XDGTapLoginPermissionCheckHandler)handler permissions:(NSArray *)permissions;
@end

NS_ASSUME_NONNULL_END
