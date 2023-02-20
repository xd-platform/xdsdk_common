using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeanCloud.Storage;
using TapTap.Bootstrap;
using TapTap.Common;
using XD.SDK.Account.Internal;
using XD.SDK.Common;
using XD.SDK.Common.Internal;
using LoginType = XD.SDK.Account.LoginType;

namespace XD.SDK.Account{
    public class XDGAccountMobileImpl{
        private XDGAccountMobileImpl(){
            EngineBridge.GetInstance()
                .Register(XDGUnityBridge.ACCOUNT_SERVICE_NAME, XDGUnityBridge.ACCOUNT_SERVICE_IMPL);
        }

        private readonly string XDG_ACCOUNT_SERVICE = "XDGLoginService"; //注意要和iOS本地的桥接文件名一样！ 
        private static volatile XDGAccountMobileImpl _instance;
        private static readonly object Locker = new object();

        public static XDGAccountMobileImpl GetInstance(){
            lock (Locker){
                if (_instance == null){
                    _instance = new XDGAccountMobileImpl();
                }
            }

            return _instance;
        }

        public void Login(List<LoginType> loginTypes, Action<XDGUser> callback, Action<XDGError> errorCallback){
            var command = new Command.Builder()
                .Service(XDG_ACCOUNT_SERVICE)
                .Method("login")
                .Args("login", getLoginTypeStr(loginTypes))
                .Callback(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                try{
                    XDGTool.Log("Login 方法结果: " + result.ToJSON());
                    if (!XDGTool.checkResultSuccess(result)){
                        XDGTool.LogError($"Login 登录失败1 :{result.ToJSON()}");
                        errorCallback(new XDGErrorMobile(result.code, result.message));
                        return;
                    }

                    var userWrapper = new XDGUserWrapper(result.content);
                    if (userWrapper.error != null){
                        XDGTool.LogError($"Login 登录失败2 :{result.ToJSON()}");
                        errorCallback(userWrapper.error);
                        return;
                    }

                    if (userWrapper.user == null){
                        XDGTool.LogError($"Login 登录失败3 :{result.ToJSON()}");
                    }

                    ActiveLearnCloudToken(userWrapper.user, callback, errorCallback);
                } catch (Exception e){
                    errorCallback(new XDGErrorMobile(result.code, result.message));
                    XDGTool.LogError("Login 报错" + e.Message);
                    Console.WriteLine(e);
                }
            });
        }

        private static string getLoginTypeStr(List<LoginType> types){
            var str = "";
            foreach (var type in types){
                if (type == XD.SDK.Account.LoginType.TapTap){
                    str += "TAPTAP,";
                } else if (type == XD.SDK.Account.LoginType.Google){
                    str += "GOOGLE,";
                } else if (type == XD.SDK.Account.LoginType.Facebook){
                    str += "FACEBOOK,";
                } else if (type == XD.SDK.Account.LoginType.Apple){
                    str += "APPLE,";
                } else if (type == XD.SDK.Account.LoginType.LINE){
                    str += "LINE,";
                } else if (type == XD.SDK.Account.LoginType.Twitter){
                    str += "TWITTER,";
                } else if (type == XD.SDK.Account.LoginType.Guest){
                    str += "GUEST,";
                // } else if (type == XD.SDK.Account.LoginType.Twitch){
                //     str += "TWITCH,";
                } else if (type == XD.SDK.Account.LoginType.Steam){
                    str += "STEAM,";
                } else if (type == XD.SDK.Account.LoginType.Phone){
                    str += "PHONE,";
                // } else if (type == XD.SDK.Account.LoginType.QQ){
                //     str += "QQ,";
                }
            }

            return str;
        }

        private async void ActiveLearnCloudToken(XDGUser user, Action<XDGUser> callback,
            Action<XDGError> errorCallback){
            XDGTool.Log("LoginSync 开始执行  ActiveLearnCloudToken");

            if (user == null || XDGTool.IsEmpty(user.userId)){
                errorCallback(new XDGErrorMobile(-1001, "user is null"));
                XDGTool.LogError("LoginSync 报错：user 是空！");
                return;
            } else{
                XDGTool.userId = user.userId; //日志打印用
                XDGTool.Log($"LoginSync 设置userId {user.userId}");
            }

            XDGTool.Log("LoginSync 开始执行   GetCurrent");
            var preUser = await TDSUser.GetCurrent();
            if (preUser != null){
                if (preUser.ObjectId == user.userId){
                    XDGTool.Log("LoginSync 使用local pre user");
                    callback(user);
                    return;
                } else{
                    // id 不同可能是有残存的数据，则清空后走重新创建逻辑
                    XDGTool.Log("LoginSync 开始执行   await LCUser.Logout();");
                    await LCUser.Logout();
                }
            }

            XDGCommon.ShowLoading();
            var resultJson = "空";
            var command = new Command(XDG_ACCOUNT_SERVICE, "loginSync", true, null);
            EngineBridge.GetInstance().CallHandler(command, (async result => {
                try{
                    resultJson = result.ToJSON();
                    XDGTool.Log("LoginSync 方法结果: " + resultJson);
                    if (!XDGTool.checkResultSuccess(result)){
                        XDGCommon.HideLoading();
                        errorCallback(new XDGErrorMobile(result.code, result.message));
                        return;
                    }

                    var contentDic = Json.Deserialize(result.content) as Dictionary<string, object>;
                    var sessionToken = SafeDictionary.GetValue<string>(contentDic, "sessionToken");
                    var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "error");

                    if (errorDic != null){ //接口失败
                        XDGCommon.HideLoading();
                        errorCallback(new XDGErrorMobile(errorDic));
                        XDGTool.LogError("LoginSync 报错：请求sessionToken接口失败， 【result结果：" + resultJson + "】");
                        return;
                    }

                    if (XDGTool.IsEmpty(sessionToken)){ //接口成功，token是空(不太可能吧)
                        XDGCommon.HideLoading();
                        errorCallback(new XDGErrorMobile(-1000, "sessionToken is null"));
                        XDGTool.LogError("LoginSync 报错：token 是空！ 【result结果：" + resultJson + "】");
                        return;
                    }

                    LCUser lcUser = LCObject.CreateWithoutData(LCUser.CLASS_NAME, user.userId) as LCUser;
                    lcUser.SessionToken = sessionToken;
                    await lcUser.SaveToLocal();

                    callback(user);
                    XDGCommon.HideLoading();
                    XDGTool.Log("LoginSync  BecomeWithSessionToken 执行完毕");
                } catch (Exception e){
                    XDGCommon.HideLoading();
                    errorCallback(new XDGErrorMobile(result.code, result.message));
                    if (e.InnerException != null){
                        XDGTool.LogError("LoginSync 报错：" + e.Message + e.StackTrace + "【InnerException： " +
                                         e.InnerException.Message + e.InnerException.StackTrace + "】" + "。 【result结果：" +
                                         resultJson + "】");
                    } else{
                        XDGTool.LogError("LoginSync 报错：" + e.Message + e.StackTrace + "。 【result结果：" + resultJson +
                                         "】");
                    }

                    Console.WriteLine(e);
                }
            }));
        }


        public async void Logout(){
            await TDSUser.Logout(); //退出LC
            var command = new Command(XDG_ACCOUNT_SERVICE, "logout", false, null);
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void AddUserStatusChangeCallback(Action<XDGUserStatusCodeType, string> callback){
            var command = new Command(XDG_ACCOUNT_SERVICE, "addUserStatusChangeCallback", true,
                null);
            EngineBridge.GetInstance().CallHandler(command, (result) => {
                XDGTool.Log("AddUserStatusChangeCallback 方法结果: " + result.ToJSON());

                if (!XDGTool.checkResultSuccess(result)){
                    callback(XDGUserStatusCodeType.ERROR, "Unknow error");
                    return;
                }

                XDGUserStatusChangeWrapper wrapper = new XDGUserStatusChangeWrapper(result.content);
                if (wrapper.code == (int) XDGUserStatusCodeType.LOGOUT){
                    TDSUser.Logout();
                    callback(XDGUserStatusCodeType.LOGOUT, wrapper.message);
                } else if (wrapper.code == (int) XDGUserStatusCodeType.BIND){
                    callback(XDGUserStatusCodeType.BIND, wrapper.message);
                } else if (wrapper.code == (int) XDGUserStatusCodeType.UNBIND){
                    callback(XDGUserStatusCodeType.UNBIND, wrapper.message);
                }else if (wrapper.code == (int) XDGUserStatusCodeType.ProtocolAgreedAfterLogout){
                    callback(XDGUserStatusCodeType.ProtocolAgreedAfterLogout, wrapper.message);
                } else{
                    XDGTool.LogError($"AddUserStatusChangeCallback 未知回调 :{result.ToJSON()}");
                    callback(XDGUserStatusCodeType.ERROR, wrapper.message);
                }
            });
        }

        public void GetUser(Action<XDGUser> callback, Action<XDGError> errorCallback){
            var command = new Command(XDG_ACCOUNT_SERVICE, "getUser", true, null);
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("GetUser 方法结果: " + result.ToJSON());
                if (!XDGTool.checkResultSuccess(result)){
                    XDGTool.LogError($"GetUser 失败1 :{result.ToJSON()}");
                    errorCallback(new XDGErrorMobile(result.code, result.message));
                    return;
                }

                XDGUserWrapper userWrapper = new XDGUserWrapper(result.content);
                if (userWrapper.error != null){
                    XDGTool.LogError($"GetUser 失败2 :{result.ToJSON()}");
                    errorCallback(userWrapper.error);
                    return;
                }

                if (userWrapper.user == null){
                    XDGTool.LogError($"GetUser 失败3 :{result.ToJSON()}");
                }

                callback(userWrapper.user);
            });
        }

        public void OpenUserCenter(){
            var command = new Command(XDG_ACCOUNT_SERVICE, "openUserCenter", false, null);
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void LoginByType(LoginType loginType, Action<XDGUser> callback, Action<XDGError> errorCallback){
            var command = new Command.Builder()
                .Service(XDG_ACCOUNT_SERVICE)
                .Method("loginByType")
                .Args("loginType", XDGUserMobile.GetLoginTypeString(loginType)) //和app交互用的是字符串，如TapTap 
                .Callback(true)
                .CommandBuilder();

            XDGTool.Log("调用方法：loginByType ");
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("LoginByType 方法结果: " + result.ToJSON());
                if (!XDGTool.checkResultSuccess(result)){
                    XDGTool.LogError($"LoginByType 登录失败1：{result.ToJSON()} ");
                    errorCallback(new XDGErrorMobile(result.code, result.message));
                    return;
                }

                XDGUserWrapper wrapper = new XDGUserWrapper(result.content);
                if (wrapper.error != null){
                    XDGTool.LogError($"LoginByType 登录失败2：{result.ToJSON()}");
                    errorCallback(wrapper.error);
                    return;
                }

                if (wrapper.user == null){
                    XDGTool.LogError($"LoginByType 登录失败3 wrapper user 是空 ：{result.ToJSON()}");
                }

                ActiveLearnCloudToken(wrapper.user, callback, errorCallback);
            });
        }

        public void OpenUnregister(){
            var command = new Command(XDG_ACCOUNT_SERVICE, "accountCancellation", false, null);
            EngineBridge.GetInstance().CallHandler(command);
        }

        // 641 FB token
        public void IsTokenActiveWithType(LoginType loginType, Action<bool> callback){
            var command = new Command.Builder()
                .Service(XDG_ACCOUNT_SERVICE)
                .Method("isTokenActiveWithType")
                .Args("isTokenActiveWithType", XDGUserMobile.GetLoginTypeString(loginType))
                .Callback(true)
                .CommandBuilder();
                
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("isTokenActiveWithType 方法结果: " + result.ToJSON());
                if (!XDGTool.checkResultSuccess(result)){
                    XDGTool.LogError($"isTokenActiveWithType 失败1：{result.ToJSON()} ");
                    callback(false);
                    return;
                }
                
                var contentDic = Json.Deserialize(result.content) as Dictionary<string, object>;
                var success = SafeDictionary.GetValue<bool>(contentDic, "success");
                callback(success);
            }); 
        }
                
        public void BindByType(LoginType loginType, Action<bool, XDGError> callback){
            var command = new Command.Builder()
                .Service(XDG_ACCOUNT_SERVICE)
                .Method("bindByType")
                .Args("bindByType", XDGUserMobile.GetLoginTypeString(loginType))
                .Callback(true)
                .CommandBuilder();
                
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("bindByType 方法结果: " + result.ToJSON());
                
                if (!XDGTool.checkResultSuccess(result)){
                    XDGTool.LogError($"bindByType 失败1：{result.ToJSON()} ");
                    callback(false, new XDGErrorMobile(result.code, result.message));
                    return;
                }
                
                var contentDic = Json.Deserialize(result.content) as Dictionary<string, object>;
                var success = SafeDictionary.GetValue<bool>(contentDic, "success");
                var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "error");
                
                XDGErrorMobile error = null;
                if (errorDic != null){
                    error = new XDGErrorMobile(errorDic);
                    XDGTool.LogError($"bindByType 失败2：{result.ToJSON()} ");
                }
                callback(success, error);
            }); 
        }

        public void GetFacebookToken(Action<string, string> successCallback, Action<XDGError> errorCallback){
            var command = new Command.Builder()
                .Service(XDG_ACCOUNT_SERVICE)
                .Method("getFacebookToken")
                .Callback(true)
                .CommandBuilder();
                
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("getFacebookToken 方法结果: " + result.ToJSON());
                if (!XDGTool.checkResultSuccess(result)){
                    XDGTool.LogError($"getFacebookToken 失败1：{result.ToJSON()} ");
                    errorCallback(new XDGErrorMobile(result.code, result.message));
                    return;
                }
                
                var contentDic = Json.Deserialize(result.content) as Dictionary<string, object>;
                var userId = SafeDictionary.GetValue<string>(contentDic, "userID");
                var accessToken = SafeDictionary.GetValue<string>(contentDic, "access_token");
                var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "error");

                if (errorDic != null){
                    XDGTool.LogError($"getFacebookToken 失败2：{result.ToJSON()} ");
                    errorCallback(new XDGErrorMobile(errorDic));
                } else{
                    successCallback(userId, accessToken);
                }
            }); 
        }
        
        public void updateThirdPlatformTokenWithCallback(Action<bool> callback){
            var command = new Command.Builder()
                .Service(XDG_ACCOUNT_SERVICE)
                .Method("updateThirdPlatformTokenWithCallback")
                .Callback(true)
                .CommandBuilder();
                
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("updateThirdPlatformTokenWithCallback 方法结果: " + result.ToJSON());
                if (!XDGTool.checkResultSuccess(result)){
                    XDGTool.LogError($"updateThirdPlatformTokenWithCallback 失败1：{result.ToJSON()} ");
                    callback(false);
                    return;
                }
                
                var contentDic = Json.Deserialize(result.content) as Dictionary<string, object>;
                var success = SafeDictionary.GetValue<bool>(contentDic, "success");
                callback(success);
            }); 
        }
        
    }
}