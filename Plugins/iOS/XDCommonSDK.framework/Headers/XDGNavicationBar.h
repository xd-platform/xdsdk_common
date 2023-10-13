//
//  XDGNavicationBar.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/21.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGNavicationBar : UIView
@property (nonatomic, strong) UIView *backgroundView;
@property (nonatomic, strong) UIButton *backButton;
@property (nonatomic, strong) UIButton *closeButton;
@property (nonatomic, strong) UILabel *titleLabel;

+ (XDGNavicationBar *)createNavigationBar:(NSString *)title;
@end

NS_ASSUME_NONNULL_END
