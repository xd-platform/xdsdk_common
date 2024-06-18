#if UNITY_STANDALONE || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TapTap.Bootstrap;
using TapTap.Login;
using UnityEngine;
using UnityEngine.Scripting;
using XD.SDK.Account;
using XD.SDK.Account.PC.Internal;
using XD.SDK.Common;
using XD.SDK.Common.PC.Internal;
using XD.SDK.Payment;

namespace XD.SDK.Account
{
    public class UserStatusChangeDelegate {
        public Action OnLogout { get; set; }
        public Action<XD.SDK.Account.LoginType> OnBind { get; set; }
        public Action<XD.SDK.Account.LoginType> OnUnbind { get; set; }
        public Action OnProtocolAgreedAfterLogout { get; set; }
    }
    
    [Preserve]
    public class XDGAccountPC : IXDGAccount
    {
        // 指定渠道单点登录，包括自动登录
        public async void LoginByType(XD.SDK.Account.LoginType loginType, 
            Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            try
            {
                var user = await LoginByType(loginType);

                XDGPayment.CheckRefundStatusWithUI(_ => {
                    callback?.Invoke(user);
                });
            }
            catch (XDException xe)
            {
                errorCallback?.Invoke(xe);
            }
            catch (Exception e)
            {
                errorCallback?.Invoke(XDException.MSG(e.Message));
            }
        }

        // 主机登录,移动端不需要
        public async void LoginByConsole(Action<XDGUser> successCallback, Action failCallback, Action<XDGError> errorCallback)
        {
            // TODO@luran: 解决failCallback
            try
            {
                var user = await LoginByConsole();

                XDGPayment.CheckRefundStatusWithUI(_ => {
                    successCallback?.Invoke(user);
                });
            }
            catch (XDException xe)
            {
                errorCallback?.Invoke(xe);
            }
            catch (Exception e)
            {
                errorCallback?.Invoke(XDException.MSG(e.Message));
            }
        }
        
        // 登出接口
        public async void Logout()
        {
            await InternalLogout();
        }
        
        // 得到用户信息
        public async void GetUser(Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            try
            {
                var userInfo = await GetUserInfo();
                callback?.Invoke(userInfo);
            }
            catch (XDException xe)
            {
                errorCallback?.Invoke(xe);
            }
            catch (Exception e)
            {
                errorCallback?.Invoke(XDException.MSG(e.Message));
            }
        }
        
        // 绑定账号和第三方平台，除了 Default 和 Guest
        public async void BindByType(XD.SDK.Account.LoginType loginType, Action<bool, XDGError> callback)
        {
            try
            {
                await UserModule.Bind(loginType);
                callback?.Invoke(true, null);
            }
            catch (Exception e)
            {
                callback?.Invoke(false, XDException.MSG(e.Message));
            }
        }
        
        // 添加用户状态改变的回调，状态改变包括：1）登出；2）绑定；3）解绑；4）出错了，未知code
        public void AddUserStatusChangeCallback(Action<XDGUserStatusCodeType, string> callback)
        {
            UserStatusChangeDelegate = new UserStatusChangeDelegate {
                OnBind = loginType => {
                    callback?.Invoke(XDGUserStatusCodeType.BIND, LoginTypeModel.GetName(loginType));
                },
                OnUnbind = loginType => {
                    callback?.Invoke(XDGUserStatusCodeType.UNBIND, LoginTypeModel.GetName(loginType));
                },
                OnLogout = () => {
                    // TODO@:填写Msg信息
                    callback?.Invoke(XDGUserStatusCodeType.LOGOUT, "Logout");
                },
                OnProtocolAgreedAfterLogout = () => {
                    callback?.Invoke(XDGUserStatusCodeType.ProtocolAgreedAfterLogout, null);
                }
            };
        }
        
        // 打开用户中心
        public void OpenUserCenter()
        {
            OpenUserCenterInternal();
        }

        // 打开帐户注销页面
        public async void OpenUnregister()
        {
            await CancelAccount();
        }
        
        #region XDSDK 迁移
        
        public static UserStatusChangeDelegate UserStatusChangeDelegate { get; set; }
        
        private static async Task<XDGUser> LoginByType(XD.SDK.Account.LoginType loginType) {
            XDGCommonPC.CheckInit();
            // 登陆开始 Track
            AliyunTrack.LoginStart(loginType.ToString());
            try {
                var user = await UserModule.Login(loginType);
                await AgreementModule.SignAgreement(user.userId);
                AliyunTrack.LoginSuccess();
                return user;
            } catch (XDHttpException e) {
                if ((e.HttpStatusCode == 401 && loginType == LoginType.Default) || e.code == 40311) {
                    _ = InternalLogout(false);
                }
                string errorMsg = e.Message;
                if (e.code == ResponseCode.TOKEN_EXPIRED)
                    errorMsg = "XD_TOKEN_EXPIRED";
                AliyunTrack.LoginFail(errorMsg);
                throw e;
            } catch (Exception e) {
                AliyunTrack.LoginFail(e.Message);
                AgreementModule.ClearLocalSignedAgreements();
                throw e;
            }
        }
        
        private static async Task<XDGUser> LoginByConsole() {
            XDGCommonPC.CheckInit();
            try {
                var user = await UserModule.LoginByConsole();
                await AgreementModule.SignAgreement(user.userId);
                AliyunTrack.LoginSuccess();
                return user;
            } catch (Exception e)
            {
                var errorMsg = e.Message;
                if (e is XDException xd && (xd.code == ResponseCode.TOKEN_EXPIRED || xd.code == 401))
                    errorMsg = "XD_TOKEN_EXPIRED";
                AliyunTrack.LoginFail(errorMsg);
                AgreementModule.ClearLocalSignedAgreements();
                throw e;
            }
        }
        
        internal static async Task InternalLogout(bool needNotify = true) {
            // SDK 登陆成功
            AliyunTrack.LoginLogout("LOGOUT_API");

            await TDSUser.Logout();
            TapLogin.Logout();
            AccessTokenModule.ClearToken();
            UserModule.ClearUserData();
            AgreementModule.ClearLocalSignedAgreements();
            // 确认协议
            _ = AgreementModule.ConfirmAgreement().ContinueWith(t => {
                if (!t.IsFaulted) {
                    if (needNotify)
                    {
                        UserStatusChangeDelegate?.OnProtocolAgreedAfterLogout?.Invoke();
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            if (needNotify)
            {
                UserStatusChangeDelegate?.OnLogout?.Invoke();
            }
        }
        
        private async Task<XDGUser> GetUserInfo() {
            XDGCommonPC.CheckInit();
            await CheckLogin();

            return await UserModule.FetchUserInfo();
        }
        
        private static async Task CheckLogin() {
            var localUser = await UserModule.GetLocalUser();
            if (localUser == null) {
                throw XDException.MSG("请先登录");
            }
        }
        
        private static void OpenUserCenterInternal(){
            var user = UserModule.current;
            if (user == null){
                Common.XDGLogger.Warn("请先登录");
                UIManager.ShowToast(Localization.GetCurrentLocalizableString().NotLogin);
                return;
            }

            XD.SDK.Common.PC.Internal.UIManager.ShowUI<UserCenterAlert>(null, null);
        }
        
        private static async Task CancelAccount() {
            var current = UserModule.current;
            if (current == null) {
                Common.XDGLogger.Warn("请先登录");
                UIManager.ShowToast(Localization.GetCurrentLocalizableString().NotLogin);
                return;
            }

            string cancelUrl = ConfigModule.CancelUrl;
            if (string.IsNullOrEmpty(cancelUrl)) {
                Common.XDGLogger.Warn("没有有效的注销 url");
                return;
            }

            try {
                UIManager.ShowLoading();
                Dictionary<string, object> queryParams = new Dictionary<string, object> {
                    { "kid", current.token.kid },
                    { "mac_key", current.token.macKey },
                    { "source", "game" },
                    { "cn", ConfigModule.IsGlobal ? "0" : "1" },
                    { "sdk_lang", XD.SDK.Common.PC.Internal.Localization.GetLanguageKey() },
                    { "version", XDGCommonPC.SDKVersion },
                    { "client_id", ConfigModule.ClientId },
                    { "did", SystemInfo.deviceUniqueIdentifier }
                };
                IPInfo ipInfo = await IPInfoModule.GetIpInfo();
                if (ipInfo != null) {
                    queryParams["country_code"] = ipInfo.countryCode;
                }
                if (ConfigModule.region != null) {
                    queryParams["region"] = ConfigModule.region;
                }

                string url = $"{cancelUrl}?{Common.Internal.XDUrlUtils.ToQueryString(queryParams)}";
                Common.XDGLogger.Debug($"cancel url: {url}");

                // 打开内置浏览器
                Dictionary<string, object> data = new Dictionary<string, object> {
                    { "url", url }
                };
                Action<int, object> callback = (code, obj) => {
                    if (code == CancelAccountAlert.CANCEL_DONE_CODE) {
                        _ = InternalLogout();
                    }
                };
                XD.SDK.Common.PC.Internal.UIManager.ShowUI<CancelAccountAlert>("CancelAccountAlert", data, callback);
            } finally {
                UIManager.DismissLoading();
            }
        }

        #endregion

        #region 未实现部分
        public void Login(List<XD.SDK.Account.LoginType> loginTypes, Action<XDGUser> callback, Action<XDGError> errorCallback)
        {
            Debug.LogErrorFormat("NotImplementedException");
        }
        
        public void IsTokenActiveWithType(XD.SDK.Account.LoginType loginType, Action<bool> callback)
        {
            Debug.LogErrorFormat("NotImplementedException");
        }
        
        #endregion
    }
}
#endif