#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Account.Internal;
using XD.SDK.Account.PC.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class ProfileResponse : BaseResponse {
        [JsonProperty("data")]
        public XDUser User { get; set; }
    }
}
#endif