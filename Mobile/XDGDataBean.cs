using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common;
using XD.SDK.Payment;

namespace XD.SDK.Payment
{
    public class XDGSkuDetailInfoMobile : XDGSkuDetailInfo
    {
        private XDGError _xdgError;

        private List<SkuDetailBean> _skuDetailList;

        public XDGError xdgError => _xdgError;

        public List<SkuDetailBean> skuDetailList => _skuDetailList;

        public XDGSkuDetailInfoMobile(string jsonStr)
        {
            var dic = Json.Deserialize(jsonStr) as Dictionary<string, object>;
            var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "error");
            if (errorDic != null)
            {
                _xdgError = new XDGErrorMobile(errorDic);
            }

            var list = SafeDictionary.GetValue<List<object>>(dic, "products");
            if (list == null) return;
            _skuDetailList = new List<SkuDetailBean>();
            foreach (var skuDetail in list)
            {
                var innerDic = skuDetail as Dictionary<string, object>;
                var skuDetailBean = new SkuDetailBean(innerDic);
                _skuDetailList.Add(skuDetailBean);
            }
        }
    }
    
    public class XDGOrderInfoWrapperMobile : XDGOrderInfoWrapper
    {
        private XDGError _xdgError;
        private XDGOrderInfo _orderInfo;
        private string _debugMsg;

        public XDGError xdgError => _xdgError;
        public XDGOrderInfo orderInfo => _orderInfo;
        public string debugMsg => _debugMsg;

        public XDGOrderInfoWrapperMobile(string jsonStr)
        {
            Debug.LogFormat("支付返回 json: {0}", jsonStr);
            var dic = Json.Deserialize(jsonStr) as Dictionary<string, object>;
            var errorDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "error");
            if (errorDic != null)
            {
                _xdgError = new XDGErrorMobile(errorDic);
                XDGTool.LogError($"产品支付失败： {jsonStr}");
            }

            var orderInfoDic = SafeDictionary.GetValue<Dictionary<string, object>>(dic, "orderInfo");
            _orderInfo = new XDGOrderInfo(orderInfoDic);

            _debugMsg = SafeDictionary.GetValue<string>(dic, "debugMessage");
        }
    }
    

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
        public List<XDGRefundDetails> refundList => _refundList?.ConvertAll<XDGRefundDetails>(a => a as XDGRefundDetails);
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