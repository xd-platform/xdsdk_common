#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

namespace XD.SDK.Common
{
    public class XDGSettingsMobile : IXDGSettings
    {
        public void SetTargetCountryOrRegion(string region)
        {
            XDGCommonMobileImpl.GetInstance().setTargetCountryOrRegion(region);
        }
    }
}
#endif