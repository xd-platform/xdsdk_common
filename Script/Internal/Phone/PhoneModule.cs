#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    public class PhoneModule {
        public const int INVALID_PHONE_NUMBER_FORMAT = 40022;
        public const int REQUEST_CODE_FREQUENTLY = 42901;
        public const int LOGIN_REQUEST_FREQUENTLY = 42900;
        public const int RISK_CONTROL = 40024;
        public const int RISK_CONTROL_FAILED = 40025;
        public const int INVALID_VERIFY_CODE = 40114;
        public const int LOGIN_FREQUENTLY = 42900;
        public const int INVALILD_TOKEN = 40113;

        public static Task Login() {
            // 手机登录总流程
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            PhoneNumberInputAlert alert = null;
            AliyunTrack.LoginAuthorize();
            AliyunTrack.LoginTrackPhoneData();
            
            alert = UIManager.ShowUI<PhoneNumberInputAlert>("XDPhoneNumberInputAlert", null, async (code, data) => {
                if (code == PhoneNumberInputAlert.CLOSE_CODE) {
                    // 关闭不结束 Task，同 Web 授权
                    UIManager.Dismiss();
                    AliyunTrack.LoginAuthorizeFail("cancel");
                    AliyunTrack.LoginFail("cancel");
                } else if (code == PhoneNumberInputAlert.NEXT_CODE) {
                    // 已经拿到手机号
                    PhoneNumberInputAlert.Data input = data as PhoneNumberInputAlert.Data;
                    string areaCode = input.AreaCode;
                    string phoneNumber = input.PhoneNumber;
                    try {
                        
                        // 验证码阶段
                        await VerifyCodeInputAlert.Verify(
                            phoneNumber,
                            riskControlData => RequestLoginVerifyCode(areaCode, phoneNumber, riskControlData),
                            verificationCode => LoginByPhone(areaCode, phoneNumber, verificationCode)
                        );
                        AliyunTrack.LoginAuthorizeSuccess();
                        tcs.TrySetResult(null);
                        UIManager.Dismiss();
                    } catch (XDException e) {
                        if (e.code == INVALID_PHONE_NUMBER_FORMAT) {
                            alert.CheckPhoneNumberValid(phoneNumber);
                        } else if (e.code == REQUEST_CODE_FREQUENTLY) {
                            // 频繁登录
                            UIManager.ShowToast(e.Message);
                        } else {
                            // 其他异常
                            if (string.IsNullOrEmpty(e.Message)) {
                                // TODO 翻译
                                UIManager.ShowToast("发送失败");
                            } else {
                                UIManager.ShowToast(e.Message);
                            }
                        }
                    } catch (TaskCanceledException tce) {
                        // 退出不处理
                        AliyunTrack.LoginAuthorizeFail(tce.Message);
                        UIManager.Dismiss();
                        AliyunTrack.LoginFail("cancel");
                    } catch (Exception e) {
                        AliyunTrack.LoginAuthorizeFail(e.Message);
                        UIManager.ShowToast("发送失败");
                    }
                }
            });
            return tcs.Task;
        }

        public static Task Bind() {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            UIManager.ShowUI<PhoneNumberInputAlert>("XDPhoneNumberInputAlert", null, async (code, data) => {
                if (code == PhoneNumberInputAlert.CLOSE_CODE) {
                    tcs.TrySetCanceled();
                    UIManager.Dismiss();
                } else if (code == PhoneNumberInputAlert.NEXT_CODE) {
                    try {
                        PhoneNumberInputAlert.Data input = data as PhoneNumberInputAlert.Data;
                        string areaCode = input.AreaCode;
                        string phoneNumber = input.PhoneNumber;
                        await VerifyCodeInputAlert.Verify(
                            phoneNumber,
                            riskControlData => RequestLoginVerifyCode(areaCode, phoneNumber, riskControlData),
                            verificationCode => BindByPhone(areaCode, phoneNumber, verificationCode)
                        );
                        tcs.TrySetResult(null);
                        UIManager.Dismiss();
                    } catch (TaskCanceledException) {
                        UIManager.Dismiss();
                    } catch (XDException e) {
                        if (e.code == REQUEST_CODE_FREQUENTLY) {
                            // 频繁登录
                            UIManager.ShowToast(e.Message);
                        } else {
                            tcs.TrySetException(e);
                        }
                    }
                }
            });
            return tcs.Task;
        }

        public static async Task Unbind(Dictionary<string, object> extras) {
            string phoneNumber = extras["openId"] as string;
            await VerifyCodeInputAlert.Verify(phoneNumber,
                riskControlData => RequestUnbindVerifyCode(riskControlData),
                code => UnbindByPhone(code));
        }

        private static async Task<RequestVerifyCodeData> RequestLoginVerifyCode(string areaCode, string phoneNumber,
           string riskControlData = null) {
            string path = "api/login/v1/verification/code/send";
            Dictionary<string, object> data = new Dictionary<string, object> {
                { "areaCode", areaCode },   // 当前版本固定国内
                { "phoneNumber", phoneNumber }
            };
            Dictionary<string, object> headers = null;
            if (riskControlData != null) {
                headers = new Dictionary<string, object>() {
                    { "Risk-Authenticate-Sig", riskControlData }
                };
            }

            RequestVerifyCodeResponse response = null;
            try
            {
                response = await XDHttpClient.Post<RequestVerifyCodeResponse>(path,
                    headers: headers, data: data);
                AliyunTrack.LoginMobileVerifyCodeSendSuccess();
            }
            catch (Exception)
            {
                AliyunTrack.LoginMobileVerifyCodeSendFail();
                throw;
            }
            
            return response?.Data;
        }

        private static async Task<RequestVerifyCodeData> RequestUnbindVerifyCode(string riskControlData = null) {
            string path = "api/account/v1/unbind/verification/code/send";
            Dictionary<string, object> headers = null;
            if (riskControlData != null) {
                headers = new Dictionary<string, object>() {
                    { "Risk-Authenticate-Sig", riskControlData }
                };
            }
            RequestVerifyCodeResponse response = await XDHttpClient.Post<RequestVerifyCodeResponse>(path,
                headers: headers);
            return response.Data;
        }

        private static async Task LoginByPhone(string areaCode, string phoneNumber, string code) {
            Dictionary<string, object> authData = new Dictionary<string, object> {
                { "type", (int) LoginType.Phone },
                { "token", $"{areaCode} {phoneNumber}" },
                { "secret", code },
                { "subType", "text" }
            };
            await AccessTokenModule.Login(authData, null);
            AliyunTrack.LoginMobileVerifyCodePassSuccess();
        }

        private static async Task BindByPhone(string areaCode, string phoneNumber, string code) {
            Dictionary<string, object> authData = new Dictionary<string, object> {
                { "type", (int) LoginType.Phone },
                { "token", $"{areaCode} {phoneNumber}" },
                { "secret", code },
                { "subType", "text" }
            };
            await XDHttpClient.Post<BaseResponse>(UserModule.XDG_BIND_INTERFACE, data: authData);
        }

        private static async Task UnbindByPhone(string code) {
            Dictionary<string, object> data = new Dictionary<string, object>() {
                { "type", (int)LoginType.Phone },
                { "verificationCode", code }
            };
            await XDHttpClient.Post<BaseResponse>(UserModule.XDG_UNBIND_INTERFACE, data: data);
        }
    }
}
#endif