using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class RequestVerifyCodeData {
        [JsonProperty("resendTime")]
        public int ReSendInterval { get; set; }
    }

    public class RequestVerifyCodeResponse : BaseResponse {
        [JsonProperty("data")]
        public RequestVerifyCodeData Data { get; set; }
    }
}