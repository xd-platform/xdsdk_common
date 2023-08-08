using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using System.Collections;
using System.Linq;
using XD.SDK.Account;
using XD.SDK.Account.Internal;
using XD.SDK.Common;
using XD.SDK.Common.Internal;

namespace XD.SDK.Account
{
    // The user's bound accounts. eg.@[@"TAPTAP",@"GOOGLE",@"FACEBOOK"]
    [Serializable]
    public class XDGUserMobile : XDGUser
    {
        // The user's user ID.
        public string _userId;

        // The user’s user ID in string.
        public string _sub;

        // The user's user name.
        public string _name;

        // The user's current loginType.
        public string _loginType; //App传来的是字符串，如 TapTap。 通过 GetLoginType() 方法获取枚举

        public string _avatar;

        public string _nickName;

        public List<string> _boundAccounts;

        // The user's token.
        public XDGAccessToken _token;

        public XDGUserMobile(string json)
        {
            Dictionary<string, object> dic = Json.Deserialize(json) as Dictionary<string, object>;
            this._userId = SafeDictionary.GetValue<string>(dic, "userId");
            this._sub = SafeDictionary.GetValue<string>(dic, "sub");
            this._name = SafeDictionary.GetValue<string>(dic, "name");
            this._loginType = SafeDictionary.GetValue<string>(dic, "loginType");
            this._avatar = SafeDictionary.GetValue<string>(dic, "avatar");
            this._nickName = SafeDictionary.GetValue<string>(dic, "nickName");
            this._boundAccounts = SafeDictionary.GetValue<List<object>>(dic, "boundAccounts")?.Cast<string>().ToList();
            this._token = new XDGAccessTokenMobile(SafeDictionary.GetValue<Dictionary<string, object>>(dic, "token"));
        }

        public XDGUserMobile(Dictionary<string, object> dic)
        {
            this._userId = SafeDictionary.GetValue<string>(dic, "userId");
            this._sub = SafeDictionary.GetValue<string>(dic, "sub");
            this._name = SafeDictionary.GetValue<string>(dic, "name");
            this._loginType = SafeDictionary.GetValue<string>(dic, "loginType");
            this._avatar = SafeDictionary.GetValue<string>(dic, "avatar");
            this._nickName = SafeDictionary.GetValue<string>(dic, "nickName");
            this._boundAccounts = SafeDictionary.GetValue<List<object>>(dic, "boundAccounts")?.Cast<string>().ToList();
            this._token = new XDGAccessTokenMobile(SafeDictionary.GetValue<Dictionary<string, object>>(dic, "token"));

            XDGTool.Log($"打印UserId: {_userId}");
        }

        public XD.SDK.Account.LoginType GetLoginType()
        {
            var strType = this._loginType.ToLower();
            switch (strType)
            {
                case "taptap":
                    return XD.SDK.Account.LoginType.TapTap;
                case "google":
                    return XD.SDK.Account.LoginType.Google;
                case "facebook":
                    return XD.SDK.Account.LoginType.Facebook;
                case "apple":
                    return XD.SDK.Account.LoginType.Apple;
                case "line":
                    return XD.SDK.Account.LoginType.LINE;
                case "twitter":
                    return XD.SDK.Account.LoginType.Twitter;
                // case "qq":
                //     return XD.SDK.Account.LoginType.QQ;
                // case "twitch":
                //     return XD.SDK.Account.LoginType.Twitch;
                case "steam":
                    return XD.SDK.Account.LoginType.Steam;
                case "guest":
                    return XD.SDK.Account.LoginType.Guest;
                case "phone":
                    return XD.SDK.Account.LoginType.Phone;
            }

            return XD.SDK.Account.LoginType.Default;
        }

        public static string GetLoginTypeString(LoginType loginType)
        {
            switch (loginType)
            {
                case XD.SDK.Account.LoginType.TapTap:
                    return "TapTap";
                case XD.SDK.Account.LoginType.Google:
                    return "Google";
                case XD.SDK.Account.LoginType.Facebook:
                    return "Facebook";
                case XD.SDK.Account.LoginType.Apple:
                    return "Apple";
                case XD.SDK.Account.LoginType.LINE:
                    return "LINE";
                case XD.SDK.Account.LoginType.Twitter:
                    return "Twitter";
                // case XD.SDK.Account.LoginType.QQ:
                //     return "QQ";
                // case XD.SDK.Account.LoginType.Twitch:
                //     return "Twitch";
                case XD.SDK.Account.LoginType.Steam:
                    return "Steam";
                case XD.SDK.Account.LoginType.Guest:
                    return "Guest";
                case XD.SDK.Account.LoginType.Phone:
                    return "Phone";
                default:
                    return "Default";
            }
        }

        public string userId => _userId;
        public string name => _name;
        public string sub => _sub;
        public string loginType => GetLoginTypeString(getLoginType());
        public string avatar => _avatar;
        public string nickName => _nickName;
        public List<string> boundAccounts => _boundAccounts;
        public XDGAccessToken token => _token;
        public LoginType getLoginType()
        {
            return (LoginType)GetLoginType();
        }
    }

    [Serializable]
    public class XDGUserWrapper
    {
        public XDGUser user;
        public XDGError error;

        public XDGUserWrapper(string json)
        {
            Dictionary<string, object> contentDic = Json.Deserialize(json) as Dictionary<string, object>;
            Dictionary<string, object>
                userDic = SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "user");
            Dictionary<string, object> errorDic =
                SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "error");

            if (userDic != null)
            {
                this.user = new XDGUserMobile(userDic);
            }

            if (errorDic != null)
            {
                this.error = new XDGErrorMobile(errorDic);
            }
        }
    }

    [Serializable]
    public class XDGAccessTokenMobile : XDGAccessToken
    {
        // 唯一标志
        private string _kid;

        // 认证码类型
        private string _tokenType;

        // mac密钥
        private string _macKey;

        // mac密钥计算方式
        private string _macAlgorithm;

        public XDGAccessTokenMobile(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            this._kid = SafeDictionary.GetValue<string>(dic, "kid");
            this._tokenType = SafeDictionary.GetValue<string>(dic, "tokenType");
            this._macKey = SafeDictionary.GetValue<string>(dic, "macKey");
            this._macAlgorithm = SafeDictionary.GetValue<string>(dic, "macAlgorithm");
        }

        public string kid => _kid;
        public string tokenType => _tokenType;
        public string macKey => _macKey;
        public string macAlgorithm => _macAlgorithm;
    }

    public enum LoginType
    {
        Guest = 0,    // 游客登录
        WeChat = 1,    // PC 未实现
        Apple = 2,    // 苹果登录
        Google = 3,    // Google 登录
        Facebook = 4,    // PC 未实现
        TapTap = 5,    // Tap 登录
        LINE = 6,    // PC 未实现
        Twitter = 7,    // PC 未实现
        // QQ       = 8,
        // Twitch   = 9,
        Steam = 10,   // Steam 登录
        Phone = 11,   // 手机号 登陆
        Default = -1,   // 自动登录，以上次登录成功的信息登录
    }
    // ios的，要与后台一致！
    // XDGLoginInfoTypeGuest = 0,
    // XDGLoginInfoTypeWeChat = 1,
    // XDGLoginInfoTypeApple = 2,
    // XDGLoginInfoTypeGoogle = 3,
    // XDGLoginInfoTypeFacebook = 4,
    // XDGLoginInfoTypeTapTap = 5,
    // XDGLoginInfoTypeLine = 6,
    // XDGLoginInfoTypeTwitter = 7
}