using System;
using System.Collections.Generic;
using System.Linq;
using LC.Newtonsoft.Json;
using LC.Newtonsoft.Json.Serialization;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common.Internal;
using Object = UnityEngine.Object;

namespace XD.SDK.Common{
    [Serializable]
    public class XDGErrorMobile : XDGError{
        public int _code; //40021: 邮箱未做验证, 40902: 邮箱冲突，第三方登录方式绑定邮箱关联账号已存在当前登录方式的另一个账号绑定。这两个code游戏需要弹框提示！！
        public string _error_msg;
        public Dictionary<string, object> _extra_data; //参考： https://stg-xdsdk.ap-sg.tdsapps.com/docs/account/account-server#taptap-%E7%BB%91%E5%AE%9A%E9%82%AE%E7%AE%B1%E6%9C%AA%E9%AA%8C%E8%AF%81

        public XDGErrorMobile(Dictionary<string, object> dic){
            if (dic != null){
                _code = SafeDictionary.GetValue<int>(dic, "code");
                _error_msg = SafeDictionary.GetValue<string>(dic, "error_msg");
                if (string.IsNullOrEmpty(_error_msg)){
                    _error_msg = SafeDictionary.GetValue<string>(dic, "errorMsg");
                }

                if (dic.ContainsKey(@"extra_data")){
                    var dataObj = dic["extra_data"]; //有可能空字符串
                    if (dataObj != null && dataObj.GetType() == typeof(Dictionary<string, object>)){
                        _extra_data = dataObj as Dictionary<string, object>;
                    }
                }
            }
        }

        public XDGErrorMobile(int code, string errorMsg){
            this._code = code;
            this._error_msg = errorMsg;
        }

        public string ToJSON(){
            return JsonUtility.ToJson(this);
        }

        public int code => _code;
        public string error_msg => _error_msg;
        public Dictionary<string, object> extra_data => _extra_data;
    }
}