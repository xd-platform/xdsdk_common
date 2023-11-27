//
//  UIImage+TDSSDK.h
//  TDSSDK
//
//  Created by JiangJiahao on 2020/8/23.
//  Copyright © 2020 JiangJiahao. All rights reserved.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface UIImage (TDSSDK)
+ (UIImage *)xdg_imageNamed:(NSString *)name;
+ (UIImage *)xdg_resizeableImageNamed:(NSString *)name;
@end

NS_ASSUME_NONNULL_END
