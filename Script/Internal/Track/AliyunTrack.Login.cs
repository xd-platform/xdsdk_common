#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    /// <summary>
    /// 登陆部分
    /// </summary>
    internal partial class AliyunTrack
    {
        private static string loginTypeString = "";
        internal static string LoginEventSessionId;
        private static string userLoginTypeString;
        private static bool enableAuthorizeTrack;
        private static bool enableSDKLoginTrack;
        private static bool enableSlientAuthorize;
        private static bool isPhoneLogin;
        private static bool enableTrackPhoneData;
        // 手机号登录时候的手机号
        internal static string MobileNumberString { get; set; }
        // 登陆验证码显示次数
        internal static int VerifyShowCount { get; set; }
        // 登陆验证码发送次数
        internal static int VerifySendCount { get; set; }
        // 登陆验证码校验次数
        internal static int VerifyResponseCount { get; set; }

        internal static void ResetPhoneLoginData()
        {
            VerifyShowCount = VerifySendCount = VerifyResponseCount = 0;
            MobileNumberString = "";
        }

        internal static void LoginTrackPhoneData()
        {
            enableTrackPhoneData = true;
        }
        /// <summary>
        /// 开始登录
        /// </summary>
        /// <param name="currentLoginTypeString">本次登陆类型</param>
        /// <param name="userLoginTypeString">上次登陆类型</param>
        internal static void LoginStart(string currentLoginTypeString, string userLoginTypeString = null)
        {
            loginTypeString = currentLoginTypeString;
            LoginEventSessionId = GetNewEventSessionId();
            AliyunTrack.userLoginTypeString = userLoginTypeString ?? currentLoginTypeString;
            enableAuthorizeTrack = false;
            isPhoneLogin = currentLoginTypeString == LoginType.Phone.ToString();
            MobileNumberString = "";
            LogEventAsync("sdklogin_start", GetLoginModuleCommonProperties("sdklogin_start"));
        }
        
        /// <summary>
        /// 登陆封控拦截成功
        /// </summary>
        internal static void LoginRiskSuccess(string reason)
        {
            var contents = GetLoginModuleCommonProperties("sdklogin_risk_success");
            contents["event_reason"] = reason;
            LogEventAsync("sdklogin_risk_success", contents);
        }
        
        /// <summary>
        /// 激活授权埋点
        /// </summary>
        internal static void LoginEnableAuthorizeTrack()
        {
            enableAuthorizeTrack = true;
        }
        
        /// <summary>
        /// 激活 SDK 登陆成功/失败埋点
        /// </summary>
        internal static void LoginEnableSDKLoginTrack()
        {
            enableSDKLoginTrack = true;
        }

        /// <summary>
        /// 激活静默登陆
        /// </summary>
        internal static void LoginEnableSlientAuthorize()
        {
            enableSlientAuthorize = true;
        }
        
        /// <summary>
        /// 打开手机登录页面
        /// </summary>
        internal static void LoginOpenMobilePanel()
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_to_mobile_define");
            // enter_from 代表进入该页面的上一级页面,PC 上只有正常打开这一种情况
            contents["enter_from"] = "start";
            LogEventAsync("sdklogin_to_mobile_define", contents);
        }
        
        /// <summary>
        /// 输入手机号页面点击按钮
        /// 1-下一步;2-使用一键登录;3-关闭
        /// </summary>
        internal static void LoginMobileInputBtnClick(int buttonType)
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_mobile_button_click");
            // enter_from 代表进入该页面的上一级页面,PC 上只有正常打开这一种情况
            contents["button_click"] = buttonType.ToString();
            LogEventAsync("sdklogin_mobile_button_click", contents);
        }
        
        /// <summary>
        /// 滑动验证结果,因为滑动验证逻辑是在前端页面里,sdk 并不能直接拿到成功或者失败的逻辑,所以不埋点
        /// </summary>
        /// <param name="result">验证结果:0-失败;1-成功</param>
        /// <param name="failReason">验证失败时记录失败错误码</param>
        /// <param name="slideCount">本次登录seesionid内滑块验证次数</param>
        internal static void LoginMobileDivRes(int result, string failReason, int slideCount)
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_div_res");
            contents["get_res"] = result.ToString();
            contents["event_reason"] = failReason;
            contents["event_times"] = slideCount.ToString();
            LogEventAsync("sdklogin_div_res", contents);
        }
        
        /// <summary>
        /// 唤起输入验证码页面
        /// </summary>
        internal static void LoginMobileVerifyCodeInput()
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_mobile_code_input");
            contents["event_times"] = (++VerifyShowCount).ToString();
            LogEventAsync("sdklogin_mobile_code_input", contents);
        }
        
        /// <summary>
        /// 手机登录发送验证码成功
        /// </summary>
        internal static void LoginMobileVerifyCodeSendSuccess()
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_mobile_code_send_success");
            contents["event_times"] = (++VerifySendCount).ToString();
            LogEventAsync("sdklogin_mobile_code_send_success", contents);
        }
        
        /// <summary>
        /// 手机登录发送验证码失败	
        /// </summary>
        internal static void LoginMobileVerifyCodeSendFail()
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_mobile_code_send_fail");
            contents["event_times"] = (++VerifySendCount).ToString();
            LogEventAsync("sdklogin_mobile_code_send_fail", contents);
        }
        
        /// <summary>
        /// 手机登录验证码校验通过	
        /// </summary>
        internal static void LoginMobileVerifyCodePassSuccess()
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_mobile_code_success");
            contents["event_times"] = (++VerifyResponseCount).ToString();
            LogEventAsync("sdklogin_mobile_code_success", contents);
        }
        
        /// <summary>
        /// 手机登录验证码校验失败	
        /// </summary>
        internal static void LoginMobileVerifyCodePassFail(string reason)
        {
            if (false == enableTrackPhoneData) return;
            var contents = GetLoginModuleCommonProperties("sdklogin_mobile_code_fail");
            contents["event_times"] = (++VerifyResponseCount).ToString();
            contents["event_reason"] = reason;
            LogEventAsync("sdklogin_mobile_code_fail", contents);
        }

        /// <summary>
        /// 登陆跳转授权
        /// </summary>
        internal static void LoginAuthorize()
        {
            if (!enableAuthorizeTrack) return;
            if (!enableSlientAuthorize)
                LogEventAsync("sdklogin_to_authorize", GetLoginModuleCommonProperties("sdklogin_to_authorize"));
            else
                LogEventAsync("silent_sdklogin_to_authorize", GetLoginModuleCommonProperties("silent_sdklogin_to_authorize"));
        }
        
        /// <summary>
        /// 登陆授权成功
        /// </summary>
        internal static void LoginAuthorizeSuccess()
        {
            if (enableAuthorizeTrack)
            {
                if (!enableSlientAuthorize)
                    LogEventAsync("sdklogin_authorize_success", GetLoginModuleCommonProperties("sdklogin_authorize_success"));
                else
                    LogEventAsync("silent_sdklogin_authorize_success", GetLoginModuleCommonProperties("silent_sdklogin_authorize_success"));
            }

            enableAuthorizeTrack = false;
        }
        
        /// <summary>
        /// 登陆授权失败
        /// </summary>
        internal static void LoginAuthorizeFail(string reason)
        {
            if (enableAuthorizeTrack)
            {
                if (!enableSlientAuthorize)
                {
                    var contents = GetLoginModuleCommonProperties("sdklogin_authorize_fail");
                    contents["event_reason"] = reason;
                    LogEventAsync("sdklogin_authorize_fail", contents);
                }
                else
                {
                    var contents = GetLoginModuleCommonProperties("silent_sdklogin_authorize_fail");
                    contents["event_reason"] = reason;
                    LogEventAsync("silent_sdklogin_authorize_fail", contents);
                }
            }

            enableSlientAuthorize = false;
        }
        
        /// <summary>
        /// SDK 登录成功
        /// </summary>
        internal static void LoginPreSuccess()
        {
            if (enableSDKLoginTrack)
            {
                if (!enableSlientAuthorize)
                    LogEventAsync("sdklogin_pre_login_success", GetLoginModuleCommonProperties("sdklogin_pre_login_success"));
                else
                    LogEventAsync("silent_sdklogin_pre_login_success", GetLoginModuleCommonProperties("silent_sdklogin_pre_login_success", userLoginTypeString));
            }

            enableSDKLoginTrack = false;
            enableSlientAuthorize = false;
            
        }
        
        /// <summary>
        /// SDK 登录失败
        /// </summary>
        internal static void LoginPreFail(string reason)
        {
            if (enableSDKLoginTrack)
            {
                if (!enableSlientAuthorize)
                {
                    var contents = GetLoginModuleCommonProperties("sdklogin_pre_login_fail", userLoginTypeString);
                    contents["event_reason"] = reason;
                    LogEventAsync("sdklogin_pre_login_fail", contents);
                }
                else
                {
                    var contents = GetLoginModuleCommonProperties("silent_sdklogin_pre_login_fail", userLoginTypeString);
                    contents["event_reason"] = reason;
                    LogEventAsync("silent_sdklogin_pre_login_fail", contents);
                }
            }

            enableSDKLoginTrack = false;
            enableSlientAuthorize = false;
        }

        /// <summary>
        /// 登录成功
        /// </summary>
        internal static void LoginSuccess()
        {
            var contents = GetLoginModuleCommonProperties("sdklogin_success", userLoginTypeString);
            LogEventAsync("sdklogin_success", contents);
            enableTrackPhoneData = false;
            ResetPhoneLoginData();
        }
        
        /// <summary>
        /// 登录失败
        /// </summary>
        /// <param name="extraInfo">失败原因</param>
        internal static void LoginFail(string reason)
        {
            var contents = GetLoginModuleCommonProperties("sdklogin_fail", userLoginTypeString);
            contents["event_reason"] = reason;
            LogEventAsync("sdklogin_fail", contents);
            enableTrackPhoneData = false;
            ResetPhoneLoginData();
        }
        
        /// <summary>
        /// 登出账号
        /// </summary>
        /// <param name="extraInfo">登出原因(1.LOGOUT_API：游戏调用 SDK 的 logout.;2.XD_TOKEN_EXPIRED：自动登录时 XD Token 过期调用的 logout.;3.THIRD_TOKEN_EXPIRED：自动登录时第三方登录 Token 过期调用的 logout</param>
        internal static void LoginLogout(string reason)
        {
            var contents = GetLoginModuleCommonProperties("sdklogin_logout", userLoginTypeString);
            contents["event_reason"] = reason;
            LogEventAsync("sdklogin_logout", contents);
            ResetLoginData();
            ResetPhoneLoginData();
        }

        private static void ResetLoginData()
        {
            userLoginTypeString = "";
            loginTypeString = "";
        }
        
        private static Dictionary<string, string> GetLoginModuleCommonProperties(string eventName, string userLoginTypeString = "")
        {
            var current = UserModule.current?.userId;
            var content = new Dictionary<string, string>();
            content["tag"] = "sdklogin";
            content["event_session_id"] = LoginEventSessionId;
            content["login_type"] = loginTypeString;
            content["user_login_type"] = string.IsNullOrEmpty(current) ? "" : userLoginTypeString;
            content["logid"] = GetLogId(LoginEventSessionId, eventName);
            content["isLoginModule"] = "true"; 
            if (SteamUtils.IsSDKSupported)
            {
                content["console_type"] = "steam";
            }
            if (isPhoneLogin)
            {
                // text 代表验证码登录, sim 代表自动登录
                // PC 上只有验证码登录
                content["sub_login_type"] = "text";
                content["mobile"] = string.IsNullOrEmpty(MobileNumberString) ? "" : CreateMD5("86 " + MobileNumberString);
            }
            return content;
        }
        
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string prior to .NET 5
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
    
    
}
#endif