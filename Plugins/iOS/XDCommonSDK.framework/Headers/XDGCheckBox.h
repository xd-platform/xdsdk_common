//
//  XDGCheckBox.h
//  XDCommonSDK
//
//  Created by Fattycat on 2022/11/30.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN
typedef void (^XDGCheckBoxHandler)(bool checkd);

@interface XDGCheckBox : UIView

@property (nonatomic, strong) UILabel *label;

@property (nonatomic, copy) XDGCheckBoxHandler checkBoxHandler;

@property (nonatomic, assign, getter=isChecked) bool checked;

- (void)setText:(NSString *)string;

- (void)setAttributedText:(NSAttributedString *)attributedString;

@end

NS_ASSUME_NONNULL_END
