#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using UnityEngine;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    internal class GuestAuth {
        internal static Dictionary<string, object> GetAuthData() {
            AliyunTrack.LoginAuthorize();
            AliyunTrack.LoginAuthorizeSuccess();
            return new Dictionary<string, object>{
                { "type", (int) XD.SDK.Account.LoginType.Guest },
                { "token", SystemInfo.deviceUniqueIdentifier }
            };
        }
    }
}
#endif