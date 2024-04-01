#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class Bind {
        [JsonProperty("loginType")]
        public int LoginType { get; set; }

        [JsonProperty("loginName")]
        public string LoginName { get; set; }

        [JsonProperty("nickName")]
        public string NickName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("bindDate")]
        public string BindDate { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("openId")]
        public string OpenId { get; set; }
    }
}
#endif