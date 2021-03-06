
#import <XDCommonSDK/TDSGlobalAsyncHttp.h>
#import <XDCommonSDK/TDSGlobalHttpConfig.h>

NS_ASSUME_NONNULL_BEGIN
typedef NSString * const TDSGlobalHTTPParamKey NS_TYPED_EXTENSIBLE_ENUM;

/** common params key */
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_CLIENT_ID_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_CHANNEL_KEY;
//FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_ACCESS_TOKEN_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_USER_ID_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_SERVICE_ID_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_ROLE_ID_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_ROLE_NAME_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_RES_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_CPU_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_MEM_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_PLATFORM_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_OS_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_MOD_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_BRAND_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_APP_VERSION_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_APP_VERSION_CODE_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_SDK_VERSION_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_LANGUAGE_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_SDK_LANGUAGE_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_GAME_NAME_KEY;
//FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_REGION_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_PKG_NAME_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_DEVICE_ID_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_DEVICE_LOCATION_KEY;
FOUNDATION_EXPORT TDSGlobalHTTPParamKey TDSG_COMMON_PARAM_DEVICE_TIME_KEY;

@interface TDSGlobalSDKAsyncHttp : TDSGlobalAsyncHttp
/// ??????HTTP?????????????????????
/// @param config ??????
+ (void)updateHttpConfig:(TDSGlobalHttpConfig *)config;

/// GET??????
/// @param urlStr url
/// @param requestParams ??????????????????????????????????????????
/// @param params ??????????????????
/// @param callBackBlock ????????????
/// @param failedCallback ????????????
+ (TDSGlobalAsyncHttp *)httpGet:(NSString *)urlStr requestParams:(nullable NSDictionary *)requestParams params:(nullable NSDictionary *)params callBack:(CallBackBlock)callBackBlock failedCallback:(CallBackBlock)failedCallback;

/// POST??????
/// @param urlStr URL
/// @param requestParams ????????????????????????????????????????????????????????????
/// @param params ??????????????????
/// @param callBackBlock ????????????
/// @param failedCallback ????????????
+ (TDSGlobalAsyncHttp *)httpPost:(NSString *)urlStr requestParams:(nullable NSDictionary *)requestParams params:(nullable NSDictionary *)params callBack:(CallBackBlock)callBackBlock failedCallback:(CallBackBlock)failedCallback;

@end

NS_ASSUME_NONNULL_END
