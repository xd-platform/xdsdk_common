#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class TapDBConfig
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }
        
        [JsonProperty("channel")]
        public string Channel { get; set; }
        
        [JsonProperty("game_version")]
        public string Version { get; set; }
    }
    
    public class TapConfig {
        
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("client_id")]
        public string ClientId2 { set { ClientId = value; } }

        [JsonProperty("clientToken")]
        public string ClientToken { get; set; }

        [JsonProperty("client_token")]
        public string ClientToken2 { set { ClientToken = value; } }

        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }

        [JsonProperty("server_url")]
        public string ServerUrl2 { set { ServerUrl = value; } }

        [JsonProperty("tapDBChannel")]
        public string TapDBChannel { get; set; }
        
        [JsonProperty("db_config")]
        public TapDBConfig DBConfig { get; set; }

        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; }
    }
}
#endif