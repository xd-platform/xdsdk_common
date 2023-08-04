

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface TDSGlobalTimer : NSObject

+ (TDSGlobalTimer *)timerWithTimeInterval:(NSTimeInterval)ti target:(id)aTarget selector:(SEL)aSelector userInfo:(nullable id)userInfo repeats:(BOOL)yesOrNo;

+ (TDSGlobalTimer *)timerWithTimeInterval:(NSTimeInterval)ti target:(id)aTarget selector:(SEL)aSelector userInfo:(nullable id)userInfo repeats:(BOOL)yesOrNo fireAtStart:(BOOL)fireAtStart;

- (void)invalidateTimer;

- (void)pause;

- (void)resume;

@end


NS_ASSUME_NONNULL_END
