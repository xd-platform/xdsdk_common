
//  多语言

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

extern NSString *const TDSG_ACCOUNT_CENTER_TITLE_KEY;
extern NSString *const TDSG_ACCOUNT_INFO_TITLE_KEY;
extern NSString *const TDSG_ACCOUNT_BIND_TITLE_KEY;
extern NSString *const TDSG_ACCOUNT_BIND_BUTTON_TITLE_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_BUTTON_TITLE_KEY;

extern NSString *const TDSG_ACCOUNT_DELETE_TITLE_KEY;
extern NSString *const TDSG_ACCOUNT_DELETE_SURE_TITLE_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_TITLE_KEY;

extern NSString *const TDSG_ACCOUNT_UNBIND_ALERT_TEXT_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_BUTTON_TEXT_KEY;
extern NSString *const TDSG_ACCOUNT_DELETE_ALERT_TEXT_KEY;
extern NSString *const TDSG_ACCOUNT_DELETE_ALERT_CONFIRM_TEXT_KEY;

extern NSString *const TDSG_ACCOUNT_DELETE_ALERT_CONTENT_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_ALERT_CONTENT_KEY;
extern NSString *const TDSG_ACCOUNT_DELETE_BIND_ALERT_CONTENT_KEY;

extern NSString *const TDSG_ACCOUNT_UNBIND_ALERT_CONFIRM_CONTENT_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_ALERT_CONFIRM_WORD_KEY;

extern NSString *const TDSG_ACCOUNT_DELETE_ALERT_CONFIRM_CONTENT_KEY;
extern NSString *const TDSG_ACCOUNT_DELETE_ALERT_CONFIRM_WORD_KEY;

extern NSString *const TDSG_ACCOUNT_BIND_CANCEL_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_CANCEL_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_DELETE_CANCEL_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_BIND_FAIL_FORMAT_KEY;

extern NSString *const TDSG_ACCOUNT_UNBIND_FAIL_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_SUCCESS_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_SUCCESS_LOGOUT_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_UNBIND_SUCCESS_LOGOUT_GUEST_TOAST_KEY;

extern NSString *const TDSG_ACCOUNT_BIND_FAIL_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_BIND_SUCCESS_TOAST_KEY;

extern NSString *const TDSG_ACCOUNT_DELETE_FAIL_TOAST_KEY;
extern NSString *const TDSG_ACCOUNT_DELETE_SUCCESS_LOGOUT_TOAST_KEY;


extern NSString *const TDSG_ACCOUNT_CURRENT_PREFIX_KEY;
extern NSString *const TDSG_ACCOUNT_FORMAT_KEY;
extern NSString *const TDSG_USERID_FORMAT_KEY;
extern NSString *const TDSG_ACCOUNT_NETWORK_ERROR_TOAST_KEY;


extern NSString *const TDSG_ALERT_CANCEL_TEXT_KEY;
extern NSString *const TDSG_ALERT_INPUT_ERROR_TEXT_KEY;
extern NSString *const TDSG_LOADING_TEXT_KEY;

extern NSString *const TDSG_COPY_SUCCESS_TOAST_KEY;

extern NSString *const TDSG_LOAD_FAIL_TEXT_KEY;
extern NSString *const TDSG_LOAD_NET_FAIL_TEXT_KEY;
extern NSString *const TDSG_LOAD_PROTOCOL_FAIL_TEXT_KEY;
extern NSString *const TDSG_CONFIRM_BUTTON_TEXT_KEY;

extern NSString *const TDSG_GUEST_TEXT_KEY;

extern NSString *const TDSG_LOGIN_SUCCESS_TOAST_KEY;
extern NSString *const TDSG_LOGIN_FAIL_TOAST_KEY;
extern NSString *const TDSG_AUTH_FAIL_TOAST_KEY;
extern NSString *const TDSG_LOGIN_CANCEL_TOAST_KEY;
extern NSString *const TDSG_LOGIN_ING_TOAST_KEY;
extern NSString *const TDSG_LOGIN_NETWORK_ERROR_TOAST_KEY;
extern NSString *const TDSG_LOGOUT_TEXT_KEY;
extern NSString *const TDSG_LOGIN_BUTTON_TITLE_KEY;

/*
 条款协议
 */
extern NSString *const TDSG_SERVICE_TITLE_KEY;
extern NSString *const TDSG_SERVICE_AGREE_TERMS_KEY;
extern NSString *const TDSG_SERVICE_AGREE_PUSH_KEY;
extern NSString *const TDSG_SERVICE_AGREE_CONFIRM_KEY;
extern NSString *const TDSG_SERVICE_DECLINE_CONFIRM_KEY;
extern NSString *const TDSG_SERVICE_AGREE_ADULT_KEY;
extern NSString *const TDSG_SERVICE_CALIFORNIA_TITLE_KEY;
extern NSString *const TDSG_SERVICE_CENTER_TITLE_KEY;

extern NSString *const TDSG_SERVICE_DENY_DIALOG_CONTENT_KEY;
extern NSString *const TDSG_SERVICE_DENY_DIALOG_DENY_KEY;
extern NSString *const TDSG_SERVICE_DENY_DIALOG_AGREE_KEY;
extern NSString *const TDSG_SERVICE_NA_TOAST;
/**
 支付
 */
extern NSString *const TDSG_FEFUND_TITLE_KEY;
extern NSString *const TDSG_FEFUND_DESC_KEY;
extern NSString *const TDSG_FEFUND_TEXT_KEY;
extern NSString *const TDSG_FEFUND_SUCCESS_TOAST_KEY;
extern NSString *const TDSG_FEFUND_FAIL_TOAST_KEY;
extern NSString *const TDSG_FEFUND_CANCEL_TOAST_KEY;
extern NSString *const TDSG_REFUND_NET_FAIL_TOAST_KEY;
extern NSString *const TDSG_REFUND_PROCESSING_TOAST_KEY;
extern NSString *const TDSG_FEFUND_PLATFORM_ERROR_DESC_KEY;
extern NSString *const  XDG_ACCOUNT_CANCELLATION;


extern NSString *const TDSG_FEFUND_SERVICE_TITLE_KEY;
extern NSString *const TDSG_FEFUND_SERVICE_BUTTON_TITLE_KEY;
extern NSString *const TDSG_FEFUND_SERVICE_TITLE_TAIL_KEY;

extern NSString *const TDSG_PAY_SUCCESS_TOAST_KEY;
extern NSString *const TDSG_PAY_FAIL_TOAST_KEY;
extern NSString *const TDSG_PAY_CANCEL_TOAST_KEY;

extern NSString *const TDSG_PAY_PROMOTION_EXCHANGE_TITLE_KEY;
extern NSString *const TDSG_PAY_PROMOTION_EXCHANGE_DESC_KEY;
extern NSString *const TDSG_PAY_NET_FAIL_TOAST_KEY;

extern NSString *const TDSG_PAY_PRODUCT_NOT_EXIST_TOAST_KEY;

extern NSString *const TDSG_PAY_SETTING_RESTRICT_TITLE_KEY;
extern NSString *const TDSG_PAY_SETTING_RESTRICT_DESC_KEY;

@interface TDSGlobalMultiLanguage : NSObject
+ (void)updateLanguageLocale:(NSInteger)locale;
+ (NSString *)getTextByKey:(NSString *)key;
+ (NSLocale *)getLocale;
+ (NSString *)getLanguageTypeString;
/// 客服页面语言
+ (NSString *)getReportLanguageTypeString;

@end

NS_ASSUME_NONNULL_END
