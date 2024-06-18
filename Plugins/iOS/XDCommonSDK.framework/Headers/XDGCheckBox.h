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

@property (nonatomic, assign, getter=isHalfChecked) bool halfChecked;

@property (nonatomic, assign, getter=isstaticChecked) bool staticChecked;

@property (nonatomic, assign, getter=isRoundedRect) bool roundedRect;

@property (nonatomic, assign, getter=isSolidRound) bool solidRound;

- (instancetype)initWithRoundedRect;

- (instancetype)initWithSolidRound;

- (void)setText:(NSString *)string;

- (void)setAttributedText:(NSAttributedString *)attributedString;

- (void)setHalfCheck;

- (void)setStaticChecked:(bool)staticChecked;

@end

NS_ASSUME_NONNULL_END
