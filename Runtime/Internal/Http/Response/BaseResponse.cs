using System.Net.Http.Headers;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.Internal {
    public class BaseResponse {
        [JsonProperty("code")]
        public int Code { get; private set; }

        [JsonProperty("msg")]
        public string Message { get; private set; }

        [JsonProperty("detail")]
        public string Detail { get; private set; }

        public HttpResponseHeaders Headers { get; set; }
    }
}
