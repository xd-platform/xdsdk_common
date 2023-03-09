#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Account.Internal;

namespace XD.SDK.Account.PC.Internal {
    public class AccessToken : XDGAccessToken
    {
        [JsonProperty("kid")]
        public string kid { get; set; }
        
        [JsonProperty("macKey")]
        public string macKey { get; set; }
        
        [JsonProperty("tokenType")]
        public string tokenType { get; set; }
        
        // TODO@PC 本来没有这个字段, mobile 有,不确定如何取值
        public string macAlgorithm { get; set; }

        [JsonProperty("expireIn")]
        public long ExpireIn { get; set; }

        [JsonProperty("steamId")]
        public string SteamId { get; set; }
    }
}
#endif