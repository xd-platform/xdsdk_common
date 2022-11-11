using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TapTap.Bootstrap;
using TapTap.Common;
using UnityEngine;

namespace XD.SDK.Common{
    public class XDGCommonImpl{
        private static readonly string COMMON_MODULE_UNITY_BRIDGE_NAME = "XDGCoreService";
        private static volatile XDGCommonImpl _instance;
        private static readonly object locker = new object();

        private static string Channel = "";
        private static string GameVersion = "";

        private XDGCommonImpl(){
            XDGTool.Log("===> Init XDG Bridge Service");
            EngineBridge.GetInstance()
                .Register(XDGUnityBridge.COMMON_SERVICE_NAME, XDGUnityBridge.COMMON_SERVICE_IMPL);
        }

        public static XDGCommonImpl GetInstance(){
            if (_instance != null) return _instance;
            lock (locker){
                if (_instance == null){
                    _instance = new XDGCommonImpl();
                }
            }

            return _instance;
        }

        public void InitSDK(Action<bool, string> callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("initSDK")
                .Callback(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, (result) => {
                XDGTool.Log("===> Init XDG SDK result: " + result.ToJSON());
                if (!checkResultSuccess(result)){
                    callback(false, "Init SDK Fail");
                    XDGTool.LogError("初始化失败 result：" + result.ToJSON());
                    return;
                }

                var wrapper = new XDGInitResultWrapper(result.content);
                if (wrapper.localConfigInfo.tapSdkConfig != null && wrapper.isSuccess){
                    var info = wrapper.localConfigInfo.tapSdkConfig;
                    var region = RegionType.CN;
                    if (info.region != 0){ //0国内，否则海外
                        region = RegionType.IO;
                    }
                    
                    //替换channel, gameVersion
                    if (!string.IsNullOrEmpty(Channel)){
                        info.channel = Channel;
                    }
                    if (!string.IsNullOrEmpty(GameVersion)){
                        info.gameVersion = GameVersion;
                    }

                    if (string.IsNullOrEmpty(info.gameVersion)){
                        info.gameVersion = ""; //不可以null 否则3.9.0 tapdb初始化不了
                    }
                    
                    var config = new TapConfig.Builder()
                        .ClientID(info.clientId) // 必须，开发者中心对应 Client ID
                        .ClientToken(info.clientToken) // 必须，开发者中心对应 Client Token
                        .ServerURL(info.serverUrl) // 开发者中心 > 你的游戏 > 游戏服务 > 云服务 > 数据存储 > 服务设置 > 自定义域名 绑定域名
                        .RegionType(region) // 非必须，默认 CN 表示国内, IO国外
                        .TapDBConfig(info.enableTapDB, info.channel, info.gameVersion, info.idfa)
                        .ConfigBuilder();
                    TapBootstrap.Init(config);
                    XDGTool.Log(
                        $"初始化 TapBootstrap 成功：{JsonUtility.ToJson(info)}");
                }

                callback(wrapper.isSuccess, wrapper.message);
            });
        }

        public void IsInitialized(Action<bool> callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("isInitialized")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("===> IsInitialized: " + result.ToJSON());
                if (!checkResultSuccess(result)){
                    callback(false);
                    return;
                }

                callback("true".Equals(result.content.ToLower()));
            });
        }

        public void SetLanguage(LangType langType){
            XDGTool.Log("===> SetLanguage langType: " + langType);
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setLanguage")
                .Args("langType", (int) langType)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);

            //设置 TapCommon
            TapLanguage tType = TapLanguage.AUTO;
            if (langType == LangType.ZH_CN){
                tType = TapLanguage.ZH_HANS;
            } else if (langType == LangType.ZH_TW){
                tType = TapLanguage.ZH_HANT;
            } else if (langType == LangType.JP){
                tType = TapLanguage.JA;
            } else if (langType == LangType.KR){
                tType = TapLanguage.KO;
            } else if (langType == LangType.EN){
                tType = TapLanguage.EN;
            } else if (langType == LangType.TH){
                tType = TapLanguage.TH;
            } else if (langType == LangType.ID){
                tType = TapLanguage.ID;
            }

            TapCommon.SetLanguage(tType);
        }

        public void Share(ShareFlavors shareFlavors, string imagePath, XDGShareCallback callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("shareWithImage")
                .Args("shareFlavors", (int) shareFlavors)
                .Args("imagePath", imagePath)
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("===> share with type: " + shareFlavors + " result: " + result.ToJSON());
                if (!checkResultSuccess(result)){
                    callback.ShareFailed($"Share Failed:{result.message}");
                    return;
                }

                var shareWrapper = new XDGShareResultWrapper(result.content);
                if (shareWrapper.cancel){
                    callback.ShareCancel();
                    return;
                }

                if (shareWrapper.error != null){
                    if (!string.IsNullOrEmpty(shareWrapper.error.error_msg)){
                        callback.ShareFailed(shareWrapper.error.error_msg);
                        return;
                    }
                }

                callback.ShareSuccess();
            });
        }

        public void Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("shareWithUriMessage")
                .Args("shareFlavors", (int) shareFlavors)
                .Args("uri", uri)
                .Args("message", message)
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("===> share with type: " + shareFlavors + " result: " + result.ToJSON());
                if (!checkResultSuccess(result)){
                    callback.ShareFailed($"Share Failed:{result.message}");
                    return;
                }

                var shareWrapper = new XDGShareResultWrapper(result.content);
                if (shareWrapper.cancel){
                    callback.ShareCancel();
                    return;
                }

                if (shareWrapper.error != null){
                    if (!string.IsNullOrEmpty(shareWrapper.error.error_msg)){
                        callback.ShareFailed(shareWrapper.error.error_msg);
                        return;
                    }
                }

                callback.ShareSuccess();
            });
        }

        public void Report(string serverId, string roleId, string roleName){
            var argsDic = new Dictionary<string, object>{
                {"serverId", serverId},
                {"roleId", roleId},
                {"roleName", roleName}
            };
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("report")
                .Args(argsDic)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> Report:  {serverId} -- {roleId} -- {roleName}");
        }

        public void TrackRole(string serverId, string roleId, string roleName, int level){
            var argsDic = new Dictionary<string, object>{
                {"serverId", serverId},
                {"roleId", roleId},
                {"roleName", roleName},
                {"level", level}
            };
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackRole")
                .Args(argsDic)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> TrackRole:  {serverId} -- {roleId} -- {roleName} -- {level}");
        }

        public void TrackUser(string userId){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackUser")
                .Args("userId", userId)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> TrackUser:  {userId}");
        }

        public void TrackEvent(string eventName){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackEvent")
                .Args("eventName", eventName)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log($"===> TrackEvent:  {eventName}");
        }


        public void EventCompletedTutorial(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("eventCompletedTutorial")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> eventCompletedTutorial");
        }

        public void EventCreateRole(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("eventCreateRole")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> eventCreateRole");
        }

        public void GetVersionName(Action<string> callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("getSDKVersionName")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("===> GetVersionName: " + result.ToJSON());
                if (!checkResultSuccess(result)){
                    callback($"GetVersionName Failed:{result.message}");
                    return;
                }

                callback(result.content);
            });
        }

        public void TrackAchievement(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("trackAchievement")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> trackAchievement");
        }

        public void SetCurrentUserPushServiceEnable(bool enable){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setCurrentUserPushServiceEnable")
                .Args("enable", enable)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> SetCurrentUserPushServiceEnable");
        }

        public void IsCurrentUserPushServiceEnable(Action<bool> callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("isCurrentUserPushServiceEnable")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("===> isCurrentUserPushServiceEnable: " + result.ToJSON());
                if (!checkResultSuccess(result)){
                    callback(false);
                    return;
                }

                callback("true".Equals(result.content.ToLower()));
            });
        }

        public void StoreReview(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("storeReview")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
            XDGTool.Log("===> StoreReview");
        }

        public void GetRegionInfo(Action<XDGRegionInfoWrapper> callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("getRegionInfo")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("GetRegionInfo result --> " + JsonUtility.ToJson(result));
                callback(new XDGRegionInfoWrapper(result.content));
            });
        }

        public void ShowLoading(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("showLoading")
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void HideLoading(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("hideLoading")
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void LoginSuccessEvent(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("loginSuccessEvent")
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void LoginFailEvent(string loginFailMsg){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("loginFailEvent")
                .Args("loginFailMsg", loginFailMsg)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void SetDebugMode(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setDebugMode")
                .Args("setDebugMode", 1)
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void setTargetCountryOrRegion(string range){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("setTargetCountryOrRegion")
                .Args("setTargetCountryOrRegion", range)
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void disableAgreementUI(){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("disableAgreementUI")
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }

        public void updateConfigFileName(string fileName){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("updateConfigFileName")
                .Args("updateConfigFileName", fileName)
                .Callback(false)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
        }
        
        public void ReplaceChannelAndVersion(string channel, string gameVersion){
            Channel = channel;
            GameVersion = gameVersion;
        }

        public void clearAllUserDefaultsData(){
#if UNITY_IOS
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("clearAllUserDefaultsData")
                .OnceTime(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command);
#endif
        }
        
        public void GetDid(Action<string> callback){
            var command = new Command.Builder()
                .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                .Method("getDid")
                .Callback(true)
                .CommandBuilder();
            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGTool.Log("===> getDid: " + result.ToJSON());
                if (!checkResultSuccess(result)){
                    callback($"getDid Failed:{result.message}");
                    return;
                }
                callback(result.content);
            });
        }

        private bool checkResultSuccess(Result result){
            return result.code == Result.RESULT_SUCCESS && !string.IsNullOrEmpty(result.content);
        }
        
        public void GetAgreementList(Action<List<XDGAgreement>> callback)
        {
            try
            {
                var command = new Command.Builder()
                    .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                    .Method("getAgreementList")
                    .Callback(true)
                    .CommandBuilder();
                EngineBridge.GetInstance().CallHandler(command, result => {
                    XDGTool.Log("===> GetAgreementList: " + result.ToJSON());
                    if (!checkResultSuccess(result)){
                        return;
                    }
                    var content = result.content;
                    callback ?.Invoke(GetAgreementList(content));
                });
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat($"获取 AgreementList 遇到错误!\n{e.Message}\n{e.StackTrace}");
            }
        }

        private List<XDGAgreement> GetAgreementList (string jsonStr)
        {
            XDGTool.Log("[GetAgreementList] jsonStr:\n" + jsonStr);
            List<XDGAgreement> result = null;
            var dicStrStr = Json.Deserialize(jsonStr);
            var dic = dicStrStr as Dictionary<string, object>;
            var list = SafeDictionary.GetValue<List<object>>(dic, "list");
            if (list == null)  return result;
            result = new List<XDGAgreement>();
            foreach (var agreementStr in list)
            {
                var agreement = new XDGAgreement(agreementStr as Dictionary<string, object>);
                result.Add(agreement);
            }

            return result;
        }
        
        public void ShowDetailAgreement(string agreementUrl)
        {
            try
            {
                var command = new Command.Builder()
                    .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                    .Method("showDetailAgreement")
                    .Args("showDetailAgreement", agreementUrl)
                    .OnceTime(true)
                    .CommandBuilder();
                EngineBridge.GetInstance().CallHandler(command);
                XDGTool.Log($"===> ShowDetailAgreement:  {agreementUrl}");
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat($"显示详细Agreement遇到错误!\n{e.Message}\n{e.StackTrace}");
            }
        }
        
        public void SetExitHandler(Action onExitHandler)
        {
            try
            {
                XDGTool.Log("[unity 设置 exitHandler:");
                var command = new Command.Builder()
                    .Service(COMMON_MODULE_UNITY_BRIDGE_NAME)
                    .Method("setExitHandler")
                    .Callback(true)
                    .CommandBuilder();
                EngineBridge.GetInstance().CallHandler(command, result => {
                    XDGTool.Log("===> setExitHandler: " + result.ToJSON());
                    if (!checkResultSuccess(result)){
                        return;
                    }
                    if (onExitHandler == null)
                        Application.Quit();
                    else
                        onExitHandler.Invoke();
                });
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat($"SetExitHandler遇到错误!\n{e.Message}\n{e.StackTrace}");
            }
        }
    }
}