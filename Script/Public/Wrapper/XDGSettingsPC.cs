using XD.SDK.Common.PC.Internal;

#if UNITY_EDITOR || UNITY_STANDALONE

namespace XD.SDK.Common
{
    public class XDGSettingsPC : IXDGSettings
    {
        public void SetTargetCountryOrRegion(string region)
        {
            ConfigModule.region = region;
        }
    }
}
#endif