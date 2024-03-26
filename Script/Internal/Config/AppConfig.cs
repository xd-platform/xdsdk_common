#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class AppConfig {
        [JsonProperty("client_id")]
        public string ClientId { get; internal set; }

        [JsonProperty("region_type")]
        public string RegionType { get; internal set; }

        [JsonProperty("game_name")]
        public string GameName { get; internal set; }

        [JsonProperty("app_id")]
        public string AppId { get; internal set; }

        [JsonProperty("tapsdk")]
        public TapConfig TapConfig { get; internal set; }

        [JsonProperty("web_pay_url")]
        public string WebPayUrl { get; internal set; }

        [JsonProperty("webpay_url_for_pc")]
        public string WebPayUrlForPC { get; internal set; }

        [JsonProperty("report_url")]
        public string ReportUrl { get; internal set; }

        [JsonProperty("logout_url")]
        public string CancelUrl { get; internal set; }

        [JsonProperty("google")]
        public GoogleConfig Google { get; internal set; }

        [JsonProperty("apple")]
        public AppleConfig Apple { get; internal set; }

        [JsonProperty("facebook")]
        public FacebookConfig Facebook { get; internal set; }

        /// <summary>
        /// 判断是否为海外
        /// </summary>
        internal bool IsGlobal => RegionType == "Global";
    }
}
#endif
