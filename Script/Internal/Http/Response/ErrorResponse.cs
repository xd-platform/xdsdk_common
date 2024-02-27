#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class ErrorResponse : BaseResponse {
        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; private set; }
    }
}
#endif