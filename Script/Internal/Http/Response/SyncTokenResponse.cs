#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class SyncTokenResponse : BaseResponse {
        [JsonProperty("data")]
        public SyncToken SyncToken { get; private set; }
    }

    public class SyncToken {
        [JsonProperty("sessionToken")]
        public string SessionToken { get; private set; }
    }
}
#endif