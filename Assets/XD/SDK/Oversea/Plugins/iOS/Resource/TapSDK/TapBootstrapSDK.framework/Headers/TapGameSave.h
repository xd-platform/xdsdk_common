//
//  TapGameSave.h
//  TapBootstrapSDK
//
//  Created by Bottle K on 2021/8/10.
//

#import <Foundation/Foundation.h>
#import <LeanCloudObjc/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface TapGameSave : LCObject
@property (nonatomic, copy) NSString *name;
@property (nonatomic, copy) NSString *summary;
@property (nonatomic, strong) NSDate *modifiedAt;
@property (nonatomic, assign) double playedTime;
@property (nonatomic, assign) int progressValue;
@property (nonatomic, strong, nullable, readonly) LCFile *cover;
@property (nonatomic, strong, readonly) LCFile *gameFile;

/*!
 Set cover with your cover local path
 @attention Cover's type can only be jpg or png
 @param localPath Local path of your cover
 @param error Pointer to an NSError that will be set if necessary
 */
- (void)setCoverWithLocalPath:(NSString *)localPath error:(NSError *__autoreleasing *)error;

/*!
 Set gameFile with file local path
 @param localPath Local path of your cover
 @param error Pointer to an NSError that will be set if necessary
 */
- (void)setGameFileWithLocalPath:(NSString *)localPath error:(NSError *__autoreleasing *)error;

/*!
 Returns a LCQuery for GameSaves of current user.
 @result The LCQuery for GameSaves of current user.
 */
+ (LCQuery *)queryWithCurrentUser;

// MARK: Unavailable
+ (instancetype)objectWithClassName:(NSString *)className NS_UNAVAILABLE;
+ (instancetype)objectWithClassName:(NSString *)className objectId:(NSString *)objectId NS_UNAVAILABLE;
+ (instancetype)objectWithClassName:(NSString *)className dictionary:(NSDictionary *)dictionary NS_UNAVAILABLE;
- (instancetype)initWithClassName:(NSString *)newClassName NS_UNAVAILABLE;
@end

NS_ASSUME_NONNULL_END
