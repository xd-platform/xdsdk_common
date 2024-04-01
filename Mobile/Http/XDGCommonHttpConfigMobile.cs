using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace XD.SDK.Common.Internal {
    public class XDGCommonHttpConfigMobile : IXDCommonHttpConfig {
        public Dictionary<string, string> GetCommonQueryParams(string url, long timestamp) {
            string fullUrl;
#if UNITY_ANDROID
            using (AndroidJavaClass CommonBridgeClass = new AndroidJavaClass("com.xd.common.bridge.XDGCommonBridge")) {
                fullUrl = CommonBridgeClass.CallStatic<string>("getCommonQueryString", url, timestamp);
            }
#elif UNITY_IOS
            IntPtr ptr = XDCommonBridgeGetCommonQueryString(url, timestamp);
            fullUrl = Marshal.PtrToStringAnsi(ptr);
#endif
            Uri uri = new Uri(fullUrl);
            NameValueCollection queries = XDUrlUtils.ParseQueryString(uri.Query);
            return queries.AllKeys.ToDictionary(key => Uri.UnescapeDataString(key), key => Uri.UnescapeDataString(queries[key]));
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern IntPtr XDCommonBridgeGetCommonQueryString(string url, long timestamp);
#endif
    }
}
