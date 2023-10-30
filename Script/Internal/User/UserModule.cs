#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using UnityEngine;
using TapTap.Bootstrap;
using XD.SDK.Account;
using XD.SDK.Account;
using XD.SDK.Account.PC.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class UserModule {
        // 获取用户信息
        private readonly static string XDG_USER_PROFILE = "api/account/v1/info";

        // 与leanCloud同步
        private readonly static string XDG_LOGIN_SYN = "api/login/v1/syn";

        // 获取用户绑定信息
        private readonly static string XDG_BIND_LIST = "api/account/v1/bind/list";

        // 绑定接口
        public readonly static string XDG_BIND_INTERFACE = "api/account/v1/bind";

        // 解绑接口
        public readonly static string XDG_UNBIND_INTERFACE = "api/account/v1/unbind";

        private static readonly Persistence persistence = new Persistence(Path.Combine(Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "user"));

        internal static XDUser current = null;

        public static async Task<XDGUser> GetLocalUser() {
            if (current == null) {
                current = await persistence.Load<XDUser>();
            }
            return current;
        }

        public static void ClearUserData() {
            persistence.Delete();
            current = null;
        }

        public static async Task<List<Bind>> GetBindList() {
            BindResponse response = await XDHttpClient.Get<BindResponse>(XDG_BIND_LIST);
            return response.Binds;
        }

        public static async Task Bind(LoginType loginType) {
            if (loginType == LoginType.Phone)
            {
                AliyunTrack.ResetPhoneLoginData();
                await PhoneModule.Bind();
                return;
            }
            Dictionary<string, object> authData = await GetLoginAuthData(loginType);
            await XDHttpClient.Post<BaseResponse>(XDG_BIND_INTERFACE, data: authData);
        }

        public static Task Unbind(XD.SDK.Account.LoginType loginType, Dictionary<string, object> extras = null) {
            if (loginType == LoginType.Phone) {
                AliyunTrack.ResetPhoneLoginData();
                // 手机号解绑需要验证码
                return PhoneModule.Unbind(extras);
            }
            Dictionary<string, object> data = new Dictionary<string, object>() {
                { "type", (int)loginType }
            };
            return XDHttpClient.Post<BaseResponse>(XDG_UNBIND_INTERFACE, data: data);
        }

        public static async Task<XDGUser> Login(XD.SDK.Account.LoginType loginType) {
            var localUser = await GetLocalUser();
            
            if (loginType == XD.SDK.Account.LoginType.Default) {
                if (localUser == null) {
                    XDLogger.Error("缺少本地登录信息");
                    throw XDException.MSG(XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString().LoginFailed);
                }
                
                localUser = await FetchUserInfo();
                
                _ = AccessTokenModule.Refresh();
                return localUser;
            }
            
            try {
                AliyunTrack.LoginEnableAuthorizeTrack();
                // 单独处理手机号登录
                if (loginType == LoginType.Phone)
                {
                    AliyunTrack.ResetPhoneLoginData();
                    AliyunTrack.LoginEnableSDKLoginTrack();
                    await PhoneModule.Login();
                    UIManager.ShowLoading();
                } else {
                    Dictionary<string, object> loginParams = await GetLoginAuthData(loginType);
                    UIManager.ShowLoading();
                    Dictionary<string, object> cacheData = GetCacheData(loginType);
                    await AccessTokenModule.Login(loginParams, cacheData);
                }
                AliyunTrack.LoginEnableSDKLoginTrack();
                var user = await FetchUserInfo();
                await SyncTDSUser(user.userId);
                XDLogger.Debug(string.Format($"xd userID: {user.userId}"));
                return user;
            } catch (XDException e) {
                UIManager.DismissLoading();
                if (e.extra_data != null) {
                    LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
                    if (e.code == 40021 || e.code == 40901 || e.code == 40902) {
                        string title = null;
                        string content = null;
                        string lt = e.extra_data["loginType"] as string;
                        string email = e.extra_data["email"] as string;
                        if (e.code == 40021) {
                            title = localizableString.EmailNotVerified;
                            content = string.Format(localizableString.LoginEmailNotVerified, lt, email, lt);
                        } else if (e.code == 40901) {
                            title = localizableString.AccountAlreadyExists;
                            string conflicts = string.Join("/", (e.extra_data["conflicts"] as List<object>).Select(item => (item as Dictionary<string, object>)["loginType"]));
                            content = string.Format(localizableString.LoginEmailConflict, lt, email, conflicts);
                        } else if (e.code == 40902) {
                            title = localizableString.AccountAlreadyExists;
                            string conflicts = string.Join("/", (e.extra_data["conflicts"] as List<object>).Select(item => (item as Dictionary<string, object>)["loginType"]));
                            content = string.Format(localizableString.BindEmailConflict, lt, email, lt, conflicts);
                        }
                        Dictionary<string, object> conf = new Dictionary<string, object> {
                            { ConfirmDialog.TITLE_KEY, title },
                            { ConfirmDialog.CONTENT_KEY, content },
                            { ConfirmDialog.CONFIRM_TEXT_KEY, localizableString.IUnderstand }
                        };

                        TaskCompletionSource<XDUser> tcs = new TaskCompletionSource<XDUser>();
                        UIManager.ShowUI<ConfirmDialog>(conf, (code, data) => {
                            UIManager.Dismiss();
                            e.extra_data = null;
                            tcs.TrySetException(e);
                        });
                        return await tcs.Task;
                    } else {
                        throw e;
                    }
                } else {
                    throw e;
                }
            } finally {
                XD.SDK.Common.PC.Internal.UIManager.DismissLoading();
            }
        }

        internal static async Task<XDGUser> LoginByConsole() {
            // 判断当前有接入哪个主机 SDK
            if (SteamUtils.IsSDKSupported) {
                try {
                    AliyunTrack.LoginStart("Default_Steam", "Steam");
                    XD.SDK.Common.PC.Internal.UIManager.ShowLoading();

                    AccessToken localToken = await AccessTokenModule.GetLocalAccessToken();
                    string steamId = SteamAuth.GetSteamIdFromSDK();
                    XDGUser user;
                    if (localToken?.SteamId == steamId) {
                        try {
                            user = await FetchUserInfo();
                            
                            // 同自动登录一样，触发刷新 token
                            _ = AccessTokenModule.Refresh();
                        } catch (XDException e) {
                            if (e.code == ResponseCode.TOKEN_EXPIRED) {
                                // Token 过期
                                AliyunTrack.LoginEnableSlientAuthorize();
                                user = await LoginByConsoleInternal();
                            } else {
                                throw e;
                            }
                        }
                    } else {
                        // 当前 steamId 与缓存 steamId 不一致，则使用当前 steam 信息重新授权（相当于忽略本地缓存）
                        user = await LoginByConsoleInternal();
                    }
                    await SyncTDSUser(user.userId);
                    XDLogger.Debug(string.Format($"xd userID: {user.userId}"));
                    return user;
                } finally {
                    UIManager.DismissLoading();
                }
            }

            // 尝试其他主机 SDK
            throw new NotImplementedException("NO supported console sdk.");
        }

        internal static async Task<XDGUser> LoginByConsoleInternal() {
            AliyunTrack.LoginEnableAuthorizeTrack();
            Dictionary<string, object> authData = await SteamAuth.GetAuthDataFromSDK();
            Dictionary<string, object> cacheData = SteamAuth.GetCacheData();
            AliyunTrack.LoginEnableSDKLoginTrack();
            await AccessTokenModule.SignIn(authData, cacheData);
            return await FetchUserInfo();
        }

        internal static async Task<Dictionary<string, object>> GetLoginAuthData(XD.SDK.Account.LoginType loginType) {
            switch (loginType) {
                case XD.SDK.Account.LoginType.Guest:
                    return GuestAuth.GetAuthData();
                case XD.SDK.Account.LoginType.TapTap:
                    return await TapAuth.GetAuthData();
                case XD.SDK.Account.LoginType.Apple:
                    return await AppleAuth.GetAuthData();
                case XD.SDK.Account.LoginType.Google:
                    return await GoogleAuth.GetAuthData();
                case XD.SDK.Account.LoginType.Steam:
                    return await SteamAuth.GetAuthData();
                case LoginType.Facebook:
                    return await FacebookAuth.GetAuthData();
                default:
                    return new Dictionary<string, object>();
            }
        }

        internal static Dictionary<string, object> GetCacheData(XD.SDK.Account.LoginType loginType) {
            switch (loginType) {
                case XD.SDK.Account.LoginType.Steam:
                    return SteamAuth.GetCacheData();
                default:
                    return null;
            }
        }

        private static async Task SyncTDSUser(string userId) {
            // 效果等于 BecomeWithSessionToken,为了少一次网络请求
            SyncTokenResponse tokenResponse = await XDHttpClient.Post<SyncTokenResponse>(XDG_LOGIN_SYN);
            TDSUser user = TDSUser.CreateWithoutData(TDSUser.CLASS_NAME, userId) as TDSUser;
            user.SessionToken = tokenResponse.SyncToken.SessionToken;
            await user.SaveToLocal();
        }

        public static async Task<XDGUser> FetchUserInfo() {
            try
            {
                ProfileResponse response = await XDHttpClient.Get<ProfileResponse>(XDG_USER_PROFILE);

                current = response.User as XDUser;
                await persistence.Save(current);
                AliyunTrack.LoginPreSuccess();
                return current;
            }
            catch (XDException xdException)
            {
                var errorMsg = xdException.Message;
                // 所有 40801 错误代表 401 授权失败,需要明示原因为 XD_TOKEN_EXPIRED
                if (xdException.code == ResponseCode.TOKEN_EXPIRED || xdException.code == 401)
                    errorMsg = "XD_TOKEN_EXPIRED";
                AliyunTrack.LoginPreFail(errorMsg);
                throw xdException;
            }catch (Exception e) {
                AliyunTrack.LoginPreFail(e.Message);
                throw e;
            }
        }
    }
}
#endif