#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class ConfigData {
        [JsonProperty("configs")]
        public Config Config { get; private set; }
    }

    public class ConfigResponse : BaseResponse {
        [JsonProperty("data")]
        public ConfigData ConfigData { get; private set; }
    }
}
#endif