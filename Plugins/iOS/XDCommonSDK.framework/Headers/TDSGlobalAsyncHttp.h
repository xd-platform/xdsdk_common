
//  简单HTTP请求

#import <Foundation/Foundation.h>
#import <XDCommonSDK/TDSGlobalHttpResult.h>

extern NSString *const TDSG_GLOBAL_TIMEOUTKEY;
extern NSString *const TDSG_GLOABL_HTTPMETHODKEY;
extern NSString *const TDSG_GLOBAL_HTTPBODYKEY;
extern NSString *const TDSG_GLOBAL_DATAFORMAT;

/** header */
extern NSString *const TDSG_GLOBAL_AUTH_KEY;


typedef void(^CallBackBlock)(TDSGlobalHttpResult *result);
typedef void(^GetAllCallBack)(NSArray *resultArr,BOOL successAll);


@interface TDSGlobalAsyncHttp : NSObject
@property (nonatomic,copy) CallBackBlock callBackBlock;
@property (nonatomic,copy) CallBackBlock failedCallback;

- (NSString *)currentHost;

- (void)stopTask;
- (void)retryTask;

- (void)handleSuccessResult:(TDSGlobalHttpResult *)result;
- (void)handleFailResult:(TDSGlobalHttpResult *)result;

/**
 拼接在URL后面通用参数
 */
+ (NSMutableDictionary *)appendGetCommonParams;

/**
 添加到HTTP BODY中通用参数
 */
+ (NSMutableDictionary *)appendPostCommonParams;

/// GET请求
/// @param urlStr url
/// @param requestParams 网络请求参数，如超时、格式等
/// @param customHeaderParams 自定义请求头参数
/// @param params 本次请求参数
/// @param callBackBlock 成功回调
/// @param failedCallback 失败回调
+ (TDSGlobalAsyncHttp *)httpGet:(NSString *)urlStr
                   commonParams:(NSDictionary *)commonParams
            requestParams:(NSDictionary *)requestParams
             customHeader:(NSDictionary *)customHeaderParams
                   params:(NSDictionary *)params
                 callBack:(CallBackBlock)callBackBlock failedCallback:(CallBackBlock)failedCallback;

///**
// 多个get请求并发，同时返回
//
// @param urlStrArr URL数组
// @param requestParamsArr 请求参数数组
// @param customHeaderParamsArr 自定义请求头数组
// @param paramsDicArr 参数数组
// @param callback 回掉
// */
//+ (void)httpGetAll:(NSArray *)urlStrArr
//  requestParamsArr:(NSArray *)requestParamsArr
//  customHeadersArr:(NSArray *)customHeaderParamsArr
//            params:(NSArray *)paramsDicArr
//          callback:(GetAllCallBack)callback;

/// POST请求
/// @param urlStr URL
/// @param requestParams 网络请求参数，如超时、数据格式、请求头等
/// @param customHeaderParams 自定义请求头参数
/// @param params 本次请求参数
/// @param callBackBlock 成功回调
/// @param failedCallback 失败回调
+ (TDSGlobalAsyncHttp *)httpPost:(NSString *)urlStr
             requestParams:(NSDictionary *)requestParams
              customHeader:(NSDictionary *)customHeaderParams
                    params:(NSDictionary *)params
                  callBack:(CallBackBlock)callBackBlock
            failedCallback:(CallBackBlock)failedCallback;

+ (TDSGlobalAsyncHttp *)httpPost:(NSString *)urlStr
                    commonParams:(NSDictionary *)commonParams
             requestParams:(NSDictionary *)requestParams
               queryParams:(NSDictionary *)queryParams
              customHeader:(NSDictionary *)customHeaderParams
                    params:(NSDictionary *)params
                  callBack:(CallBackBlock)callBackBlock
            failedCallback:(CallBackBlock)failedCallback;

@end
