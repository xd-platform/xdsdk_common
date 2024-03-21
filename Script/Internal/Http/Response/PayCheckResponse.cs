#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class PayCheckResult {
        [JsonProperty("list")]
        public List<PayInfo> List { get; set; }
    }

    public class PayCheckResponse : BaseResponse {
        [JsonProperty("data")]
        public PayCheckResult Result { get; set; }
    }
}
#endif