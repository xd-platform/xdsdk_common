# XDSDK
## 1.在Packages/manifest.json中加入如下引用
```
    //1. 通过UPM方式添加引用 (外网使用) 
    "com.leancloud.realtime": "https://github.com/leancloud/csharp-sdk-upm.git#realtime-0.10.12",
    "com.leancloud.storage": "https://github.com/leancloud/csharp-sdk-upm.git#storage-0.10.12",
    "com.taptap.tds.bootstrap": "https://github.com/TapTap/TapBootstrap-Unity.git#3.16.0",
    "com.taptap.tds.common": "https://github.com/TapTap/TapCommon-Unity.git#3.16.0",
    "com.taptap.tds.login": "https://github.com/TapTap/TapLogin-Unity.git#3.16.0",
    "com.taptap.tds.tapdb": "https://github.com/TapTap/TapDB-Unity.git#3.16.0",
    "com.xd.sdk.common": "https://github.com/xd-platform/xdsdk_common.git#6.5.3",
    "com.xd.sdk.account": "https://github.com/xd-platform/xdsdk_account.git#6.5.3",
    "com.xd.sdk.payment": "https://github.com/xd-platform/xdsdk_payment.git#6.5.3",          //可选，没有支付可以不加
    "com.xd.sdk.oversea": "https://github.com/xd-platform/xdsdk_oversea.git#6.5.3",          //可选，海外用的，国内不加
    "com.tapsdk.antiaddiction":"https://github.com/taptap/TapAntiAddiction-Unity.git#3.16.0", //可选,不是国内没有防沉迷可以不加，防沉迷需要游戏自行接入,XDSDK里不包含 
    
    //2. 通过NPM方式添加引用 (内网可用)
    "com.leancloud.realtime": "0.10.12",
    "com.leancloud.storage": "0.10.12",
    "com.taptap.tds.bootstrap": "3.16.0",
    "com.taptap.tds.common": "3.16.0",  
    "com.taptap.tds.login": "3.16.0",
    "com.taptap.tds.tapdb": "3.16.0",
    "com.xd.sdk.common": "6.5.3",
    "com.xd.sdk.account": "6.5.3",
    "com.xd.sdk.payment": "6.5.3",           //可选，没有支付可以不加
    "com.xd.sdk.oversea": "6.5.3",      //可选，海外用的，国内不加
    "com.tapsdk.antiaddiction":"3.16.0",      //可选,不是国内没有防沉迷可以不加，防沉迷需要游戏自行接入,XDSDK里不包含 
    
    "scopedRegistries": [
    {
      "name": "XDSDK",
      "url": "http://npm.xindong.com",
      "scopes": [
        "com.xd"
      ]
    },
    {
      "name": "TapSDK",
      "url": "https://nexus.tapsvc.com/repository/npm-registry/",
      "scopes": [
        "com.tapsdk",
        "com.taptap",
        "com.leancloud"
      ]
    }
  ]
```

## 2.配置SDK
#### 
* 将 [TDS-Info.plist](https://github.com/xd-platform/xd_sdk_resource/tree/master/640/TDS-Info.plist) 放在 `/Assets/Plugins` 中,
* 将 `XDConfig.json` 放在 `/Assets/Plugins` 中 ([海外配置下载这个文件](https://github.com/xd-platform/xd_sdk_resource/tree/master/640/oversea/XDConfig.json),  [国内配置下载这个文件](https://github.com/xd-platform/xd_sdk_resource/tree/master/640/mainland/XDConfig.json))
* 可选配置：如果有用Firebase，需要将`GoogleService-Info.plist`放在`/Assets/Plugins/iOS`中(iOS用)，将`google-services.json`放在`/Assets/Plugins/Android`中(Android用)，这个两个文件是游戏从[Firebase后台](https://console.firebase.google.com)下载的。
* 可选配置：如果有用Twitter或Firebase，需要将[AndroidManifest.xml](https://github.com/xd-platform/xd_sdk_resource/tree/master/640/oversea/AndroidManifest.xml) 放在`/Assets/Plugins/Android`中，按需修改。

## 3.接口使用
#### 切换语言
```
XDGCommon.SetLanguage(LangType);

国内默认中文，海外默认英文
ZH_CN //简体中文
ZH_TW //繁体中文
EN //英文
TH //泰语
ID //印尼语
KR //韩语
JP //日语
DE //德语
FR //法语
PT //葡萄牙语
ES //西班牙语
TR //土耳其语
RU //俄语
```

#### 初始化SDK
```
1. 需要初始化成功才可以使用SDK登录
2. 初始化前需要先配置好 XDConfig.json 配置文件

 XDGCommon.InitSDK(((success, msg) => {
            if (success){
                ResultText.text = $"初始化成功 {success} {msg}";
                //成功后才可以使用登录
                
            } else{
                ResultText.text = $"初始化失败 {success} {msg}";
            }
        }));
```

#### 是否初始化
```
 XDGCommon.IsInitialized(success => { 
      if(success){
           //已经初始化成功
      }else{
           //没有初始化
      }
  });
```

#### 根据登录类型登录
```
1.要使用Tap登录前，需要现在Tap开发者后台里配置安卓包名、签名文件MD5、iOS Bundle ID，然后在测试用户列表里加入Tap账号id, 之后才可以用Tap APP登录。
2.登录流程：先LoginByType(Default)自动登录(自动登录: 以上次登录成功过的账户继续登录)，如果自动登录失败，再显示Tap 或 游客登录按钮给用户点击授权登录。
3.登录成功后需要调用 【XDGCommon.TrackUser();】, TapDB 统计用户。
4.登录成功后如果要使用 TDSUser 信息，需要执行 【var tdsUser = await TDSUser.GetCurrent().Result.Fetch();】 后才可以使用。
5.这里的登录成功指的是 XDSDK 登录成功，如果是国内需要接入防沉迷模块，需要在登录成功后游戏自行接入防沉迷接口，之后才是游戏最终登录成功。防沉迷实名认证需要在Tap开发者后台单独申请。
6.不同类型登录都需要配置信息和后台开通权限，如Tap登录需要在Tap开发者后台配置信息，然后在XDConfig.json里配置信息，然后XD服务端开通权限

 XDGAccount.LoginByType(LoginType, user => {
                //登录成功后调用 XDGCommon.TrackUser();  //tap db用户统计
                
              },error => {
                 //登录失败
             });
LoginType 类型：             
Default, //自动登录
TapTap,  //Tap登录
Google,  //谷歌登录
Facebook, //Facebook登录
Apple,   //iOS 苹果登录
LINE,    //LINE登录
Twitter, //Twitter登录 
Guest,    //游客登录  
Steam    //Steam登陆            
```

#### SDK自带弹框的登录
```
添加弹框显示的按钮，根据需要添加
也需参考上面单点登录6条注意事项

var loginTypes = new List<LoginType>();
loginTypes.Add(LoginType.TapTap);
loginTypes.Add(LoginType.Guest);
loginTypes.Add(LoginType.Google);
loginTypes.Add(LoginType.Facebook);
loginTypes.Add(LoginType.Twitter);
loginTypes.Add(LoginType.LINE);
loginTypes.Add(LoginType.Steam);

XDGAccount.Login(loginTypes, user => {
      //登录成功后调用 XDGCommon.TrackUser();  //tap db用户统计
      
}, error => {
       ResultText.text = error.error_msg;
       
});

```

#### 绑定用户状态回调(绑定，解绑，退出)
```
 注意，游戏需要在 LOGOUT 回调里做跳转到游戏登录页面的动作
 可以在登录成功后绑定状态回调
 XDGAccount.AddUserStatusChangeCallback((type, msg) => {
        if(type == XDGUserStatusCodeType.LOGOUT){
            //切换到游戏登录页面
        }
       ResultText.text = $"用户状态回调 code: {type}  msg:{msg}";
});

type类型有：
LOGOUT  //SDK退出登录了
BIND     //绑定了地方， msg是第三方名称
UNBIND   //解绑了第三方， msg是第三方名称
ERROR    //未知错误
```

#### 打开用户中心
```
  XDGAccount.OpenUserCenter();
```

#### 获取用户信息
```
 XDGAccount.GetUser((user) => {
       /成功
  },(error) => {
      ResultText.text = "失败: " + error.ToJSON();
});
```

#### 跳转客服反馈
```
   通过在 XDSDKConfig.json 里配置客服链接 report_url
  XDGCommon.Report(serverId, roleId, roleName);
```

#### 跳转应用评分
```
  XDGCommon.StoreReview();
```

#### 获取协议列表
```
  XDGCommon.GetAgreementList(getAgreementCallback);
```

#### 显示协议
```
  XDGCommon.ShowDetailAgreement(agreementUrl);
```

#### 打开注销页面
```
  通过在 XDSDKConfig.json 里配置注销链接 logout_url
   XDGAccount.OpenUnregister();
```

#### 退出登录
```
  XDGAccount.Logout();
```

#### 自定义退出接口
```
  XDGCommon.SetExitHandler(customExitAction);
```

#### 支付
```
1. 使用前需要把商品信息给 XDSDK 服务端配置好
2. 游戏服务端需要对接 XDSDK 服务端，是否支付成功以XDSDK服务端通知为准
3. iOS的苹果支付和安卓的谷歌支付是用的同一个接口，需要区分平台使用不同的参数

XDGPayment.PayWithProduct(roderId, productId, roleId, serverId, extras,
         wrapper => {
             XDGTool.Log("支付结果" + JsonUtility.ToJson(wrapper));
             if (wrapper.xdgError != null){
                  ResultText.text = "支付失败 :" + wrapper.xdgError.ToJSON();
             } else{
                  ResultText.text = "支付完成: " + JsonUtility.ToJson(wrapper);
            }
});
```

#### 安卓打开网页支付
```
1. 使用前需要把商品信息给 XDSDK 服务端配置好
2. 只有安卓可以使用网页支付（国内海外都可以，通过在 XDSDKConfig.json 里配置支付链接 web_pay_url）
3. 游戏服务端需要对接 XDSDK 服务端，是否支付成功以XDSDK服务端通知为准

 XDGPayment.PayWithWeb(
            roderId,
            productId,
            productName,
            price,
            roleId,
            serverId, 
            extras,
            (type, msg) => {
                ResultText.text = $"网页支付结果： {type}  msg:{msg}";
            });
 
type类型有：           
OK  //完成，(支付成功要以服务端通知为准) 
Cancel //取消
Processing //处理中
Error //出错
```

#### 安卓查询谷歌商品信息
```
XDGPayment.QueryWithProductIds(productIds, info => {
    XDGTool.Log("查询结果" + JsonUtility.ToJson(info));
    if (info.xdgError != null){
     ResultText.text = "失败：" + info.xdgError.ToJSON();
   } else{
     ResultText.text = JsonUtility.ToJson(info);
  }
});
```

#### 查询商品信息
```
1. iOS苹果商品信息 和 安卓谷歌商品信息查询，需要区分平台使用不同参数
2. 网页支付的商品信息不可以查询
XDGPayment.QueryWithProductIds(productIds, info => {
    XDGTool.Log("查询结果" + JsonUtility.ToJson(info));
    if (info.xdgError != null){
     ResultText.text = "失败：" + info.xdgError.ToJSON();
   } else{
     ResultText.text = JsonUtility.ToJson(info);
  }
});
```

#### 检查补款信息(无弹框)
```
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
```

#### 检查补款信息(SDK自带弹框)
```
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
```

#### 检查Facebook Token是否可用(目前只支持FB)
```
XDGAccount.IsTokenActiveWithType(LoginType.Facebook, (b) => {
            ResultText.text = $"结果：{b}";
        });
```

#### 绑定第三方
```
LoginType 除了 Default 和 Guest, 其他第三方类型都可以
XDGAccount.BindByType(LoginType, (success, error) => {
   ResultText.text = $"绑定结果：{success},  error:{JsonUtility.ToJson(error)}";
});
```





#### Android 11 相关配置

如果您的应用以 Android 11（API 级别 30）或更高版本为目标平台，在默认情况下，系统会自动让部分应用对您的应用可见，但会隐藏其他应用。
通过让部分应用在默认情况下不可见，系统可以了解应向您的应用显示哪些其他应用，这样有助于鼓励最小权限原则，还可帮助 Google Play 等应用商店评估应用为用户提供的隐私权和安全性。
针对 XDSDK 支持的登录类型的平台，在手机安装有对应 App 的情况下，无法正常跳转外部 App 进行授权登录时请参考以下配置项进行软件包可见性的适配配置，将配置添加到 `AndroidManifest.xml` 文件中：

```
<!--添加如下 queries 标签的内容，直接在 manifest 标签下-->
<queries>
    <!-- TapTap -->
    <package android:name="com.taptap"/>
    <package android:name="com.taptap.pad"/>
    <package android:name="com.taptap.global"/>

    <!-- Facebook -->
    <package android:name="com.facebook.katana"/>
    <!-- Facebook 有端分享 -->
    <provider android:authorities="com.facebook.katana.provider.PlatformProvider"/>
    <!-- Twitter -->
    <package android:name="com.twitter.android"/>
    <!-- LINE -->
    <package android:name="jp.naver.line.android"/>
</queries>
```