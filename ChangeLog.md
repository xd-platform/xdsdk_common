## 6.5.3
```
替换 Android iOS Native 的 Library
修复 Unity 2020.3.15 之前版本 Android 导出的问题
依赖TapSDK 3.16.0
```
## 6.5.1
```
谷歌支付结算库billing升级到4.1.0 
修复防沉迷国内海外跳转问题
移除TapDB用的 oaid_1.0.23.aar (海外不需要用，国内游戏自行下载添加这个v25版本的) https://github.com/xd-platform/xd_sdk_resource/blob/master/Other/oaid_sdk_1.0.25.aar 
依赖TapSDK 3.13.0,  LCSDK 0.10.12
```

## 6.5.0
```
安卓添加苹果网页登录
添加越南语支持
防止同一个邮箱账号裂开处理
用户中心游客账户在无第三方绑定时不再显示删除账号按钮
```

## 6.4.3
```
移除XDSDK.Datastorage
修复时间读取解析问题 
```

## 6.4.2
```
long parse 解析失败 try catch 
```


## 6.4.1
```
Facebook token自动登录刷新
解决防沉迷库冲突问题
```

## 6.4.0
```
之前的 海外6.3.1 和 国内6.2.4 的SDK合并成一个XDSDK6.4.0

海外6.3.1: https://github.com/xd-platform/sdk_intl_common_upm
国内6.2.4: https://github.com/xd-platform/sdk_cn_common_upm
```