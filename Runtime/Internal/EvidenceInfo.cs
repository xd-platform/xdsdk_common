using LC.Newtonsoft.Json;

namespace XD.SDK.Report.Internal
{
    public class EvidenceInfo
    {
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("uri")]
        public string Uri { get; }
        [JsonProperty("size")]
        public long Size { get; }

        public EvidenceInfo(string name, string uri, long size)
        {
            Name = name;
            Uri = uri;
            Size = size;
        }
    }
}