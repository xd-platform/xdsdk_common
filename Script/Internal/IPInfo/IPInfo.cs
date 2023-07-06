#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Common.Internal;

namespace XD.SDK.Common.PC.Internal {
    
    public class IPInfoWrapper : XDGRegionInfoWrapper
    {
        [JsonIgnore] 
        public XDGRegionInfo info { get; private set; }
        
        public IPInfoWrapper(IPInfo info)
        {
            this.info = info;
        }
    }
    public class IPInfo : XDGRegionInfo
    {
        [JsonProperty("country_code")]
        public string countryCode { get; set; }
        
        [JsonProperty("city")]
        public string city { get; set; }
        
        [JsonProperty("src_ip")]
        public string locationInfoType { get; set; }
        
        [JsonProperty("timeZone")]
        public string timeZone { get; set; }
        
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

    }
}
#endif