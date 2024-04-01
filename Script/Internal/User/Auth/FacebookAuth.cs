#if UNITY_EDITOR || UNITY_STANDALONE

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace XD.SDK.Common.PC.Internal {
    public class FacebookAuth : WebAuth {
        static List<HttpListener> servers = new List<HttpListener>();

        internal static Task<Dictionary<string, object>> GetAuthData() {
            Dictionary<string, object> queryParams = new Dictionary<string, object> {
                { "auth_type", "facebook" },
                { "client_id", ConfigModule.Facebook.AppId },
                { "state", LocalServerUtils.GenerateRandomDataBase64url(32) }
            };

            return StartListener(servers, Account.LoginType.Facebook, queryParams);
        }
    }
}

#endif