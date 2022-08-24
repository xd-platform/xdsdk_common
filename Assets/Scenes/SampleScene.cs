using System;
using System.Collections.Generic;
using System.Globalization;
using LC.Newtonsoft.Json;
using LeanCloud;
using LeanCloud.Storage;
using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.Login;
using TapTap.Moment;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using XD.SDK.Account;
using XD.SDK.Common;
using XD.SDK.Payment;
using Random = UnityEngine.Random;

public class SampleScene : MonoBehaviour{
    public Text ResultText;
    public InputField LanguageField;
    public InputField RangeField;
    public InputField LoginField;
    public InputField AppleProductField;
    public InputField GoogleProductField;
    public InputField WebPayServiceField;
    public InputField WebPayProductField;
    public InputField BindField;

    private XDGUser User;
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
        if (x >= 0 && x <= 13){
            var type = (LangType) x;
            XDGCommon.SetLanguage(type);
            ResultText.text = $"设置语言：{type}";
        } else{
            ResultText.text = "请输入0到13";
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

    public void InitRelease(){ //海外正式
        isCN = false;
        XDGCommonImpl.GetInstance().SetDebugMode();
        XDGCommonImpl.GetInstance().updateConfigFileName("XDConfig-release.json");
        XDGCommon.InitSDK(((success, msg) => {
            if (success){
                ResultText.text = $"初始化成功 {success} {msg}";
            } else{
                ResultText.text = $"初始化失败 {success} {msg}";
            }
        }));

        XDGAccount.AddUserStatusChangeCallback((type, msg) => {
            ResultText.text = $"用户状态回调 code: {type}  msg:{msg}";
        });
    }

    public void InitDevelop(){ //海外测试
        SetDevelopUrl();
        
        isCN = false;
        XDGCommonImpl.GetInstance().SetDebugMode();
        XDGCommonImpl.GetInstance().updateConfigFileName("XDConfig.json");
        XDGCommon.InitSDK(((success, msg) => {
            if (success){
                ResultText.text = $"初始化成功 {success} {msg}";
            } else{
                ResultText.text = $"初始化失败 {success} {msg}";
            }
        }));

        XDGAccount.AddUserStatusChangeCallback((type, msg) => {
            ResultText.text = $"用户状态回调 code: {type}  msg:{msg}";
        });
    }

    public void InitRelease_CN(){ //国内正式
        isCN = true;
        XDGCommonImpl.GetInstance().SetDebugMode();
        XDGCommonImpl.GetInstance().updateConfigFileName("XDConfig-cn-release.json");
        XDGCommon.InitSDK(((success, msg) => {
            if (success){
                ResultText.text = $"初始化成功 {success} {msg}";
            } else{
                ResultText.text = $"初始化失败 {success} {msg}";
            }
        }));

        XDGAccount.AddUserStatusChangeCallback((type, msg) => {
            ResultText.text = $"用户状态回调 code: {type}  msg:{msg}";
        });
    }

    public void InitDevelop_CN(){ //国内测试
        XDGCommon.ReplaceChannelAndVersion("replace_channel", "replace_version1.0");
        SetDevelopUrl();
       
        isCN = true;
        XDGCommonImpl.GetInstance().SetDebugMode();
        XDGCommonImpl.GetInstance().updateConfigFileName("XDConfig-cn.json");
        XDGCommon.InitSDK(((success, msg) => {
            if (success){
                ResultText.text = $"初始化成功 {success} {msg}";
            } else{
                ResultText.text = $"初始化失败 {success} {msg}";
            }
        }));

        XDGAccount.AddUserStatusChangeCallback((type, msg) => {
            ResultText.text = $"用户状态回调 code: {type}  msg:{msg}";
        });
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
        var loginTypes = new List<LoginType>();
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
            User = user;
            ResultText.text = JsonUtility.ToJson(user);
        }, error => {
            ResultText.text = "登录失败: " + JsonConvert.SerializeObject(error);
            XDGTool.Log("登录失败: " + JsonConvert.SerializeObject(error));
        });
    }

    public void AutoLogin(){
        XDGAccount.LoginByType(LoginType.Default, user => {
                User = user;
                ResultText.text = JsonUtility.ToJson(user);
            },
            error => {
                ResultText.text = "登录失败: " + JsonConvert.SerializeObject(error);
                XDGTool.Log("登录失败: " + JsonConvert.SerializeObject(error));
            });
    }

    public void TypeLogin(){
        var type = GetLoginType(LoginField.text);
        var isTap = (type == LoginType.TapTap) ? true : false;
        
        XDGAccount.LoginByType(type, user => {
                User = user;
                ResultText.text = JsonUtility.ToJson(user);
            },
            error => {
                ResultText.text = "登录失败: " + JsonConvert.SerializeObject(error);
                XDGTool.Log("登录失败: " + JsonConvert.SerializeObject(error));
            });
    }

    public void Logout(){
        XDGAccount.Logout();
    }

    public void GetUserInfo(){
        XDGAccount.GetUser((user) => {
                User = user;
                ResultText.text = JsonUtility.ToJson(user);
            },
            (error) => {
                ResultText.text = "失败: " + error.ToJSON();
            });
    }

    public void OpenUserCenter(){
        XDGAccount.OpenUserCenter();
    }

    public void OpenFeedbackCenter(){
        XDGCommon.Report("serverId", User.userId, "roleName");
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
        XDGAccount.OpenUnregister();
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

        XDGPayment.PayWithProduct("", productId, User.userId, "serverId", "ext",
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

        XDGPayment.PayWithProduct("", productId, User.userId, "serverId", "ext",
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
        
        var price = 0.1;
        var productId = "com.xd.sdkdemo1.stone30";
        if (tmp == "1"){
            price = 6.00;
            productId = "com.xd.sdkdemo1.package";
        } else if (tmp == "2"){
            price = 60.00;
            productId = "com.xd.sdkdemo1.stone60";
        } else if (tmp == "3"){
            price = 300.00;
            productId = "com.xd.sdkdemo1.stone300";
        }

        ResultText.text = $"支付 {productId}";

        var serverId = WebPayServiceField.text;
        if (string.IsNullOrEmpty(serverId)){
            serverId = "90001";
        }

        XDGPayment.PayWithWeb(
            "",
            productId,
            productId,
            price,
            User.userId,
            serverId, 
            "ext",
            (type, msg) => {
                ResultText.text = $"网页支付结果： {type}  msg:{msg}";
            });
    }

    public void isFacebookTokenActive(){
        XDGAccount.IsTokenActiveWithType(LoginType.Facebook, (b) => {
            ResultText.text = $"结果：{b}";
        });
    }

    public void getFacebookToken(){
        XDGAccountImpl.GetInstance().GetFacebookToken((uid, token) => {
            ResultText.text = $"uid：{uid},  token: {token}";
        }, (error) => {
            ResultText.text = $"失败：{JsonUtility.ToJson(error)}";
        });
    }

    public void bindType(){
        var type = GetLoginType(BindField.text);
        XDGAccount.BindByType(type, (b, error) => {
            ResultText.text = $"绑定结果：{b},  error:{JsonUtility.ToJson(error)}";
        });
    }

    public async void gouHuoApiTest(){
        var success = await TapLogin.GetTestQualification();
        ResultText.text = $"篝火result： {success}";
    }

    public void test(){
       // var str = "{\"error\":{\"code\":40902, \"error_msg\":\"第三方登录方式绑定邮箱关联账号已存在当前登录方式的另一个账号绑定\", \"extra_data\":{\"loginType\":\"google\", \"conflicts\":[{\"loginType\":\"taptap\", \"userId\":\"386882063988707329\"}], \"email\":\"z1969910954@gmail.com\"}}}";
       var str = "{\n  \"error\" : {\n    \"error_msg\" : \"当前地区无法使用注册\",\n    \"extra_data\" : \"\",\n    \"code\" : 40310\n  }\n}"; 
       
       var contentDic = Json.Deserialize(str) as Dictionary<string, object>;
        var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(contentDic, "error");

        if (errorDic != null){
            var aa = new XDGError(errorDic);
            XDGTool.Log(JsonConvert.SerializeObject(aa));
        }
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
}