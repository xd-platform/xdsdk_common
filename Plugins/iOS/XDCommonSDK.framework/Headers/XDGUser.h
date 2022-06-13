
#import <Foundation/Foundation.h>
#import <XDCommonSDK/XDGAccessToken.h>
#import <XDCommonSDK/XDGEntryType.h>

NS_ASSUME_NONNULL_BEGIN

@interface XDGUser : NSObject <NSCoding>
/**
The user's user ID.
*/
@property (nonatomic,copy,readonly) NSString *userId;

/**
The user's user name.
*/
@property (nonatomic,copy,readonly) NSString *name;

/**
 The user's nick name.
 */
@property (nonatomic,copy,readonly) NSString *nickName;

/**
 The user's head portrait.
 */
@property (nonatomic,copy) NSString *avatar;

/**
The user's current loginType.
*/
@property (nonatomic,copy,readonly) LoginEntryType loginType;
/**
The user's bound accounts. eg.@[@"TAPTAP",@"GOOGLE",@"FACEBOOK"]
*/
@property (nonatomic,copy,readonly) NSArray<NSString *> *boundAccounts;
/**
The user need push service or not
*/
@property (nonatomic,assign,readonly,getter=isPushServiceEnable) BOOL pushServiceEnable;

/**
The user's token.
*/
@property (nonatomic,strong,readonly) XDGAccessToken *token;
/// The current user profile
+ (XDGUser *)currentUser;

+ (void)clearCurrentUser;

- (instancetype)initWithUserID:(NSString *)userID
                          name:(nullable NSString *)name
                      nickName:(NSString *)nickName
                      avatar:(NSString *)avatar
                     loginType:(LoginEntryType)loginType
               boundAccounts:(NSArray *)boundAccounts
                         token:(XDGAccessToken *)token;

- (instancetype)initWithUserID:(NSString *)userID
                          name:(nullable NSString *)name
                      nickName:(NSString *)nickName
                      avatar:(NSString *)avatar
                     loginType:(LoginEntryType)loginType
                 boundAccounts:(NSArray *)boundAccounts
                         token:(XDGAccessToken *)token
             pushServiceEnable:(BOOL)enable;

@end

NS_ASSUME_NONNULL_END
