
#import <Foundation/Foundation.h>

/** 简单区分错误类型 */
typedef NS_ENUM(NSInteger,TDSGlobalHttpErrorType) {
    TDSGlobalHttpErrorTypeNone = 0,
    TDSGlobalHttpErrorTypeNormal,
    TDSGlobalHttpErrorTypeNetworkOffline,
    TDSGlobalHttpErrorTypeNetworkTimeout,
};

@interface TDSGlobalHttpResult : NSObject

@property (nonatomic,strong) NSData *data;
@property (nonatomic,strong) NSURLResponse *response;

@property (nonatomic,strong) NSError *error;
@property (nonatomic) TDSGlobalHttpErrorType errorType;
@property (nonatomic,strong) NSError *localError;              // 本地错误

@property (nonatomic, copy) NSString *originUrl;

@property (nonatomic, strong) NSDictionary *resultDic;

/// 多个get同时返回数据时使用
@property (nonatomic,strong) NSArray *dataArr;

@end
