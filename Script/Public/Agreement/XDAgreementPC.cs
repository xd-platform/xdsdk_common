#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Common.PC;
using XD.SDK.Common.Internal;
namespace XD.SDK.Common.PC.Internal {
    
    public class XDAgreementPC : XDGAgreement
    {
        [JsonProperty("type")]
        public string type { get; internal set; }
        
        [JsonProperty("url")]
        public string url { get; internal set; }
        
        public override string ToString()
        {
            return $"[type:{type};url:{url}]";
        }
    }
}
#endif