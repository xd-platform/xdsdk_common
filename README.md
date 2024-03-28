# XDGSDK-PC-6.0

## SDK 下载

[下载地址](https://git.xindong.com/xd-platform/xdgsdk-6.0/xdsdk-unity-pc/uploads/464130511f448c3c917196a584228024/xd-pc-sdk.unitypackage)

## 添加依赖

### UPM 依赖

```json
"com.leancloud.storage": "https://github.com/leancloud/csharp-sdk-upm.git#storage-0.10.14",
"com.leancloud.realtime": "https://github.com/leancloud/csharp-sdk-upm.git#realtime-0.10.14",
"com.taptap.tds.bootstrap": "https://github.com/TapTap/TapBootstrap-Unity.git#3.11.0",
"com.taptap.tds.common": "https://github.com/TapTap/TapCommon-Unity.git#3.11.0",
"com.taptap.tds.login": "https://github.com/taptap/TapLogin-Unity.git#3.11.0",
"com.taptap.tds.tapdb": "https://github.com/TapTap/TapDB-Unity.git#3.11.0",
"com.unity.textmeshpro": "3.0.6",
```

### PC 浏览器插件

SDK 中使用到了付费 PC WebView 插件：[3D WebView for Windows and macOS](https://assetstore.unity.com/packages/tools/gui/3d-webview-for-windows-and-macos-web-browser-154144)
内部游戏可以联系 TDS 获得；外部游戏则需要自行购买。将插件导入到 Unity 工程即可。

## 配置信息

与移动端一致，在 Assets/Plugins/Resources 目录下放置 XDConfig.json，只要内容包括：

```json
{
  "client_id": "d4bjgwom9zk84wk",
  "region_type": "CN",
  "game_name": "XD-CN Demo",
  "app_id": "1010",
  "tapsdk": {
    "client_id": "Wzy7xYhKtYdnLUXevV",
    "client_token": "ocXvBmR1EecL7CtmFEugoC016QKiptDC0vS5pMIf",
    "server_url": "https://jfqhf3x9.cloud.tds1.tapapis.cn",
    "permissions": [
      "public_profile",
      "user_friends"
    ]
  },
  "logout_url": "https://logout-xdsdk.xd.cn/",
  "web_pay_url": "https://sdkpay-test.xd.cn/sdk",
  "report_url": "https://www.xd.com/service",
}
```

## 日志调试

```cs
XDLogger.LogDelegate = (level, message) => {
    switch (level) {
        case XDLogLevel.Debug:
            Debug.Log($"[DEBUG] {message}");
            break;
        case XDLogLevel.Warn:
            Debug.LogWarning($"[WARN] {message}");
            break;
        case XDLogLevel.Error:
            Debug.LogError($"[ERROR] {message}");
            break;
        default:
            break;
    }
};
```

## 多语言

### 支持语言种类

```cs
public enum LanguageType {
    ZH_CN = 0,  // 简体中文
    ZH_TW = 1,  // 繁体中文
    EN = 2,     // 英文
    TH = 3,     // 泰文
    ID = 4,     // 印尼文
    KR = 5,     // 韩语
    JP = 6,     // 日语
    DE = 7,     // 德语
    FR = 8,     // 法语
    PT = 9,     // 葡萄牙语
    ES = 10,    // 西班牙语
    TR = 11,    // 土耳其语
    RU = 12,    // 俄罗斯语
}
```

### 设置语言

```cs
XDGCommon.SetLanguage(languageType);
```

## 初始化

```cs
XDGCommon.InitSDK((b, msg) =>
    {
        if(b)
            ResultText.text = "初始化成功";
        else
            ResultText.text = $"初始化失败：{msg}";
    }
);
```

## 获取 SDK 回调

```cs
XDGAccount.AddUserStatusChangeCallback((type, s) =>
    {
        if (type == XDGUserStatusCodeType.BIND)
        {
            ResultText.text = $"绑定成功: {s}";
        }
        else if (type == XDGUserStatusCodeType.UNBIND)
        {
            ResultText.text = $"解绑成功: {s}";
        }
        else if (type == XDGUserStatusCodeType.LOGOUT)
        {
            ResultText.text = $"退出登录";
        }
    }
);
```

## 登录

### 支持登录类型

```cs
public enum LoginType { 
    Default = -1,   // 自动登录，以上次登录成功的信息登录
    Guest  = 0,     // 游客登录
    TapTap = 5,     //Tap 登录
}
```

### 登录

```cs
XDGAccount.LoginByType(LoginType.Default, xdgUser => {
        ResultText.text = $"自动登录成功：{xdgUser.nickName} userId: {xdgUser.userId} kid: {xdgUser.token.kid}";
    }, error => {
        ResultText.text = $"自动登录失败：{error}";
    }
);
```

### 建议用法

- 先自动登录，如果自动登录失败，则调用 Tap 或游客登录
- 为保证登录效率，TDSUser 采用本地创建的方式，如需获取 TDSUser 信息，需要再次拉取

```cs
TDSUser current = await TDSUser.GetCurrent();
await current.Fetch();
```

## 获取用户信息

```cs
XDGAccount.GetUser(user => {
        ResultText.text = $"获取成功：{user.nickName}\n userId: {user.userId}\n kid: {user.token.kid}\n boundAccounts: {JsonConvert.SerializeObject(user.boundAccounts)}";
    }, error => {
        ResultText.text = $"获取失败：code: {error.error_msg}";
    }
);
```

## 打开账号中心

```cs
XDGAccount.OpenUserCenter();
```

## 支付

### 国内支付

目前支持微信和支付宝，采用内置浏览器的 Web 支付方式

```cs
XDGPayment.PayWithWeb(orderId, productId, productName, payAmount, roleId, serverId, extras, (resultType, str) => {
        if (resultType == WebPayResultType.OK) {
            XDLogger.Debug("支付完成");
        } else if (resultType == WebPayResultType.Cancel) {
            XDLogger.Debug("取消支付");
        } else if (resultType == WebPayResultType.Error) {
            XDLogger.Debug($"支付异常: {str}");
        }
    }
);
```

### 海外支付

目前支持跳转官网浏览器支付方式

```cs
XDGPayment.PayWithWeb(sid, rid);
```

## 打开客服中心

```cs
XDGCommon.Report(serverId, roleId, roleName);
```

## 是否初始化

```cs
XDGCommon.IsInitialized(b =>
            ResultText.text = "Wrapper 是否初始化：" + b);
```

## 获取版本号

```cs
XDGCommon.GetVersionName((s) => ResultText.text = "Wrapper 版本号：" + s);
```

## 退出登录

```cs
XDGAccount.Logout();
```

## Windows 平台网页登录唤醒

参考 [文档](https://developer.taptap.com/docs/sdk/taptap-login/guide/start/#windows-%E5%B9%B3%E5%8F%B0)

## 注意

PC SDK 中使用到了付费的 WebView 插件，Vuplex 目录只允许在公司内使用，切勿传播。