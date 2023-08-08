using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;

namespace XD.SDK.Common
{
    [Serializable]
    public class XDGUserStatusChangeWrapper
    {
        public int code;

        public string message;

        public XDGUserStatusChangeWrapper(string json)
        {
            Dictionary<string,object> dic = Json.Deserialize(json) as Dictionary<string,object>;
            this.code = SafeDictionary.GetValue<int>(dic,"code");
            this.message = SafeDictionary.GetValue<string>(dic,"message");
        }
    }
}