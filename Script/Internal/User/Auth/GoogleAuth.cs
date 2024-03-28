#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    internal class GoogleAuth : WebAuth {
        static List<HttpListener> servers = new List<HttpListener>();

        internal static Task<Dictionary<string, object>> GetAuthData() {
            Dictionary<string, object> queryParams = new Dictionary<string, object> {
                { "auth_type", "google" },
                { "client_id", ConfigModule.Google.ClientIdForPC },
                { "state", LocalServerUtils.GenerateRandomDataBase64url(32) }
            };

            return StartListener(servers, XD.SDK.Account.LoginType.Google, queryParams);
        }
    }
}
#endif