# XD
## 1.在Packages/manifest.json中加入如下引用
```
    "com.leancloud.realtime": "https://github.com/leancloud/csharp-sdk-upm.git#realtime-0.10.5",
    "com.leancloud.storage": "https://github.com/leancloud/csharp-sdk-upm.git#storage-0.10.5",
    "com.taptap.tds.bootstrap": "https://github.com/TapTap/TapBootstrap-Unity.git#3.6.3",
    "com.taptap.tds.common": "https://github.com/TapTap/TapCommon-Unity.git#3.6.3",
    "com.taptap.tds.login": "https://github.com/TapTap/TapLogin-Unity.git#3.6.3",
    "com.taptap.tds.tapdb": "https://github.com/TapTap/TapDB-Unity.git#3.6.3",
    "com.xd.sdk.common": "https://github.com/xd-platform/xd_sdk_common_upm.git#6.3.0",
    "com.xd.sdk.account": "https://github.com/xd-platform/xd_sdk_account_upm.git#6.3.0",
    "com.xd.sdk.payment": "https://github.com/xd-platform/xd_sdk_payment_upm.git#6.3.0",
    
    "scopedRegistries": [
    {
      "name": "XD SDK",
      "url": "http://npm.xindong.com",
      "scopes": [
        "com.xd"
      ]
    },
    {
      "name": "Game Package Registry by Google",
      "url": "https://unityregistry-pa.googleapis.com",
      "scopes": [
        "com.google"
      ]
    }
  ]
```

## 2.配置SDK
#### iOS配置
* 将TDS-Info.plist 放在 /Assets/Plugins 中
* 将XDG-Info.plist 放在 /Assets/Plugins/iOS 中
* 在Capabilities中打开In-App Purchase、Push Notifications、Sign In With Apple功能

#### Android配置
* 将XDG_info.json、google-Service.json 文件放在 /Assets/Plugins/Android/assets中

[文件下载](https://github.com/xd-platform/xd_sdk_resource/tree/master/Unity_Intl/ConfigFile)

google-Service.json 文件是从[Firebase控制台下载的](https://console.firebase.google.com/)

## 3.命名空间

```
using XD.SDK.Common;
```

## 4.接口使用
#### 切换语言
```
XDGCommon.SetLanguage(LangType.ZH_CN);
```

#### 初始化SDK
1. 使用sdk前需先初始化。

2. v6.2.1 版本中增加IDFA开启选项，`需要在初始化前调用 XDGCommon.EnableIDFA(true)`，然后在 `TDS-Info.plist` 中添加如下信息
```
   <key>NSUserTrackingUsageDescription</key>
   <string>想要获取IDFA</string>
```
```
 XDGCommon.InitSDK((success => {
                if (success){
              
                }else{
                
                }
            }));
```

#### 是否初始化
```
 XDGCommon.IsInitialized(b => { 
  });
```

#### SDK自带弹框的登录
```
 XDGAccount.Login(user={
    
},(error)=>{
    
});
```

#### 根据登录类型登录
```
1. Default 是自动登录 (以上次登录成功的信息登录，如果之前没登录过就会进失败回调)
2. 登录成功后调用 XDGCommon.TrackUser(string userId);  //tap db用户统计
 XDGAccount.LoginByType(LoginType, user => {
              
              },error => {
                
             });
```

#### 埋点
```
  XDGCommon.Report(serverId, roleId, roleName);
  XDGCommon.TrackRole(string serverId, string roleId, string roleName, int level);
  XDGCommon.TrackUser(string userId);  //tap db用户统计
  XDGCommon.TrackEvent(string eventName);
  XDGCommon.TrackAchievement();
```

#### 完成引导教程
```
 XDGCommon.EventCompletedTutorial();
```

#### 创建角色事件
```
  XDGCommon.EventCreateRole();
```

#### 获取版本号
```
XDGCommon.GetVersionName((version) =>{
               
            });
```

#### 通知开关
```
XDGCommon.SetCurrentUserPushServiceEnable(pushServiceEnable);
XDGCommon.IsCurrentUserPushServiceEnable(pushEnable => { 
});
```

#### 分享
```
XDGCommon.Share(ShareFlavors shareFlavors, string imagePath, XDGShareCallback callback);
XDGCommon.Share(ShareFlavors shareFlavors, string uri, string message, XDGShareCallback callback);
```

#### 获取当前获取到的地区信息
```
  XDGCommon.GetRegionInfo(wrapper =>
            {
               if (wrapper != null && wrapper.info != null) {
                    ...
               }
            });
```

#### 绑定用户状态回调(绑定，解绑，退出)
```
 XDGAccount.AddUserStatusChangeCallback((code)={

});
0x9001 退出登录
0x1001 绑定
0x1002 解绑
```

#### 获取用户信息
```
 XDGAccount.GetUser((user)={
   
},(error)=>{
    
});
```

#### 打开用户中心
```
  XDGAccount.OpenUserCenter();
```

#### 退出登录
```
  XDGAccount.Logout();
```


[CHANGE LOG](https://github.com/xd-platform/xd_sdk_common_upm/blob/github_upm/ChangeLog.md)