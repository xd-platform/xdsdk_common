using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;

namespace XD.SDK.Common{
    public class LocalConfigInfo{
        public TapSdkConfig tapSdkConfig;

        public LocalConfigInfo(Dictionary<string, object> dic){
            if (dic == null) return;
            tapSdkConfig = new TapSdkConfig(dic);
        }
    }

    public class TapSdkConfig{
        public int region; //0国内，1海外
        public string clientId;
        public string clientToken;
        public string serverUrl;
        public bool enableTapDB;
        public string channel;
        public string gameVersion;
        public bool idfa;

        public TapSdkConfig(Dictionary<string, object> dic){
            if (dic == null) return;
            region = SafeDictionary.GetValue<int>(dic, "region");
            clientId = SafeDictionary.GetValue<string>(dic, "clientId");
            clientToken = SafeDictionary.GetValue<string>(dic, "clientToken");
            serverUrl = SafeDictionary.GetValue<string>(dic, "serverURL");
            
            var dbDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "dbConfig");
            enableTapDB = SafeDictionary.GetValue<bool>(dbDic, "enableTapDB");
            channel = SafeDictionary.GetValue<string>(dbDic, "channel");
            gameVersion = SafeDictionary.GetValue<string>(dbDic, "gameVersion");
            idfa = SafeDictionary.GetValue<bool>(dbDic, "idfa");
        }
    }
}