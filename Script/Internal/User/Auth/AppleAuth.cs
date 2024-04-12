#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    internal class AppleAuth : WebAuth {
        static List<HttpListener> servers = new List<HttpListener>();

        internal static Task<Dictionary<string, object>> GetAuthData() {
            Dictionary<string, object> queryParams = new Dictionary<string, object> {
                { "auth_type", "apple" },
                { "client_id", ConfigModule.Apple.ClientId },
                { "state", LocalServerUtils.GenerateRandomDataBase64url(32) }
            };

            return StartListener(servers, XD.SDK.Account.LoginType.Apple, queryParams);
        }
    }
}
#endif