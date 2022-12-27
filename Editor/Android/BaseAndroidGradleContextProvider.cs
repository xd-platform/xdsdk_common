#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.Editor
{
    [JsonObject]
    public class BaseAndroidGradleContextProvider
    {
        [JsonProperty]
        public virtual List<XDGAndroidGradleContext> AndroidGradleContext
        {
            get;
            set;
        }

        [JsonProperty]
        public virtual int Priority
        {
            get; 
            set;
        }

        [JsonProperty]
        public virtual string ModuleName
        {
            get; 
            set;
        }
    }
}
#endif