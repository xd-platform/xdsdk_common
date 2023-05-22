#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class GoogleConfig {
        [JsonProperty("CLIENT_ID")]
        public string ClientId { get; internal set; }

        [JsonProperty("CLIENT_ID_FOR_PC")]
        public string ClientIdForPC { get; internal set; }
    }
}
#endif
