using System;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class CreateOrderResult {
        [JsonProperty("tradeNo")]
        public ulong TradeNo { get; set; }

        [JsonProperty("paymentType")]
        public int PaymentType { get; set; }

        [JsonProperty("outTradeNo")]
        public string OutTradeNo { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }

        [JsonProperty("requestTime")]
        public DateTime RequestTime { get; set; }

        [JsonProperty("orderStatus")]
        public int OrderStatus { get; set; }

        [JsonProperty("payUrl")]
        public string PayUrl { get; set; }

        [JsonProperty("applinkUrl")]
        public string ApplinkUrl { get; set; }

        [JsonProperty("schemeUrl")]
        public string SchemeUrl { get; set; }

        [JsonProperty("paymentEnvCode")]
        public int PaymentEnvCode { get; set; }

        [JsonProperty("skuName")]
        public string SkuName { get; set; }
    }

    public class CreateOrderResponse : BaseResponse {
        [JsonProperty("data")]
        public CreateOrderResult Result { get; set; }
    }
}