#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class BindEntryConfig {
        [JsonProperty("canBind")]
        public int CanBind { get; set; }

        [JsonProperty("entryName")]
        public string EntryName { get; set; }

        [JsonProperty("canUnbind")]
        public int CanUnbind { get; set; }
    }

    public class Config : AppConfig {
        [JsonProperty("region")]
        public string Region { get; private set; }

        [JsonProperty("bindEntriesConfig")]
        public List<BindEntryConfig> BindEntryConfigs { get; set; }

        [JsonProperty("gameName")]
        public string GameName2 { set { GameName = value; } }

        [JsonProperty("tapSdkConfig")]
        public TapConfig TapConfig2 { set { TapConfig = value; } }

        [JsonProperty("appId")]
        public string AppId2 { set { AppId = value; } }

        [JsonProperty("webPayUrl")]
        public string WebPayUrl2 { set { WebPayUrl = value; } }

        [JsonProperty("webPayUrlForPC")]
        public string WebPayUrl2ForPC { set { WebPayUrlForPC = value; } }

        [JsonProperty("reportUrl")]
        public string ReportUrl2 { set { ReportUrl = value; } }

        [JsonProperty("logoutUrl")]
        public string CancelUrl2 { set { CancelUrl = value; } }
    }
}
#endif