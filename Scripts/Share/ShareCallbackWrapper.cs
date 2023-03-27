using System;
using System.Collections.Generic;
using TapTap.Common;
namespace XD.SDK.Share
{
    public class ShareCallbackWrapper
    {
        public bool isError {get;}
        public int errorCode {get;}

        public string errorMsg {get;}

        public bool isSuccess {get;}
         public ShareCallbackWrapper(string json)
        {
            var dic = Json.Deserialize(json) as Dictionary<string, object>;
            var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "error");
            if (errorDic != null)
            {
                isError = true;
                errorCode = SafeDictionary.GetValue<int>(errorDic, "code");
                errorMsg = SafeDictionary.GetValue<string>(errorDic, "errorMsg");
            }else{
                isSuccess = SafeDictionary.GetValue<bool>(dic, "success");
            }
           
        }
    }
}