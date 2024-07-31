#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class Agreement {
        [JsonProperty("agreementRegion")]
        public string Region { get; set; }

        [JsonProperty("agreementVersion")]
        public string Version { get; set; }

        // deprecated 子协议内容
        [JsonProperty("agreements")]
        public List<XDAgreementPC> SubAgreements { get; set; }
        
        // 新协议首次签署字段
        [JsonProperty("titleFirst")]
        public string TitleFirst { get; set; }
        
        [JsonProperty("summaryFirst")]
        public string SummaryFirst { get; set; }
        
        [JsonProperty("promptFirst")]
        public string PromptFirst { get; set; }

        // 新协议更新字段
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("summary")]
        public string Summary { get; set; }
        
        [JsonProperty("prompt")]
        public string Prompt { get; set; }
        
        // 按钮文案
        [JsonProperty("agree")]
        public string Agree { get; set; }
        
        [JsonProperty("disagree")]
        public string Disagree { get; set; }
        
        // 勾选配置
        [JsonProperty("options")]
        public List<AgreementOption> Options { get; set; }
        
        // 韩国独有的三状态勾选
        [JsonProperty("agreeAll")]
        public string AgreeAll { get; set; }
        
        [JsonIgnore]
        public Dictionary<string, object> Extra { get; set; }
        
        public bool IsRegionKr()
        {
            return Region.ToLower().Equals("kr");
        }
        
        public bool IsRegionUs()
        {
            return Region.ToLower().Equals("us");
        }
    }

    public class AgreementOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("required")]
        public bool Required { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
#endif