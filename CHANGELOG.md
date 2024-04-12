# v6.1.2
```
1.隐藏游客删除按钮
2.增加绑定解绑回调 
3.增加绑定解绑按钮隐藏逻辑
4.tap dp pc模块处理：
   删除如下模块：
   "com.taptap.tds.login": "https://github.com/taptap/TapDB-Unity.git#3.6.3",
   SDK内部包含了TapDB PC 模块，不用通过upm再引入了 (通过upm引入的没有pc模块)
```

# v6.1.1
```
自动登录/游客登录
缓存优化
接口及代码整理
注意:内建账号用本地构建，游戏要获取TesUser信息前需要手动调用一下fetch方法【await TDSUser.GetCurrent().Result.Fetch()】
```

# v6.0.0
```
Tap扫码登录
支持多国语言
网页支付
检查补款
客服
个人中心
```