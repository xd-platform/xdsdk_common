using System;
using System.Collections.Generic;
using TapTap.Common;

namespace XD.SDK.Payment
{
    [Serializable]
    public class XDGRestoredPurchasesWrapper
    {
        public List<XDGRestoredPurchases> restoredPurchasesList;
        
        public XDGRestoredPurchasesWrapper(string json)
        {
            Dictionary<string,object> dic = Json.Deserialize(json) as Dictionary<string,object>;
            List<object> dicList = SafeDictionary.GetValue<List<object>>(dic,"transactions");

            if (dicList != null)
            {
                this.restoredPurchasesList = new List<XDGRestoredPurchases>();
                foreach (var detailDic in dicList)
                {
                    Dictionary<string,object> innerDic = detailDic as Dictionary<string,object>;
                   restoredPurchasesList.Add(new XDGRestoredPurchases(innerDic));   
                }
            }
        }
    }
    
    [Serializable]
    public class XDGRestoredPurchases
    {
        public string transactionIdentifier;
        public string productIdentifier;

        public XDGRestoredPurchases(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            this.transactionIdentifier = SafeDictionary.GetValue<string>(dic, "transactionIdentifier");
            this.productIdentifier = SafeDictionary.GetValue<string>(dic, "productIdentifier");
        }
    }
}