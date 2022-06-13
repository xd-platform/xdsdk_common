using System.Collections.Generic;
using TapTap.Common;

namespace XD.SDK.Common
{
    public class XDGRegionInfo
    {
        public string city;
        public string countryCode;
        public string timeZone;
        public string locationInfoType;

        public XDGRegionInfo(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            city = SafeDictionary.GetValue<string>(dic, "city");
            countryCode = SafeDictionary.GetValue<string>(dic, "countryCode");
            timeZone = SafeDictionary.GetValue<string>(dic, "timeZone");
            locationInfoType = SafeDictionary.GetValue<string>(dic, "locationInfoType");
        }
    }
}