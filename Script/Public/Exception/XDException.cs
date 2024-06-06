#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using LC.Newtonsoft.Json;
using UnityEngine;
using XD.SDK.Common.PC;

namespace XD.SDK.Common.PC.Internal {
    
    public class XDException : Exception, XDGError
    {
        public readonly static int DEFAULT_CODE = -1001;
        
        [JsonIgnore]
        public int code { get; private set; }
        [JsonIgnore]
        public string error_msg => base.Message;
        [JsonIgnore]
        public Dictionary<string, object> extra_data { get; internal set; }

        public string ToJSON()
        {
            return JsonUtility.ToJson(this);
        }

        public XDException(int code, string message) :
            base(message == null ? string.Empty : message) {
            this.code = code;
        }

        public XDException(int code, string message, Dictionary<string, object> extraData) :
            this(code, message) {
            extra_data = extraData;
        }

        public override string ToString() {
            return $"{code} - {base.Message}";
        }

        public static XDException MSG(string message) {
            return new XDException(DEFAULT_CODE, message);
        }

        
    }
}
#endif