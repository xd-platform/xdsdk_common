#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class LocalizableString {
        [JsonProperty("tds_account_safe_info")]
        public string AccountSafeInfo { get; set; }

        [JsonProperty("tds_account_info")]
        public string AccountInfo { get; set; }

        [JsonProperty("tds_account_bind_info")]
        public string AccountBindInfo { get; set; }

        [JsonProperty("tds_unbind")]
        public string Unbind { get; set; }

        [JsonProperty("tds_bind")]
        public string Bind { get; set; }

        [JsonProperty("tds_guest")]
        public string Guest { get; set; }

        [JsonProperty("tds_delete_account_title")]
        public string DeleteAccountTitle { get; set; }

        [JsonProperty("tds_unbind_account_title")]
        public string UnbindAccountTitle { get; set; }

        [JsonProperty("tds_delete_account_sure_title")]
        public string DeleteAccountSureTitle { get; set; }

        [JsonProperty("tds_delete_account")]
        public string DeleteAccount { get; set; }

        [JsonProperty("tds_unbind_account")]
        public string UnbindAccount { get; set; }

        [JsonProperty("tds_unbind_account_button")]
        public string UnbindAccountButton { get; set; }

        [JsonProperty("tds_delete_account_sure")]
        public string DeleteAccountSure { get; set; }

        [JsonProperty("tds_delete_content")]
        public string DeleteContent { get; set; }

        [JsonProperty("tds_unbind_content")]
        public string UnbindContent { get; set; }

        [JsonProperty("tds_unbind_delete_content")]
        public string UnbindDeleteContent { get; set; }

        [JsonProperty("tds_unbind_confirm_Content")]
        public string UnbindConfirmContent { get; set; }

        [JsonProperty("tds_delete_confirm_content")]
        public string DeleteConfirmContent { get; set; }

        [JsonProperty("tds_input_error")]
        public string InputError { get; set; }

        [JsonProperty("tds_cancel")]
        public string Cancel { get; set; }

        [JsonProperty("tds_input_error_re")]
        public string InputErrorRe { get; set; }

        [JsonProperty("tds_confirm")]
        public string Confirm { get; set; }

        [JsonProperty("tds_delete")]
        public string Delete { get; set; }

        [JsonProperty("tds_loading")]
        public string Loading { get; set; }

        [JsonProperty("tds_current_account_prefix")]
        public string CurrentAccountPrefix { get; set; }

        [JsonProperty("tds_account_format")]
        public string AccountFormat { get; set; }

        [JsonProperty("tds_current_id")]
        public string CurrentId { get; set; }

        [JsonProperty("tds_copy_success")]
        public string CopySuccess { get; set; }

        [JsonProperty("tds_cancel_bind")]
        public string CancelBind { get; set; }

        [JsonProperty("tds_unbind_error")]
        public string UnbindError { get; set; }

        [JsonProperty("tds_bind_error_reason_format")]
        public string BindErrorReasonFormat { get; set; }

        [JsonProperty("tds_bind_error")]
        public string BindError { get; set; }

        [JsonProperty("tds_bind_success")]
        public string BindSuccess { get; set; }

        [JsonProperty("tds_unbind_success")]
        public string UnbindSuccess { get; set; }

        [JsonProperty("tds_unbind_success_return_sign")]
        public string UnbindSuccessReturnSign { get; set; }

        [JsonProperty("tds_unbind_delete_success_return_sign")]
        public string UnbindDeleteSuccessReturnSign { get; set; }

        [JsonProperty("tds_unbind_guest_return")]
        public string UnbindGuestReturn { get; set; }

        [JsonProperty("tds_loading_error_retry")]
        public string LoadingErrorRetry { get; set; }

        [JsonProperty("tds_network_error_retry")]
        public string NetworkErrorRetry { get; set; }

        [JsonProperty("tds_login_success")]
        public string LoginSuccess { get; set; }

        [JsonProperty("tds_login_cancel")]
        public string LoginCancel { get; set; }

        [JsonProperty("tds_login_failed")]
        public string LoginFailed { get; set; }

        [JsonProperty("tds_login_button_title")]
        public string LoginButtonTitle { get; set; }

        [JsonProperty("tds_logout")]
        public string Logout { get; set; }

        [JsonProperty("tds_authorization_cancel")]
        public string AuthorizationCancel { get; set; }

        [JsonProperty("tds_authorization_failed")]
        public string AuthorizationFailed { get; set; }

        [JsonProperty("tds_network_error_login")]
        public string NetworkErrorLogin { get; set; }

        [JsonProperty("tds_network_error_safe_retry")]
        public string NetworkErrorSafeRetry { get; set; }

        [JsonProperty("tds_refund_custom_service_tip")]
        public string RefundCustomServiceTip { get; set; }

        [JsonProperty("tds_refund_contact_custom_service")]
        public string RefundContactCustomService { get; set; }

        [JsonProperty("tds_refund_custom_service_tip_tail")]
        public string RefundCustomServiceTipTail { get; set; }

        [JsonProperty("tds_refund_success")]
        public string RefundSuccess { get; set; }

        [JsonProperty("tds_refund_fail")]
        public string RefundFail { get; set; }

        [JsonProperty("tds_refund_cancel")]
        public string RefundCancel { get; set; }

        [JsonProperty("tds_refund_action")]
        public string RefundAction { get; set; }

        [JsonProperty("tds_refund_login_restrict_title")]
        public string RefundLoginRestrictTitle { get; set; }

        [JsonProperty("tds_refund_login_restrict_sub_title")]
        public string RefundLoginRestrictSubtitle { get; set; }

        [JsonProperty("tds_refund_ios_pay_tip")]
        public string RefundIOSPayTip { get; set; }

        [JsonProperty("tds_pay_success")]
        public string PaySuccess { get; set; }

        [JsonProperty("tds_pay_fail")]
        public string PayFail { get; set; }

        [JsonProperty("tds_pay_cancel")]
        public string PayCancel { get; set; }

        [JsonProperty("tds_button_confirm")]
        public string ButtonConfirm { get; set; }

        [JsonProperty("tds_promotion_exchange_title")]
        public string PromotionExchangeTitle { get; set; }

        [JsonProperty("tds_promotion_exchange_desc")]
        public string PromotionExchangeDesc { get; set; }

        [JsonProperty("tds_refund_android_pay_tip")]
        public string RefundAndroidPayTip { get; set; }

        [JsonProperty("tds_refund_all_pay_tip")]
        public string RefundAllPayTip { get; set; }

        [JsonProperty("tds_pay_net_fail")]
        public string PayNetFail { get; set; }

        [JsonProperty("tds_refund_net_fail")]
        public string RefundNetFail { get; set; }

        [JsonProperty("tds_purchase_processing")]
        public string PurchaseProcessing { get; set; }

        [JsonProperty("tds_terms_agreement")]
        public string TermsAgreement { get; set; }

        [JsonProperty("tds_service_terms_agreement")]
        public string ServiceTermsAgreement { get; set; }

        [JsonProperty("tds_push_agreement")]
        public string PushAgreement { get; set; }

        [JsonProperty("tds_confirm_agreement")]
        public string ConfirmAgreement { get; set; }

        [JsonProperty("tds_is_adult_agreement")]
        public string IsAdultAgreement { get; set; }

        [JsonProperty("tds_california_agreement")]
        public string CaliforniaAgreement { get; set; }

        [JsonProperty("tds_cancel_claim")]
        public string CancelClaim { get; set; }

        [JsonProperty("tds_custom_service")]
        public string CustomService { get; set; }

        [JsonProperty("tds_product_not_exists")]
        public string ProductNotExists { get; set; }

        [JsonProperty("tds_ios_restricted_payment_title")]
        public string IOSRestrictedPaymentTitle { get; set; }

        [JsonProperty("tds_ios_restricted_payment_description")]
        public string IOSRestrictedPaymentDescription { get; set; }

        [JsonProperty("xd_agreement_agree")]
        public string AgreementAgree { get; set; }

        [JsonProperty("xd_agreement_disagree")]
        public string AgreementDisagree { get; set; }

        [JsonProperty("xd_agreement_disagree_confirm_content")]
        public string AgreementDisagreeConfirmContent { get; set; }

        [JsonProperty("xd_agreement_disagree_confirm_exit")]
        public string AgreementDisagreeConfirmExit { get; set; }

        [JsonProperty("xd_agreement_disagree_confirm_back")]
        public string AgreementDisagreeConfirmBack { get; set; }

        [JsonProperty("xd_agreement_age_tips")]
        public string AgreementAgeTips { get; set; }

        [JsonProperty("xd_agreement_load_failed")]
        public string AgreementLoadFailed { get; set; }

        [JsonProperty("tds_retry")]
        public string Retry { get; set; }

        [JsonProperty("tds_net_error")]
        public string NetError { get; set; }

        [JsonProperty("tds_load_error")]
        public string LoadError { get; set; }

        [JsonProperty("confirm_logout")]
        public string ConfirmLogout { get; set; }

        [JsonProperty("confirm_logout_current_account")]
        public string ConfirmLogoutCurrentAccount { get; set; }

        [JsonProperty("confirm")]
        public string Confirm2 { get; set; }

        [JsonProperty("cancel")]
        public string Cancel2 { get; set; }

        [JsonProperty("logout")]
        public string Logout2 { get; set; }

        [JsonProperty("required")]
        public string Required { get; set; }

        [JsonProperty("optional")]
        public string Optional { get; set; }

        [JsonProperty("over_14_years_old")]
        public string Over14yearsOld { get; set; }

        [JsonProperty("i_agree")]
        public string IAgree { get; set; }

        [JsonProperty("personal_information_agreement")]
        public string PersonalInformationAgreement { get; set; }

        [JsonProperty("i_agree_personal_information_agreement_reverse")]
        public bool IAgreePersonalInformationAgreementReverse { get; set; }

        [JsonProperty("confirm_your_age")]
        public string KRConfirmYourAge { get; set; }

        [JsonProperty("bind_account")]
        public string BoundAccounts { get; set; }

        [JsonProperty("bound")]
        public string Bound { get; set; }

        [JsonProperty("account_already_exists")]
        public string AccountAlreadyExists { get; set; }

        [JsonProperty("login_email_conflict")]
        public string LoginEmailConflict { get; set; }

        [JsonProperty("bind_email_conflict")]
        public string BindEmailConflict { get; set; }

        [JsonProperty("email_not_verified")]
        public string EmailNotVerified { get; set; }

        [JsonProperty("login_email_not_verified")]
        public string LoginEmailNotVerified { get; set; }

        [JsonProperty("understand")]
        public string IUnderstand { get; set; }

        [JsonProperty("unbind_last_account_tips")]
        public string UnbindLastAccountTips { get; set; }
        
        [JsonProperty("not_login")]
        public string NotLogin { get; set; }
        
        [JsonProperty("agreement_option_not_agree_alert_content")]
        public string AgreementOptionNotAgreeAlertContent { get; set; }
        
        [JsonProperty("agreement_option_not_agree_alert_confirm")]
        public string AgreementOptionNotAgreeAlertConfirm { get; set; }
        
        [JsonProperty("agreement_option_not_agree_alert_cancel")]
        public string AgreementOptionNotAgreeAlertCancel { get; set; }
        
    }
}
#endif