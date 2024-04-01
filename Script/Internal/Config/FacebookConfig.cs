#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class FacebookConfig {
        [JsonProperty("app_id")]
        public string AppId { get; internal set; }
    }
}
#endif