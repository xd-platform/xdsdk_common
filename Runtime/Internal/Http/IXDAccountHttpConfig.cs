using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace XD.SDK.Account.Internal {
    public interface IXDAccountHttpConfig {
        Task<Dictionary<string, string>> GetAuthorizationHeaders(HttpRequestMessage request, long timestamp);
    }
}
