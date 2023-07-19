using System.Collections.Generic;
using TapTap.Common;

namespace  XD.SDK.Common
{
    public class XDGRegionInfoMobile : XDGRegionInfo
    {
        private string _city;
        private string _countryCode;
        private string _timeZone;
        private string _locationInfoType;

        public XDGRegionInfoMobile(Dictionary<string, object> dic)
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