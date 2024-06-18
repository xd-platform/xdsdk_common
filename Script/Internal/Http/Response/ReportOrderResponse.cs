using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class ReportOrderResult {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
    }

    public class ReportOrderResponse : BaseResponse {
        [JsonProperty("data")]
        public ReportOrderResult Result { get; set; }
    }
}
