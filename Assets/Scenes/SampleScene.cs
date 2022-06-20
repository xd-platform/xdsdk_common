using System;
using System.Collections.Generic;
using System.Globalization;
using LC.Newtonsoft.Json;
using LeanCloud;
using LeanCloud.Storage;
using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.Moment;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using XD.SDK.Account;
using XD.SDK.Common;
using XD.SDK.Payment;
using Random = UnityEngine.Random;
// using Plugins.AntiAddictionUIKit; // 命名空间

public class SampleScene : MonoBehaviour{
    public Text ResultText;
    public InputField LanguageField;
    public InputField RangeField;
    public InputField LoginField;
    public InputField AppleProductField;
    public InputField GoogleProductField;
    public InputField WebPayServiceField;
    public InputField WebPayProductField;

    private string UserId;
    private List<LoginType> loginTypes = new List<LoginType>();
    private bool isCN = false;

    private void Start(){
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        LCLogger.LogDelegate = (LCLogLevel level, string info) => {
            switch (level){
                case LCLogLevel.Debug:
                    XDGTool.Log($"LC [DEBUG] {DateTime.Now} {info}\n");
                    break;
                case LCLogLevel.Warn:
                    XDGTool.Log($"LC [WARNING] {DateTime.Now} {info}\n");
                    break;
                case LCLogLevel.Error:
                    XDGTool.Log($"LC [ERROR] {DateTime.Now} {info}\n");
                    break;
                default:
                    XDGTool.Log(info);
                    break;
            }
        };
    }

    public void SetLanguage(){
        var txt = LanguageField.text;
        if (string.IsNullOrEmpty(txt)){
            txt = "0";
        }

        var x = Int32.Parse(txt);
        if (x >= 0 && x <= 12){
            var type = (LangType) x;
            XDGCommon.SetLanguage(type);
            ResultText.text = $"设置语言：{type}";
        } else{
            ResultText.text = "请输入0到12";
        }
    }

    public void SetRange(){
        var region = RangeField.text;
        if (string.IsNullOrEmpty(region)){
            region = "DF";
        }

        XDGCommonImpl.GetInstance().setTargetCountryOrRegion(region);
        ResultText.text = $"设置地区：{region}";
    }

    public void InitRelease(){
        XDGCommonImpl.GetInstance().SetDebugMode();
        XDGCommonImpl.GetInstance().updateConfigFileName("XDConfig");
        XDGCommon.InitSDK(((success, msg) => {
            if (success){
                ResultText.text = $"初始化成功 {success} {msg}";
            } else{
                ResultText.text = $"初始化失败 {success} {msg}";
            }
        }));

        XDGAccount.AddUserStatusChangeCallback((code, msg) => { ResultText.text = $"用户状态回调 code: {code}  msg:{msg}"; });
    }

    public void InitDevelop(){
        SetDevelopUrl();
        InitRelease();
    }

    public void InitRelease_CN(){
        isCN = true;
        XDGCommonImpl.GetInstance().SetDebugMode();
        XDGCommonImpl.GetInstance().updateConfigFileName("XDConfig-cn");
        XDGCommon.InitSDK(((success, msg) => {
            if (success){
                ResultText.text = $"初始化成功 {success} {msg}";
                // InitAntiSDK();
            } else{
                ResultText.text = $"初始化失败 {success} {msg}";
            }
        }));

        XDGAccount.AddUserStatusChangeCallback((code, msg) => { ResultText.text = $"用户状态回调 code: {code}  msg:{msg}"; });
    }

    public void InitDevelop_CN(){
        SetDevelopUrl();
        InitRelease_CN();
    }

    public void ClearLocalData(){
        XDGCommonImpl.GetInstance().clearAllUserDefaultsData();
        ResultText.text = "清楚缓存";
    }

    public void DisableAgreementUI(){
        XDGCommonImpl.GetInstance().disableAgreementUI();
        ResultText.text = "关闭协议弹框";
    }

    public void AlertLogin(){
        loginTypes.Add(LoginType.TapTap);
        loginTypes.Add(LoginType.Guest);
        if (!isCN){
            loginTypes.Add(LoginType.Google);
            loginTypes.Add(LoginType.Facebook);
            loginTypes.Add(LoginType.Twitter);
            loginTypes.Add(LoginType.LINE);
            loginTypes.Add(LoginType.Apple);
        }

        XDGAccount.Login(loginTypes, user => {
            UserId = user.userId;
            ResultText.text = JsonUtility.ToJson(user);
        }, error => { ResultText.text = error.error_msg; });
    }

    public void AutoLogin(){
        XDGAccount.LoginByType(LoginType.Default, user => {
                UserId = user.userId;
                ResultText.text = JsonUtility.ToJson(user);
            },
            error => { ResultText.text = "登录失败: " + error.ToJSON(); });
    }

    public void TypeLogin(){
        var type = GetLoginType(LoginField.text);
        var isTap = (type == LoginType.TapTap) ? true : false;
        
        XDGAccount.LoginByType(type, user => {
                UserId = user.userId;
                ResultText.text = JsonUtility.ToJson(user);
                
                // StartAnti(user.userId, isTap);
            },
            error => {
                ResultText.text = "登录失败: " + error.ToJSON();
            });
    }

    public void Logout(){
        XDGAccount.Logout();
        // AntiAddictionUIKit.Logout();
    }

    public void GetUserInfo(){
        XDGAccount.GetUser((user) => {
                UserId = user.userId;
                ResultText.text = JsonUtility.ToJson(user);
            },
            (error) => { ResultText.text = "失败: " + error.ToJSON(); });
    }

    public void OpenUserCenter(){
        XDGAccount.OpenUserCenter();
    }

    public void OpenFeedbackCenter(){
        XDGCommon.Report("serverId", UserId, "roleName");
    }

    public void OpenAppStore(){
        XDGCommon.StoreReview();
    }

    public void OpenMoment(){
        TapMoment.Open(Orientation.ORIENTATION_LANDSCAPE);
    }

    public void GetIpInfo(){
        XDGCommon.GetRegionInfo(wrapper => {
            ResultText.text = "获取地区信息数据：" + JsonUtility.ToJson(wrapper.info);
            XDGTool.Log("获取地区信息数据：" + JsonUtility.ToJson(wrapper.info));
        });
    }

    public void OpenUnregister(){
        XDGAccount.AccountCancellation();
    }

    public void ApplePay(){
        var tmp = AppleProductField.text;
        var productId = "com.xd.sdkdemo1.stone30";
        if (tmp == "1"){
            productId = "com.xd.sdkdemo1.stone300";
        } else if (tmp == "2"){
            productId = "com.xd.sdkdemo1.stone500";
        } else if (tmp == "3"){
            productId = "com.xd.sdkdemo1.stone980";
        }

        ResultText.text = $"苹果支付 {productId}";

        XDGPayment.PayWithProduct("", productId, UserId, "serverId", "ext",
            wrapper => {
                XDGTool.Log("支付结果" + JsonUtility.ToJson(wrapper));
                if (wrapper.xdgError != null){
                    ResultText.text = "失败 :" + wrapper.xdgError.ToJSON();
                } else{
                    ResultText.text = "成功: " + JsonUtility.ToJson(wrapper);
                }
            });
    }

    public void GooglePay(){
        var tmp = GoogleProductField.text;
        var productId = "com.tds.sdkdemopro.fxqusd199";
        if (tmp == "1"){
            productId = "com.tds.sdkdemopro.fxqusd099";
        } else if (tmp == "2"){
            productId = "com.tds.sdkdemopro.fxqusd299";
        } else if (tmp == "3"){
            productId = "com.tds.sdkdemopro.fxqusd399";
        }

        ResultText.text = $"谷歌支付 {productId}";

        XDGPayment.PayWithProduct("", productId, UserId, "serverId", "ext",
            wrapper => {
                XDGTool.Log("支付结果" + JsonUtility.ToJson(wrapper));
                if (wrapper.xdgError != null){
                    ResultText.text = "失败 :" + wrapper.xdgError.ToJSON();
                } else{
                    ResultText.text = "成功: " + JsonUtility.ToJson(wrapper);
                }
            });
    }

    public void CheckPay(){
        XDGPayment.CheckRefundStatus((wrapper) => {
            XDGTool.Log("获取补单列表数据" + JsonUtility.ToJson(wrapper));
            if (wrapper.xdgError != null){
                ResultText.text = wrapper.xdgError.error_msg;
            } else{
                var list = wrapper.refundList;
                if (list != null && list.Count > 0){
                    ResultText.text = "需要补单：" + JsonUtility.ToJson(list);
                } else{
                    ResultText.text = "没有需要补单的";
                }
            }
        });
    }

    public void CheckPayWithUI(){
        XDGPayment.CheckRefundStatusWithUI((wrapper) => {
            XDGTool.Log("获取补单列表数据" + JsonUtility.ToJson(wrapper));
            if (wrapper.xdgError != null){
                ResultText.text = wrapper.xdgError.error_msg;
            } else{
                var list = wrapper.refundList;
                if (list != null && list.Count > 0){
                    ResultText.text = "需要补单：" + JsonUtility.ToJson(list);
                } else{
                    ResultText.text = "没有需要补单的";
                }
            }
        });
    }

    public void QueryAppleList(){
        var productIds = "com.xd.sdkdemo1.stone30,com.xd.sdkdemo1.stone300";
        XDGPayment.QueryWithProductIds(productIds.Split(','), info => {
            XDGTool.Log("查询结果" + JsonUtility.ToJson(info));
            if (info.xdgError != null){
                ResultText.text = "失败：" + info.xdgError.ToJSON();
            } else{
                ResultText.text = JsonUtility.ToJson(info);
            }
        });
    }

    public void QueryGoogleList(){
        var productIds = "com.tds.sdkdemopro.fxqusd099,com.tds.sdkdemopro.fxqusd199";
        XDGPayment.QueryWithProductIds(productIds.Split(','), info => {
            XDGTool.Log("查询结果" + JsonUtility.ToJson(info));
            if (info.xdgError != null){
                ResultText.text = "失败：" + info.xdgError.ToJSON();
            } else{
                ResultText.text = JsonUtility.ToJson(info);
            }
        });
    }

    public void OpenWebPay(){
        var tmp = WebPayProductField.text;
        var productId = "com.tds.sdkdemopro.fxqusd199";
        if (tmp == "1"){
            productId = "com.tds.sdkdemopro.fxqusd099";
        } else if (tmp == "2"){
            productId = "com.tds.sdkdemopro.fxqusd299";
        } else if (tmp == "3"){
            productId = "com.tds.sdkdemopro.fxqusd399";
        }

        ResultText.text = $"支付 {productId}";

        var serverId = WebPayServiceField.text;
        if (string.IsNullOrEmpty(serverId)){
            serverId = "serverId";
        }

        XDGPayment.PayWithWeb(serverId, UserId, productId, "ext",
            error => { ResultText.text = $"网页支付 " + error.ToJSON(); });
    }


    private void SetDevelopUrl(){
#if !UNITY_EDITOR && UNITY_ANDROID
        var jc = new AndroidJavaClass("com.xd.intl.common.utils.EnvHelper");
        var typeClass = new AndroidJavaClass("com.xd.intl.common.utils.EnvHelper$EnvEnum");
        var jo = typeClass.GetStatic<AndroidJavaObject>("Dev");
        jc.CallStatic("setApiEnv", jo);
#endif

#if !UNITY_EDITOR && UNITY_IOS
        TapCommon.AddHost("https://xdsdk-6.xd.cn", "https://test-xdsdk-6.xd.cn");
        TapCommon.AddHost("https://xdsdk-intnl-6.xd.com", "https://test-xdsdk-intnl-6.xd.com");
        TapCommon.AddHost("https://ecdn-xdsdk-intnl-6.xd.com", "https://test-xdsdk-intnl-6.xd.com");
        TapCommon.AddHost("https://event-tracking-cn.cn-beijing.log.aliyuncs.com/logstores/sdk6-prod/track", "https://event-tracking-cn.cn-beijing.log.aliyuncs.com/logstores/sdk6-test/track");
        TapCommon.AddHost("https://event-tracking-global.ap-southeast-1.log.aliyuncs.com/logstores/sdk6-prod/track", "https://event-tracking-global.ap-southeast-1.log.aliyuncs.com/logstores/sdk6-test/track");
#endif
    }

    private LoginType GetLoginType(string loginType){
        switch (loginType){
            case "0":
                return LoginType.TapTap;
            case "1":
                return LoginType.Google;
            case "2":
                return LoginType.Facebook;
            case "3":
                return LoginType.Apple;
            case "4":
                return LoginType.LINE;
            case "5":
                return LoginType.Twitter;
            case "6":
                return LoginType.Guest;
        }

        return LoginType.Default;
    }

    //---------防沉迷接入--------

    // private void InitAntiSDK(){
    //     string gameIdentifier = "Wzy7xYhKtYdnLUXevV"; //国内的 tap client id
    //     bool useTimeLimit = true; // 是否启用时长限制功能
    //     bool usePaymentLimit = true; // 是否启用消费限制功能
    //     bool showSwitchAccount = false; // 是否显示切换账号按钮
    //
    //     AntiAddictionUIKit.Init(gameIdentifier, useTimeLimit, usePaymentLimit, showSwitchAccount,
    //         (antiAddictionCallbackData) => {
    //             int code = antiAddictionCallbackData.code;
    //             MsgExtraParams extras = antiAddictionCallbackData.extras;
    //             // 根据 code 不同提示玩家不同信息，详见下面的说明
    //             if (code == 500){
    //                 // 开始计时
    //                 AntiAddictionUIKit.EnterGame();
    //                 Debug.Log("玩家登录后判断当前玩家可以进行游戏");
    //             }
    //             
    //             XDGTool.Log($"防沉迷 code {code},  data: {JsonUtility.ToJson(extras)}");
    //         },
    //         (exception) => {
    //             // 处理异常
    //             XDGTool.Log($"防沉迷异常: {exception}");
    //         }
    //     );
    // }
    //
    // private void StartAnti(string userId, bool isTap){
    //     if (string.IsNullOrEmpty(userId)){
    //         XDGTool.Log("防沉迷UserId是空");
    //         return;
    //     }
    //     XDGTool.Log($"防沉迷start: {userId},  isTap:{isTap}");
    //     AntiAddictionUIKit.Startup(isTap, userId);
    // }
}