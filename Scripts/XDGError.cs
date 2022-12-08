using System;
using System.Collections.Generic;
using System.Linq;
using LC.Newtonsoft.Json;
using LC.Newtonsoft.Json.Serialization;
using TapTap.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XD.SDK.Common{
    [Serializable]
    public class XDGError{
        public int code; //40021: 邮箱未做验证, 40902: 邮箱冲突，第三方登录方式绑定邮箱关联账号已存在当前登录方式的另一个账号绑定。这两个code游戏需要弹框提示！！
        public string error_msg;
        public Dictionary<string, object> extra_data; //参考： https://stg-xdsdk.ap-sg.tdsapps.com/docs/account/account-server#taptap-%E7%BB%91%E5%AE%9A%E9%82%AE%E7%AE%B1%E6%9C%AA%E9%AA%8C%E8%AF%81

        public XDGError(Dictionary<string, object> dic){
            if (dic != null){
                code = SafeDictionary.GetValue<int>(dic, "code");
                error_msg = SafeDictionary.GetValue<string>(dic, "error_msg");
                if (string.IsNullOrEmpty(error_msg)){
                    error_msg = SafeDictionary.GetValue<string>(dic, "errorMsg");
                }

                if (dic.ContainsKey(@"extra_data")){
                    var dataObj = dic["extra_data"]; //有可能空字符串
                    if (dataObj != null && dataObj.GetType() == typeof(Dictionary<string, object>)){
                        extra_data = dataObj as Dictionary<string, object>;
                    }
                }
            }
        }

        public XDGError(int code, string errorMsg){
            this.code = code;
            this.error_msg = errorMsg;
        }

        public string ToJSON(){
            return JsonUtility.ToJson(this);
        }
    }
}