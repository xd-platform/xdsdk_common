using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;

namespace XD.SDK.Common
{
    [Serializable]
    public class XDGError
    {
        public int code;
        public string error_msg;

        public XDGError(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> dic))
            {
                code = GlobalUnKnowError.UN_KNOW;
                error_msg = "error parse string:" + jsonStr;
            }
            else
            {
                code = SafeDictionary.GetValue<int>(dic,"code");
                error_msg = SafeDictionary.GetValue<string>(dic,"error_msg");
                if (string.IsNullOrEmpty(error_msg)){
                    error_msg = SafeDictionary.GetValue<string>(dic,"errorMsg");
                }
            }
        }
        
        public XDGError(Dictionary<string,object> dic)
        {
            if (dic != null)
            {
                code = SafeDictionary.GetValue<int>(dic,"code");
                error_msg = SafeDictionary.GetValue<string>(dic,"error_msg");     
                if (string.IsNullOrEmpty(error_msg)){
                    error_msg = SafeDictionary.GetValue<string>(dic,"errorMsg");
                }
            }
        }
        
        public XDGError(int code,string errorMsg)
        {
            this.code = code;
            this.error_msg = errorMsg;
        }

        public string ToJSON(){
            return JsonUtility.ToJson(this);
        }
    }
}