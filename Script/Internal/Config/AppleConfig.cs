#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class AppleConfig {
        [JsonProperty("service_id")]
        public string ClientId { get; internal set; }
    }
}
#endif