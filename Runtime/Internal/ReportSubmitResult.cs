using LC.Newtonsoft.Json;
using XD.SDK.Common.Internal;

namespace XD.SDK.Report.Internal
{
    public class ReportSubmitResult : BaseResponse
    {
        [JsonProperty("data")]
        public ReportSubmitBean ReportSubmitData;

        public class ReportSubmitBean
        {
            [JsonProperty("success")] public bool Success { get; set; }
        }
    }
}