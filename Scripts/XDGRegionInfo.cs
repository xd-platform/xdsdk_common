using System.Collections.Generic;
using TapTap.Common;
using XD.SDK.Common.Internal;

namespace XD.SDK.Common
{
    public class XDGRegionInfo : IXDGRegionInfo
    {
        public string _city;
        public string _countryCode;
        public string _timeZone;
        public string _locationInfoType;

        public XDGRegionInfo(Dictionary<string, object> dic)
        {
            if (dic == null) return;
            _city = SafeDictionary.GetValue<string>(dic, "city");
            _countryCode = SafeDictionary.GetValue<string>(dic, "countryCode");
            _timeZone = SafeDictionary.GetValue<string>(dic, "timeZone");
            _locationInfoType = SafeDictionary.GetValue<string>(dic, "locationInfoType");
        }

        public string city => _city;
        public string countryCode => _countryCode;
        public string timeZone => _timeZone;
        public string locationInfoType => _locationInfoType;
    }
}