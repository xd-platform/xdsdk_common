using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common;
using XD.SDK.Common.Internal;
using XD.SDK.Payment.Internal;

namespace XD.SDK.Payment
{
    public class XDGSkuDetailInfo
    {
        public XDGError xdgError;

        public List<SkuDetailBean> skuDetailList;

        public XDGSkuDetailInfo(string jsonStr)
        {
            var dic = Json.Deserialize(jsonStr) as Dictionary<string, object>;
            var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "error");
            if (errorDic != null)
            {
                xdgError = new XDGErrorMobile(errorDic);
            }

            var list = SafeDictionary.GetValue<List<object>>(dic, "products");
            if (list == null) return;
            skuDetailList = new List<SkuDetailBean>();
            foreach (var skuDetail in list)
            {
                var innerDic = skuDetail as Dictionary<string, object>;
                var skuDetailBean = new SkuDetailBean(innerDic);
                skuDetailList.Add(skuDetailBean);
            }
        }
    }
#if UNITY_IOS
    [Serializable]
    public class SkuDetailBean
    {
        public string localizedDescription;

        public string localizedTitle;

        public double price;

        public string productIdentifier;

        public string localeIdentifier;

        public PriceLocale priceLocale;

        public SkuDetailBean(string json)
        {
            Dictionary<string,object> dic = Json.Deserialize(json) as Dictionary<string,object>;
            this.localeIdentifier = SafeDictionary.GetValue<string>(dic,"localeIdentifier");
            this.localizedTitle = SafeDictionary.GetValue<string>(dic,"localizedTitle") ;
            this.price = SafeDictionary.GetValue<double>(dic, "price");
            this.productIdentifier = SafeDictionary.GetValue<string>(dic,"productIdentifier");
            Dictionary<string,object> priceLocaleDic = SafeDictionary.GetValue<Dictionary<string,object>>(dic,"priceLocale") as Dictionary<string,object>;
            this.priceLocale = new PriceLocale(priceLocaleDic);
        }

        public SkuDetailBean(Dictionary<string,object> dic)
        {
            this.localeIdentifier = SafeDictionary.GetValue<string>(dic,"localeIdentifier");
            this.localizedTitle = SafeDictionary.GetValue<string>(dic,"localizedTitle") ;
            this.price =SafeDictionary.GetValue<double>(dic,"price");
            this.productIdentifier = SafeDictionary.GetValue<string>(dic,"productIdentifier");
            Dictionary<string,object> priceLocaleDic = SafeDictionary.GetValue<Dictionary<string,object>>(dic,"priceLocale") as Dictionary<string,object>;
            this.priceLocale = new PriceLocale(priceLocaleDic);
        }

        public SkuDetailBean()
        {

        }

        public string ToJSON(){
            return JsonUtility.ToJson(this);
        }
        

    }
    [Serializable]
    public class PriceLocale
    {
        public string localeIdentifier;

        public string languageCode;

        public string countryCode;

        public string scriptCode;

        public string calendarIdentifier;

        public string decimalSeparator;

        public string currencySymbol;

        public PriceLocale(Dictionary<string,object> dic)
        {
            this.localeIdentifier = SafeDictionary.GetValue<string>(dic,"localeIdentifier");
            this.languageCode = SafeDictionary.GetValue<string>(dic,"languageCode");
            this.countryCode = SafeDictionary.GetValue<string>(dic,"countryCode");
            this.scriptCode = SafeDictionary.GetValue<string>(dic,"scriptCode");
            this.calendarIdentifier = SafeDictionary.GetValue<string>(dic,"calendarIdentifier");
            this.decimalSeparator = SafeDictionary.GetValue<string>(dic,"decimalSeparator");
            this.currencySymbol = SafeDictionary.GetValue<string>(dic,"currencySymbol");
        }
    }

#elif UNITY_ANDROID
    [Serializable]
    public class SkuDetailBean
    {
        public string description;

        public string freeTrialPeriod;

        public string iconUrl;

        public string introductoryPrice;

        public long introductoryPriceAmountMicros;

        public int introductoryPriceCycles;

        public string originJson;

        public string originPrice;

        public long originPriceAmountMicros;

        public string price;

        public long priceAmountMicros;

        public string priceCurrencyCode;

        public string productId;

        public string subscriptionPeriod;

        public string title;

        public string type;

        public SkuDetailBean(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            description = SafeDictionary.GetValue<string>(dic, "description");
            freeTrialPeriod = SafeDictionary.GetValue<string>(dic, "freeTrialPeriod");
            iconUrl = SafeDictionary.GetValue<string>(dic, "iconUrl");
            introductoryPrice = SafeDictionary.GetValue<string>(dic, "introductoryPrice");
            introductoryPriceAmountMicros = SafeDictionary.GetValue<long>(dic, "introductoryPriceAmountMicros");
            introductoryPriceCycles = SafeDictionary.GetValue<int>(dic, "introductoryPriceCycles");
            originJson = SafeDictionary.GetValue<string>(dic, "originJson");
            originPrice = SafeDictionary.GetValue<string>(dic, "originPrice");
            originPriceAmountMicros = SafeDictionary.GetValue<long>(dic, "originPriceAmountMicros");
            price = SafeDictionary.GetValue<string>(dic, "price");
            priceAmountMicros = SafeDictionary.GetValue<long>(dic, "priceAmountMicros");
            priceCurrencyCode = SafeDictionary.GetValue<string>(dic, "priceCurrencyCode");
            productId = SafeDictionary.GetValue<string>(dic, "productId");
            subscriptionPeriod = SafeDictionary.GetValue<string>(dic, "subscriptionPeriod");
            title = SafeDictionary.GetValue<string>(dic, "title");
            type = SafeDictionary.GetValue<string>(dic, "type");
        }
    }
#else
     public class SkuDetailBean
     {
         public SkuDetailBean(Dictionary<string,object> dic)
         {
            
         }

         public string ToJSON(){
            return JsonUtility.ToJson(this);
        }
     }
#endif

    public class XDGRestoredPurchaseWrapper
    {
        public List<XDGRestoredPurchase> transactionList;

        public XDGRestoredPurchaseWrapper(string jsonStr)
        {
            var dic = Json.Deserialize(jsonStr) as Dictionary<string, object>;
            var list = SafeDictionary.GetValue<List<object>>(dic, "transactions");
            if (list == null)
            {
                return;
            }

            transactionList = new List<XDGRestoredPurchase>();
            foreach (var obj in list)
            {
                var beanDic = obj as Dictionary<string, object>;
                transactionList.Add(new XDGRestoredPurchase(beanDic));
            }
        }
    }

#if UNITY_IOS
    [Serializable]
    public class XDGRestoredPurchase
    {
        public string transactionIdentifier;

        public string productIdentifier;

        public XDGRestoredPurchase(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public XDGRestoredPurchase(Dictionary<string,object> dic)
        {
            this.transactionIdentifier = SafeDictionary.GetValue<string>(dic,"transactionIdentifier") ;
            this.productIdentifier = SafeDictionary.GetValue<string>(dic,"productIdentifier") ;
        }
    }

#elif UNITY_ANDROID
    [Serializable]
    public class XDGRestoredPurchase
    {
        public string orderId;

        public string packageName;

        public string productId;

        public long purchaseTime;

        public string purchaseToken;

        public int purchaseState;

        public string developerPayload;

        public bool acknowledged;

        public bool autoRenewing;

        public XDGRestoredPurchase(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public XDGRestoredPurchase(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            orderId = SafeDictionary.GetValue<string>(dic, "orderId");
            packageName = SafeDictionary.GetValue<string>(dic, "packageName");
            productId = SafeDictionary.GetValue<string>(dic, "productId");
            purchaseTime = SafeDictionary.GetValue<long>(dic, "purchaseTime");
            purchaseToken = SafeDictionary.GetValue<string>(dic, "purchaseToken");
            purchaseState = SafeDictionary.GetValue<int>(dic, "purchaseState");
            developerPayload = SafeDictionary.GetValue<string>(dic, "developerPayload");
            acknowledged = SafeDictionary.GetValue<bool>(dic, "acknowledged");
            autoRenewing = SafeDictionary.GetValue<bool>(dic, "autoRenewing");
        }
    }
#else 
    public class XDGRestoredPurchase
    {
        public XDGRestoredPurchase(Dictionary<string,object> dic)
        {

        }

        public XDGRestoredPurchase(string json)
        {
            
        }
    }
#endif
    public class XDGOrderInfoWrapper
    {
        public XDGError xdgError;
        public XDGOrderInfo orderInfo;

        public XDGOrderInfoWrapper(string jsonStr)
        {
            var dic = Json.Deserialize(jsonStr) as Dictionary<string, object>;
            var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "error");
            if (errorDic != null)
            {
                xdgError = new XDGErrorMobile(errorDic);
                XDGTool.LogError($"产品支付失败： {jsonStr}");
            }

            var orderInfoDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "orderInfo");
            orderInfo = new XDGOrderInfo(orderInfoDic);
        }
    }

    [Serializable]
    public class XDGOrderInfo
    {
        public string orderId;

        public string productId;

        public string roleId;

        public string serverId;

        public string ext;

        public XDGOrderInfo(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            orderId = SafeDictionary.GetValue<string>(dic, "orderId");
            productId = SafeDictionary.GetValue<string>(dic, "productId");
            roleId = SafeDictionary.GetValue<string>(dic, "roleId");
            serverId = SafeDictionary.GetValue<string>(dic, "serverId");
            ext = SafeDictionary.GetValue<string>(dic, "ext");
        }
    }

    public class XDGRefundResultWrapperMobile : XDGRefundResultWrapper
    {
        private XDGError _xdgError;
        private List<XDGRefundDetails> _refundList;

        public XDGRefundResultWrapperMobile(string jsonStr)
        {
            var dic = Json.Deserialize(jsonStr) as Dictionary<string, object>;
            var code = SafeDictionary.GetValue<int>(dic, "code");
            var msg = SafeDictionary.GetValue<string>(dic, "msg");
            if (code != Result.RESULT_SUCCESS)
            {
                XDGTool.LogError($"CheckRefundResult 失败 :{jsonStr}");
                _xdgError = new XDGErrorMobile(code, msg);
            }
            else
            {
                var dataDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "data");
                if (dataDic == null) return;
                var list = SafeDictionary.GetValue<List<object>>(dataDic, "list");
                if (list == null)return;
                
                _refundList = new List<XDGRefundDetails>();
                for (var index = 0; index < list.Count; index++)
                {
                    var obj = list[index];
                    var beanDic = obj as Dictionary<string, object>;
                    _refundList.Add(new XDGRefundDetailsMobile(beanDic));
                }
            }
        }

        public XDGError xdgError => _xdgError as XDGError;
        public List<XDGRefundDetails> refundList => _refundList.ConvertAll<XDGRefundDetails>(a => a as XDGRefundDetails);
    }

    [Serializable]
    public class XDGRefundDetailsMobile : XDGRefundDetails
    {
        private string _tradeNo;
        private string _productId;
        private string _currency;
        private string _outTradeNo;
        private double _refundAmount;
        private int _supplyStatus;
        private int _platform;
        private int _channelType;

        public XDGRefundDetailsMobile(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            _tradeNo = SafeDictionary.GetValue<string>(dic, "tradeNo");
            _productId = SafeDictionary.GetValue<string>(dic, "productId");
            _currency = SafeDictionary.GetValue<string>(dic, "currency");
            _outTradeNo = SafeDictionary.GetValue<string>(dic, "outTradeNo");
            _refundAmount = SafeDictionary.GetValue<double>(dic, "refundAmount");
            _supplyStatus = SafeDictionary.GetValue<int>(dic, "supplyStatus");
            _platform = SafeDictionary.GetValue<int>(dic, "platform");
            _channelType = SafeDictionary.GetValue<int>(dic, "channelType");
        }

        public string tradeNo => _tradeNo;

        public string productId => _productId;

        public string currency => _currency;

        public string outTradeNo => _outTradeNo;

        public double refundAmount => _refundAmount;

        public int supplyStatus => _supplyStatus;

        public int platform => _platform;

        public int channelType => _channelType;
    }

    public class XDGInlinePayResult
    {
        public int code = -1;
        public string message = "";
        
        public XDGInlinePayResult(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> dic)) return;
            code = SafeDictionary.GetValue<int>(dic, "code");
            message = SafeDictionary.GetValue<string>(dic, "message");
        }

        public XDGInlinePayResult(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }


    public class ProductSkuWrapper
    {
        public XDGError xdgError;

        public List<ProductSkuInfo> skuList;
        
        public ProductSkuWrapper(string jsonStr)
        {
            var dic = Json.Deserialize(jsonStr) as Dictionary<string, object>;
            var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "error");
            if (errorDic != null)
            {
                xdgError = new XDGErrorMobile(errorDic);
            }

            var list = SafeDictionary.GetValue<List<object>>(dic, "products");
            if (list == null) return;
            skuList = new List<ProductSkuInfo>();
            foreach (var skuDetail in list)
            {
                var innerDic = skuDetail as Dictionary<string, object>;
                var skuDetailBean = new ProductSkuInfo(innerDic);
                skuList.Add(skuDetailBean);
            }
        }

    }

    public class ProductSkuInfo
    {
        public string channelSkuCode; // 渠道 sku
        public string costPrice; // 原价
        public string currency; // 货币
        public string desc; // 商品描述
        public string productSkuCode; // 商品 sku
        public string productSkuName; // 商品 sku 的名称
        public string region; // 国家/地区
        public string salePrice; // 现价

        public ProductSkuInfo(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            channelSkuCode = SafeDictionary.GetValue<string>(dic, "channelSkuCode");
            costPrice = SafeDictionary.GetValue<string>(dic, "costPrice");
            currency = SafeDictionary.GetValue<string>(dic, "currency");
            desc = SafeDictionary.GetValue<string>(dic, "desc");
            productSkuCode = SafeDictionary.GetValue<string>(dic, "productSkuCode");
            productSkuName = SafeDictionary.GetValue<string>(dic, "productSkuName");
            region = SafeDictionary.GetValue<string>(dic, "region");
            salePrice = SafeDictionary.GetValue<string>(dic, "salePrice");
        }
    }
}