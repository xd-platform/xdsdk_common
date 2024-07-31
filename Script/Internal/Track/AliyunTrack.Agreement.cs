#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XD.SDK.Common.PC.Internal {
    /// <summary>
    /// 隐私协议部分
    /// </summary>
    internal partial class AliyunTrack
    {
        internal enum AgreementCountryType
        {
            Default = 1,
            Korea = 2,
            US = 3,
        }

        private static AgreementCountryType countryType;
        private static string AgreementRegion;
        private static string AgreementVersion;

        private static string AgreementEventSessionId;

        internal static string AgreementMapExtra2TrackInfo(Dictionary<string, object> extraData)
        {
            var extraArray = new List<string>();
            if (AliyunTrack.countryType != AgreementCountryType.Korea) return null;
            if (extraData == null) return null;
            foreach (var key in extraData.Keys)
            {
                if (extraData[key] is bool value)
                {
                    extraArray.Add(key + (value ? "_enable" : "_disable"));
                }
            }
            
            return extraArray.Count > 0 ? string.Join(",", extraArray) : null;
        }
        
        internal static void AgreementAsk(AgreementCountryType countryType, string region, string version)
        {
            AliyunTrack.countryType = countryType;
            AgreementRegion = region;
            AgreementVersion = version;
            AgreementEventSessionId = GetNewEventSessionId();
            LogEventAsync("sdkprotocol_privacy_ask", GetAgreementModuleCommonProperties("sdkprotocol_privacy_ask"));
        }
        
        internal static void AgreementSign(string extras = "")
        {
            var contents = GetAgreementModuleCommonProperties("sdkprotocol_privacy_agree");
            contents["extras"] = extras;
            LogEventAsync("sdkprotocol_privacy_agree", contents);

        }
        
        private static Dictionary<string, string> GetAgreementModuleCommonProperties(string eventName)
        {
            var content = new Dictionary<string, string>();
            content["tag"] = "sdkprotocol";
            content["event_session_id"] = AgreementEventSessionId;
            content["popup_type"] = ((int)countryType).ToString();
            content["a_region"] = AgreementRegion;
            content["a_version"] = AgreementVersion;
            content["logid"] = GetLogId(AgreementEventSessionId, eventName);
            return content;
        }
    }
}
#endif