#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class Agreement {
        [JsonProperty("agreementRegion")]
        public string Region { get; set; }

        [JsonProperty("agreementVersion")]
        public string Version { get; set; }

        [JsonProperty("agreementUrl")]
        public string Url { get; set; }

        [JsonProperty("isKRPushServiceSwitchEnable")]
        public bool IsKRPushServiceSwitchEnable { get; set; }

        [JsonProperty("dataCollectionAgreementUrl")]
        public string KRCollectionAgreementUrl { get; set; }
        
        [JsonProperty("userAgreement")]
        public string UserAgreement { get; set; }
        
        [JsonProperty("privacyPolicy")]
        public string PrivacyPolicy { get; set; }
        
        [JsonProperty("agreements")]
        public List<XDAgreementPC> SubAgreements { get; set; }

        [JsonIgnore]
        public Dictionary<string, object> Extra { get; set; }
    }
}
#endif