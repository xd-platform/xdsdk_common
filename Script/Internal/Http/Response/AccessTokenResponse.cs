#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Account.PC.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class AccessTokenResponse : BaseResponse {
        [JsonProperty("data")]
        public AccessToken AccessToken { get; private set; }
    }
}
#endif