#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Account.PC.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class RefreshTokenData {
        [JsonProperty("isUpdated")]
        public bool IsUpdated { get; set; }

        [JsonProperty("token")]
        public AccessToken AccessToken { get; set; }
    }

    public class RefreshTokenResponse : BaseResponse {
        [JsonProperty("data")]
        public RefreshTokenData Data { get; set; }
    }
}
#endif