using LC.Newtonsoft.Json;
using XD.SDK.Common.Internal;

namespace XD.SDK.Report.Internal
{
    public class SignedUrlResponse : BaseResponse
    {
        [JsonProperty("data")] public SignedUrlBean SignedUrlData { get; set; }

        public class SignedUrlBean
        {
            [JsonProperty("signedUrl")] public string SignedUrl { get; set; }
            [JsonProperty("timeOutSec")] public int TimeOutSec { get; set; }
        }
    }
}