using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common;

namespace XD.SDK.Payment
{
    public interface XDGSkuDetailInfo
    {
        XDGError xdgError { get; }

        List<SkuDetailBean> skuDetailList { get; }
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
    
    public interface XDGOrderInfoWrapper
    {
        XDGError xdgError { get; }
        XDGOrderInfo orderInfo { get; }
        string debugMsg { get; }
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
}