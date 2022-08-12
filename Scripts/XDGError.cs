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
        public XDGExtraData extra_data;

        public XDGError(Dictionary<string, object> dic){
            if (dic != null){
                code = SafeDictionary.GetValue<int>(dic, "code");
                error_msg = SafeDictionary.GetValue<string>(dic, "error_msg");
                if (string.IsNullOrEmpty(error_msg)){
                    error_msg = SafeDictionary.GetValue<string>(dic, "errorMsg");
                }

                var dataDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "extra_data");
                if (dataDic != null){
                    var js = JsonConvert.SerializeObject(dataDic);
                    extra_data = JsonConvert.DeserializeObject<XDGExtraData>(js);
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