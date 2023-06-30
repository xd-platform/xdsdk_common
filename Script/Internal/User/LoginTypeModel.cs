#if UNITY_EDITOR || UNITY_STANDALONE
using LC.Newtonsoft.Json;
using XD.SDK.Account;
using XD.SDK.Common.PC;

namespace XD.SDK.Common.PC.Internal {
    public class LoginTypeModel {
        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("typeValue")]
        public int TypeValue{ get; set; }

        [JsonProperty("type")]
        public XD.SDK.Account.LoginType Type { get; set; }
        
        public LoginTypeModel(XD.SDK.Account.LoginType type){ 
            Type = type;
            TypeValue = (int)type;
            TypeName = GetName(type);
        }

        public static string GetName(XD.SDK.Account.LoginType type){
            switch (type) {
                case XD.SDK.Account.LoginType.Guest:
                    return "Guest";
                case XD.SDK.Account.LoginType.Apple:
                    return "Apple";
                case XD.SDK.Account.LoginType.Google:
                    return "Google";
                case XD.SDK.Account.LoginType.TapTap:
                    return "TapTap";
                case XD.SDK.Account.LoginType.Steam:
                    return "Steam";
                case LoginType.Phone:
                    return "Phone";
                default:
                    return "";
            }
        }

        public static string GetReadableName(LoginType type) {
            if (type == LoginType.Phone) {
                return "手机";
            }
            return GetName(type);
        }
    }
}
#endif