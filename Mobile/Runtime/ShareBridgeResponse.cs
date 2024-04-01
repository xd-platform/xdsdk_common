using LC.Newtonsoft.Json;

namespace XD.SDK.Share.Mobile {
    public class ShareBridgeResponse {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess => Code == 0;

        [JsonIgnore]
        public bool IsCancelled => Code == 60010;
    }
}