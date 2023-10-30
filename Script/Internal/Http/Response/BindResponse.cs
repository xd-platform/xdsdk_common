#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class BindResponse : BaseResponse {
        [JsonProperty("data")]
        public List<Bind> Binds { get; set; }
    }
}
#endif