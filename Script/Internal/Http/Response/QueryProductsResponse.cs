using System.Collections.Generic;
using LC.Newtonsoft.Json;
using XD.SDK.Payment;

namespace XD.SDK.Common.PC.Internal {
    public class QueryProductsResponse : BaseResponse {
        [JsonProperty("data")]
        public List<SkuDetailBean> Products { get; set; }
    }
}
