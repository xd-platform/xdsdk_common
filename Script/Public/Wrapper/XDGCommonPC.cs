#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.Login;
using UnityEngine;
using UnityEngine.Scripting;
using XD.SDK.Common.PC.Internal;

namespace XD.SDK.Common
{
    [Preserve]
    public class XDGCommonPC : IXDGCommon {
        public static readonly string XD_PERSISTENCE_NAME = "XD/PC";
        public string UserId { get; set; }

        // 初始化 SDK
        public async void InitSDK(Action<bool, string> callback) {
            try {
                await Init();
                callback?.Invoke(true, "XDSDK Init OK");
            } catch (Exception e) {
                callback?.Invoke(false, $"XDSDK Init Error! Message: {e.Message}\n{e.StackTrace}");
            }
        }

        // 是否初始化完毕
        public void IsInitialized(Action<bool> callback) {
            callback?.Invoke(isInited);
        }

        // 客服系统
        public void Report(string serverId, string roleId, string roleName) {
#pragma warning disable 4014
            OpenCustomerCenter(serverId, roleId, roleName);
#pragma warning restore 4014
        }

        // 获得相关协议
        public void GetAgreementList(Action<List<XDGAgreement>> callback) {
            var list = AgreementList;
            callback?.Invoke(list);
        }

        // 显示协议详情
        public void ShowDetailAgreement(string agreementUrl) {
            Application.OpenURL(agreementUrl);
        }

        // 获取版本号
        public void GetVersionName(Action<string> callback) {
            callback?.Invoke(SDKVersion);
        }

        // 设置语言
        public void SetLanguage(LangType langType) {
            XD.SDK.Common.PC.Internal.Localization.CurrentLang = langType;
        }

        // 获取地区信息
        public async void GetRegionInfo(Action<XDGRegionInfoWrapper> callback) {
            var info = await GetLocationInfo();
            callback?.Invoke(new IPInfoWrapper(info));
        }

        #region XDSDK 迁移
        private static readonly string VERSION = "6.18.0-qa6180.2";
        public static string SDKVersion => VERSION;
        private static bool isIniting;
        private static bool isInited;
        
        public static void CheckInit() {
            if (!isInited) {
                throw XDException.MSG("请先初始化");
            }
        }
        
        private static async Task Init() {
            if (isIniting) {
                XDGLogger.Warn("正在初始化...");
                return;
            }

            if (isInited) {
                XDGLogger.Warn("已经初始化");
                return;
            }

            isIniting = true;
              
            try {
                ConfigModule.LoadConfig();

                AliyunTrack.Init();

                await AgreementModule.ConfirmAgreement();

                _ = ConfigModule.FetchConfig();

                InitTap();
                isInited = true;
            } catch (Exception e) {
                isInited = false;
                throw e;
            } finally {
                isIniting = false;
            }
        }
        
        private static void InitTap() {
            TapCommon.SetDurationStatisticsEnabled(false);
            var tapConfig = ConfigModule.TapConfig;
            TapLogin.Init(tapConfig.ClientId, false, false);

            RegionType regionType = ConfigModule.IsGlobal ? RegionType.IO : RegionType.CN;
            var tdsConfig = new TapTap.Common.TapConfig.Builder()
                .ClientID(tapConfig.ClientId)
                .ClientToken(tapConfig.ClientToken)
                .ServerURL(tapConfig.ServerUrl)
                .RegionType(regionType)
                .TapDBConfig(tapConfig.DBConfig.Enable, tapConfig.DBConfig.Channel, tapConfig.DBConfig.Version)
                .ConfigBuilder();
            TapBootstrap.Init(tdsConfig);
        }
        
        private static async Task OpenCustomerCenter(string serverId, string roleId, string roleName){
            var url = await CustomerModule.GetCustomerCenterUrl(serverId, roleId, roleName);
            if (string.IsNullOrEmpty(url)){
                XDGLogger.Warn("请先登录游戏");
            } else{
                url = Uri.EscapeUriString(url);
                XDGLogger.Debug("客服中心URL: " + url);
                Application.OpenURL(url);
            }
        }
        
        private static List<XDGAgreement> AgreementList
        {
            get
            {
                var agreements = AgreementModule.CurrentAgreement?.SubAgreements;
                var result = new List<XDGAgreement>(agreements.Count);
                foreach (var agreement in agreements)
                {
                    result.Add(agreement);
                }

                return result;
            }
        }
        
        private static async Task<IPInfo> GetLocationInfo() {
            return await IPInfoModule.RequestIpInfo();
        }

        #endregion
        
        #region 未实现部分

        public void Share(ShareFlavors shareFlavors, string uri, XDGShareCallback callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void TrackRole(string serverId, string roleId, string roleName, int level)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void TrackUser(string userId = null)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void TrackEvent(string eventName, Dictionary<string,object> properties = null)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void EventCompletedTutorial()
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void EventCreateRole()
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }
        
        public void TrackAchievement()
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void SetCurrentUserPushServiceEnable(bool enable)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void IsCurrentUserPushServiceEnable(Action<bool> callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void StoreReview()
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void ShowLoading()
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void HideLoading()
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void DisableAgreementUI()
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void ReplaceChannelAndVersion(string channel, string gameVersion)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void SetExitHandler(Action onExitHandler)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }
        #endregion
    }
}
#endif
