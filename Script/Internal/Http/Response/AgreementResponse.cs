#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class AgreementResponse : BaseResponse {
        [JsonProperty("data")]
        public Agreement Agreement { get; set; }
    }

    public class AgreementConfirm {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("agreementVersion")]
        public string Version { get; set; }
    }

    public class AgreementConfirmResponse : BaseResponse {
        [JsonProperty("data")]
        public AgreementConfirm Confirm { get; set; }
    }
}
#endif