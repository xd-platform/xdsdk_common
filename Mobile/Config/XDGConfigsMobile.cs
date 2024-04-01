using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace XD.SDK.Common.Internal {
    public class XDGConfigsMobile : IXDConfigs {
        public bool IsCN {
            get {
#if UNITY_ANDROID
                using (AndroidJavaClass CommonBridgeClass = new AndroidJavaClass("com.xd.common.bridge.XDGCommonBridge")) {
                    return CommonBridgeClass.CallStatic<bool>("isCN");
                }
#elif UNITY_IOS
            return XDCommonBridgeIsCN();
#endif
                return true;
            }
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern bool XDCommonBridgeIsCN();
#endif
    }
}
