using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Report.Internal
{
    public class ReportParamsInternal
    {
        [JsonProperty("reporter")]
        public UserInfo Reporter;
        [JsonProperty("reportee")]
        public UserInfo Reportee;
        [JsonProperty("reasons")]
        public List<ReasonParam> ReasonParams;
        [JsonProperty("user_description", NullValueHandling = NullValueHandling.Ignore)]
        public string UserDescription;
        [JsonProperty("evidence", NullValueHandling = NullValueHandling.Ignore)]
        public List<EvidenceInfo> Evidence;
        [JsonProperty("extras", NullValueHandling = NullValueHandling.Ignore)]
        public string Extras;

        public class UserInfo
        {
            [JsonProperty("xdid")]
            public string XdId { get; }
            [JsonProperty("extras", NullValueHandling = NullValueHandling.Ignore)]
            public string Extras { get; }

            public UserInfo(string xdId, string extras)
            {
                XdId = xdId;
                Extras = extras;
            }
        }

        public class ReasonParam
        {
            [JsonProperty("id")]
            public long ID;
            [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
            public string Title;
            [JsonProperty("extras", NullValueHandling = NullValueHandling.Ignore)]
            public string Extras;
        }
    }
}