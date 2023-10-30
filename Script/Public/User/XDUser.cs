#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using LC.Newtonsoft.Json;
using XD.SDK.Account;
using XD.SDK.Common.PC.Internal;

namespace XD.SDK.Account.PC.Internal {
    public class XDUser : XDGUser
    {
        public const int LOGIN_TYPE_GUEST = 0;
        public const int LOGIN_TYPE_APPLE = 2;
        public const int LOGIN_TYPE_GOOGLE = 3;
        public const int LOGIN_TYPE_FACEBOOK = 4;
        public const int LOGIN_TYPE_TAP = 5;
        public const int LOGIN_TYPE_STEAM = 10;
        public const int LOGIN_TYPE_PHONE = 11;

        [JsonProperty("userId")]
        public string userId { get; set; }

        [JsonProperty("sub")]
        public string sub { get; set;  }

        [JsonProperty("username")]
        public string name { get; set; }  // name

        [JsonProperty("nickName")]
        public string nickName { get; set; }
        
        [JsonProperty("loginType")]
        public long LoginTypeLong { get; set; }
        
        [JsonIgnore]
        public long loginTypeLong => LoginTypeLong;

        [JsonIgnore]
        public string loginType
        {
            get => XDGAccount.GetLoginTypeString(getLoginType());
        }

        [JsonProperty("avatar")]
        public string avatar { get; set; }
        
        [Obsolete]
        [JsonIgnore]
        public List<string> LoginList => boundAccounts;

        [JsonProperty("loginList")]
        public List<string> boundAccounts { get; set; }
        
        [JsonIgnore]
        public XDGAccessToken token => AccessTokenModule.local;
        
        public XD.SDK.Account.LoginType getLoginType()
        {
            switch (loginTypeLong) {
                case LOGIN_TYPE_GUEST:
                    return XD.SDK.Account.LoginType.Guest;
                case LOGIN_TYPE_APPLE:
                    return XD.SDK.Account.LoginType.Apple;
                case LOGIN_TYPE_GOOGLE:
                    return XD.SDK.Account.LoginType.Google;
                case LOGIN_TYPE_TAP:
                    return XD.SDK.Account.LoginType.TapTap;
                case LOGIN_TYPE_STEAM:
                    return XD.SDK.Account.LoginType.Steam;
                case LOGIN_TYPE_PHONE:
                    return LoginType.Phone;
                case LOGIN_TYPE_FACEBOOK:
                    return LoginType.Facebook;
                default:
                    return XD.SDK.Account.LoginType.Guest;
            }
        }
    }
}
#endif