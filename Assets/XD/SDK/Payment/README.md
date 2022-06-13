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
(依赖的仓库地址)
* [TapTap.Common](https://github.com/TapTap/TapCommon-Unity.git)
* [TapTap.Bootstrap](https://github.com/TapTap/TapBootstrap-Unity.git)
* [TapTap.Login](https://github.com/TapTap/TapLogin-Unity.git)
* [TapTap.TapDB](https://github.com/TapTap/TapDB-Unity.git)
* [LeanCloud](https://github.com/leancloud/csharp-sdk-upm)

#### 购买商品
```
XDGPayment.PayWithProduct(orderIdStr, productId, "roleID", "serverId", "ext",
wrapper =>
{
XDGTool.Log("支付结果" + JsonUtility.ToJson(wrapper));
if (wrapper.xdgError != null)
{
logText = "支付商品错误 :" + wrapper.xdgError.ToJSON();
}
else
{
logText = "支付商品结果订单数据: " + JsonUtility.ToJson(wrapper);
}
});
```

#### 网页支付
```
 XDGPayment.PayWithWeb("1", "4332464624", error => { logText = "网页支付:" + error.ToJSON(); });
```

#### 查询商品
```

            XDGPayment.QueryWithProductIds(productIds.Split(','), info =>
            {
                XDGTool.Log("查询商品结果" + JsonUtility.ToJson(info));
                if (info.xdgError != null)
                {
                    logText = "查询商品错误：" + info.xdgError.ToJSON();
                }
                else
                {
                    logText = "查询商品结果：";
                    foreach (var t in info.skuDetailList)
                    {
                        logText += JsonUtility.ToJson(t);
                    }
                }
            });
```

#### 未完成订单
```
  XDGPayment.QueryRestoredPurchase(list =>
            {
                XDGTool.Log("未完成订单" + JsonUtility.ToJson(list));
                logText = "未完成订单: ";
                foreach (var t in list)
                {
                    logText += JsonUtility.ToJson(t);
                }
            });
```

#### 补单
```
 XDGPayment.RestorePurchase("info", "orderId", productId, "4332464624", "serverId", "ext", wrapper =>
            {
                if (wrapper.xdgError != null)
                {
                    logText = "补单错误: xdgError -> " + wrapper.xdgError.ToJSON();
                }
            });
```

#### 获取补单列表数据
```
  XDGPayment.CheckRefundStatus((wrapper) =>
            {
                XDGTool.Log("获取补单列表数据" + JsonUtility.ToJson(wrapper));
                if (wrapper.xdgError != null)
                {
                    logText = wrapper.xdgError.error_msg;
                }
                else
                {
                    var list = wrapper.refundList;
                    if (list != null && list.Count > 0)
                    {
                        var tempText = "";
                        for (var i = 0; i < list.Count; i++)
                        {
                            tempText += JsonUtility.ToJson(list[i]);
                        }

                        logText = "需要补单：" + tempText;
                    }
                    else
                    {
                        logText = "没有需要补单的单子";
                    }
                }
            });
```

#### 获取补单列表数据(带UI) 
```
  XDGPayment.CheckRefundStatusWithUI((wrapper) =>
            {
                XDGTool.Log("获取补单列表数据" + JsonUtility.ToJson(wrapper));
                if (wrapper.xdgError != null)
                {
                    logText = wrapper.xdgError.error_msg;
                }
                else
                {
                    var list = wrapper.refundList;
                    if (list != null && list.Count > 0)
                    {
                        logText = "需要补单：" + JsonUtility.ToJson(list);
                    }
                    else
                    {
                        logText = "没有需要补单的单子";
                    }
                }
            });
```