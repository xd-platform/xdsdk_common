using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace XD.SDK.Common
{
// The user's bound accounts. eg.@[@"TAPTAP",@"GOOGLE",@"FACEBOOK"]
    [Serializable]
    public class XDGUser{
        // The user's user ID.
        public string userId;

        // The user’s user ID in string.
        public string sub;

        // The user's user name.
        public string name;

        // The user's current loginType.
        public string loginType; //App传来的是字符串，如 TapTap。 通过 GetLoginType() 方法获取枚举

        public string avatar;

        public string nickName;

        public List<string> boundAccounts;

        // The user's token.
        public XDGAccessToken token;

        public XDGUser(string json){
                Dictionary<string,object> dic = Json.Deserialize(json) as Dictionary<string,object>;
                this.userId = SafeDictionary.GetValue<string>(dic, "userId");
                this.sub = SafeDictionary.GetValue<string>(dic, "sub");
                this.name = SafeDictionary.GetValue<string>(dic, "name");
                this.loginType = SafeDictionary.GetValue<string>(dic, "loginType");
                this.avatar = SafeDictionary.GetValue<string>(dic, "avatar");
                this.nickName = SafeDictionary.GetValue<string>(dic, "nickName");
                boundAccounts = SafeDictionary.GetValue<List<object>>(dic, "boundAccounts").Cast<string>().ToList();
                this.token = new XDGAccessToken(SafeDictionary.GetValue<Dictionary<string, object>>(dic, "token"));
        }
        
        public XDGUser(Dictionary<string,object> dic){   
            this.userId = SafeDictionary.GetValue<string>(dic,"userId");
            this.sub = SafeDictionary.GetValue<string>(dic,"sub");
            this.name = SafeDictionary.GetValue<string>(dic,"name");
            this.loginType = SafeDictionary.GetValue<string>(dic, "loginType");
            this.avatar = SafeDictionary.GetValue<string>(dic, "avatar");
            this.nickName = SafeDictionary.GetValue<string>(dic, "nickName");
            boundAccounts = SafeDictionary.GetValue<List<object>>(dic, "boundAccounts").Cast<string>().ToList();
            this.token  = new XDGAccessToken(SafeDictionary.GetValue<Dictionary<string,object>>(dic,"token"));
            
            XDGTool.Log($"打印UserId: {userId}");
        }

        public LoginType GetLoginType(){
            var strType = this.loginType.ToLower();
            switch (strType){
                case "taptap":
                    return LoginType.TapTap;
                case "google":
                    return LoginType.Google;
                case "facebook":
                    return LoginType.Facebook;
                case "apple":
                    return LoginType.Apple;
                case "line":
                    return LoginType.LINE;
                case "twitter":
                    return LoginType.Twitter;
                case "guest":
                    return LoginType.Guest;
            }

            return LoginType.Default;
        }

        public static string GetLoginTypeString(LoginType loginType){
            switch (loginType){
                case LoginType.TapTap:
                    return "TapTap";
                case LoginType.Google:
                    return "Google";
                case LoginType.Facebook:
                    return "Facebook";
                case LoginType.Apple:
                    return "Apple";
                case LoginType.LINE:
                    return "LINE";
                case LoginType.Twitter:
                    return "Twitter";
                case LoginType.Guest:
                    return "Guest";
                default:
                    return "Default";
            }
        }
    }
    
    [Serializable]
        public class XDGUserWrapper{
            public XDGUser user;
            public XDGError error;

            public XDGUserWrapper(string json){
                Dictionary<string, object> contentDic = Json.Deserialize(json) as Dictionary<string, object>;
                Dictionary<string, object>
                    userDic = SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "user");
                Dictionary<string, object> errorDic =
                    SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "error");

                if (userDic != null){
                    this.user = new XDGUser(userDic);
                }

                if (errorDic != null){
                    this.error = new XDGError(errorDic);
                }
            }
        }

        [Serializable]
        public class XDGAccessToken{
            // 唯一标志
            public string kid;

            // 认证码类型
            public string tokenType;

            // mac密钥
            public string macKey;

            // mac密钥计算方式
            public string macAlgorithm;

            public XDGAccessToken(Dictionary<string, object> dic){
                if (dic == null) return;
                this.kid = SafeDictionary.GetValue<string>(dic, "kid");
                this.tokenType = SafeDictionary.GetValue<string>(dic, "tokenType");
                this.macKey = SafeDictionary.GetValue<string>(dic, "macKey");
                this.macAlgorithm = SafeDictionary.GetValue<string>(dic, "macAlgorithm");
            }
        }

        public enum LoginType{
            Default,
            TapTap,
            Google,
            Facebook,
            Apple,
            LINE,
            Twitter,
            Guest
        }
    
}