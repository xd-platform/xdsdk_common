using System.Net;

namespace XD.SDK.Common
{
    public static class XDUtility
    {
        public static bool UseProxy { get; private set; }
        public static IWebProxy Proxy { get; private set; }

        public static void SetCustomProxy(IWebProxy proxy)
        {
            if (proxy == null)
            {
                UseProxy = false;
            }
            else
            {
                UseProxy = true;
                Proxy = proxy;
            }
        } 
    }
}