#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TapTap.Common;
using TapTap.Login;
using XD.SDK.Account;
using TapAccessToken = TapTap.Login.AccessToken;

namespace XD.SDK.Common.PC.Internal {
    internal class TapAuth {
        internal static async Task<Dictionary<string, object>> GetAuthData() {
            TapAccessToken tapAccessToken = await RequestTapToken();
            return new Dictionary<string, object> {
                { "type", (int) XD.SDK.Account.LoginType.TapTap },
                { "token", tapAccessToken.accessToken },
                { "secret", tapAccessToken.macKey },
                { "scope", string.Join(",", ConfigModule.TapConfig.Permissions?.ToArray()) }
            };
        }

        internal static async Task<TapAccessToken> RequestTapToken() {
            try
            {
                AliyunTrack.LoginAuthorize();
                TapAccessToken accessToken = await TapLogin.Login(ConfigModule.TapConfig.Permissions?.ToArray());
                AliyunTrack.LoginAuthorizeSuccess();
                return accessToken;
            } catch (TapException e) {
                if (e.code == (int)TapErrorCode.ERROR_CODE_LOGIN_CANCEL) {
                    XDLogger.Debug($"Tap 取消登录，{e}");
                    AliyunTrack.LoginAuthorizeFail(e.Message);
                } else {
                    XDLogger.Error($"Tap 登录失败：{e}");
                }
                throw new XDException(e.code, e.Message);
            } catch (Exception e) {
                AliyunTrack.LoginAuthorizeFail(e.Message);
                XDLogger.Error($"Tap 登录失败：{e}");
                throw XDException.MSG(e.Message);
            }
        }
    }
}
#endif