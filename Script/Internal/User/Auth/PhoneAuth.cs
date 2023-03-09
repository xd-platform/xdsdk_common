#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XD.SDK.Common.PC.Internal {
    public class PhoneAuth {
        //internal static Task<object> Login() {
        //    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

        //    UIManager.ShowUI<PhoneVerificationAlert>("XDPhoneLoginAlert", null, async (code, data) => {
        //        PhoneVerificationAlert.VerificationData verificationData = data as PhoneVerificationAlert.VerificationData;
        //        try {
        //            UIManager.ShowLoading();
        //            Dictionary<string, object> authData = new Dictionary<string, object> {
        //                { "type", (int) Account.LoginType.Phone },
        //                { "token", verificationData.PhoneNumber },
        //                { "secret", verificationData.Code },
        //                { "subType", "text" }
        //            };
        //            await AccessTokenModule.Login(authData, null);
        //            UIManager.Dismiss();
        //            tcs.TrySetResult(null);
        //        } catch (XDException e) {
        //            if (e.code == 40114) {
        //                // 验证码错误
        //                verificationData.PhoneVerificationAlert.VerifyFailed();
        //            } else {
        //                tcs.TrySetException(e);
        //            }
        //        } catch (Exception e) {
        //            tcs.TrySetException(e);
        //        } finally {
        //            UIManager.DismissLoading();
        //        }
        //    });

        //    return tcs.Task;
        //}

    }
}
#endif