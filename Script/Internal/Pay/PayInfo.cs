#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Payment;
using XD.SDK.Payment.PC;

namespace XD.SDK.Common.PC.Internal {
    public class PayInfo : XDGRefundDetails
    {
        [JsonProperty("tradeNo")]
        public string tradeNo { get; set; }
        
        [JsonProperty("outTradeNo")]
        public string outTradeNo { get; set; }
        
        [JsonProperty("productId")]
        public string productId { get; set; }
        
        [JsonProperty("currency")]
        public string currency { get; set; }
        
        [JsonProperty("refundAmount")]
        public double refundAmount { get; set; }
        
        [JsonProperty("supplyStatus")]
        public int supplyStatus { get; set; }
        
        [JsonProperty("platform")]
        public int platform { get; set; }  //1- iOS;2-Android;3-Web;4-macOS;5-Windows;6-unknown

        [JsonProperty("channelId")]
        public int ChannelId { get; set; }
        
        [JsonProperty("channelType")]
        public int channelType { get; set; }

        public bool IsIOS => platform == 1;

        public bool IsAndroid => platform == 2;
    }
}
#endif