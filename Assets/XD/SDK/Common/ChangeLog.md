## Unity v6.3.1
```
[Android]
优化 Google 支付
```

## Unity v6.3.0
```
[iOS]
新增登录流程、支付流程埋点(阿里云SLS)
隐私协议弹窗 UI 优化

[Android]
新增登录流程、支付流程埋点(阿里云SLS)
隐私协议弹窗 UI 优化
网页支付新增返回按钮
```

## Unity v6.2.1
```
[iOS]
1. 增加IDFA开启选项： 
   1.1 需要在初始化前调用 XDGCommon.EnableIDFA(true); 
   1.2 TDS-Info.plist 中添加如下信息
       <key>NSUserTrackingUsageDescription</key>
       <string>想要获取IDFA</string>
2. 设置 XDGSDK 语言时会同步到 TapSDK

[Android]
1. 修复Android zendesk 客服系统打开的语言问题
2. 设置 XDGSDK 语言时会同步到 TapSDK
```
## Unity v6.2.0
```
安卓：网页支付新增商品id、附加参数字段
安卓：修复加州协议点击无反应
```
## Unity v6.1.2
```
添加iOS注销页面url
```
## Unity v6.1.1
```
版本内容：(6.0.2 bugfix移植版)
增加应用配置、用户信息的缓存机制，减少初始化、自动登录失败的问题。
增加初始化、登录、内建账号流程的失败原因log上报
移除单点登录流程中「登录成功」toast 提示
修复 Windows 环境下无法进行 Android 打包的问
TDSSDK 升级至 3.6.1，LC SDK 升级至 0.10.2, Unity内建账号采用本地构建
iOS 安卓 Unity 三端的AppsFlyer SDK 统一升级到 6.5.2，修复打点问题
「Android」修复登录失败、登录取消后无回调的问题
「Android」修复登录成功后无用户绑定信息的问题
「Android」修复唤起弹窗时有概率闪退的问题
「Android」修复网页支付无法唤起第三方支付 App 的问题（包括 Toss、Go-Pay、LINE Pay）
「Android」修复网页支付滑动卡顿问题
「iOS」修复签名时间有概率不一致，导致无法登录、支付的问题
「iOS」修复初始化失败时 App ID 为空，导致用户支付掉单的问题
「iOS」修复初始化失败，导致用户国家/地区信息为空的问题
「iOS」增加初始化和自动登录缓存
「iOS」修复 appid 为空 票据上报问题
「iOS」修复初始化失败导致 countryCode 为空问题
「iOS」增加IP缓存
「iOS」FaceBook分享图片优化
注意：
由于6.1.1把内建账号采用本地构建了(TDSUser里加入sessionToken和objectId)，要用TDSUser其他信息，需要 user.fetch() 拉取一下。
```

## Unity v6.0.2 (Android v6.0.3, iOS v6.0.2)
```
### Android BugFix
* 增加本地缓存优化初始化接口不稳定情况下无法初始化问题，增加初始化失败具体原因的 msg 字段(缓存初次初始化配置，后台静默更新最新配置)
* 增加自动登录本地用户优先读取本地缓存逻辑，后台静默更新用户信息
* 修复弹窗概率性闪退问题
* 修复获取同步内建账号 token 失败时无法收到回调问题
* 修复 XDGUser boundAccounts 为 null 问题
* 增加网页支付跳转第三方 App 的 scheme 规则
* 修复登录授权取消时无法收到回调问题
* 取消单点登录(loginByType)流程中登录成功 toast 提示
* 修复 Unity 环境下网页支付页面卡帧问题
### iOS BugFix
* 修复签名时间戳不一致的问题
* 增加初始化失败原因
* 增加初始化和自动登录缓存
* 修复 appid 为空 票据上报问题
* 修复初始化失败导致 countryCode 为空问题
### Unity BugFix
* 内建账号使用本地构建
* TapSDK 升级至 3.6.1，LC SDK 升级至 0.10.2 （修复用户为泰国日历时无法登录的问题）
* 初始化失败/登录失败/内建账号失败 信息提交bugly
* windows 环境下打包 Android 的脚本修改(换行符问题)
```

## v6.1.0 (Android v6.0.2, iOS v6.1.0)
```
### Feature
* [Android]游客账号唯一标识逻辑变更
* 支持 Facebook 权限配置（例如获取好友列表权限）
* [Android]第三方 SDK 版本升级：Facebook (12.0.0)、AppsFlyer(6.4.3)、Firebase Core(18.0.0)
* XDGUser 新增 avatar 和 nickName 属性
* [iOS] 增加账号注销功能
### BugFix

* 修复 Windows 环境下 Unity （Android）无法打包问题
* [Android]修复 Unity 同步内建账号失败时，游戏无法收到登录失败回调的问题
* [Android]修复弹窗概率性闪退问题
* [Android]修复 AppsFlyer session 事件上报时机错误问题
* [Android]修复新版本网页支付在 Unity 环境下滑动卡帧问题


### TapSDK Dependencies

* (TapCommon、TapBootstrap、TapLogin、TapDB)： **3.3.1**
```

## v6.0.1 (Android v6.0.1, iOS v6.0.1)
```
### Feature
* [iOS] Facebook SDK 升级版本 12.1.0
* 登录流程优化

### Bugfix
* [Android] 修复断网情况下初始化失败问题
* [Android] 修复 Twitter 授权取消时 Toast 提示文案
* [Android] 修复Unity 桥接 Android 传入 Default 类型自动登录不生效的情况
* [Android] 修复阿拉伯语情况下无法登录问题
```
## v6.0.0 (Android v6.0.0 & iOS v6.0.0)
```
### Feature
* 增加 Common、Account、Payment 三大功能模块
* 一包发全球策略
```