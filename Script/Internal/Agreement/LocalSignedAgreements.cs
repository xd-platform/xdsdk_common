#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class LocalSignedAgreements {
        [JsonProperty("agreementVersions")]
        public Dictionary<string, string> AgreementVersions { get; set; }
    }
}
#endif