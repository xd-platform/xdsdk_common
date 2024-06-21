#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using XD.SDK.Common;

namespace XD.SDK.Account.Internal
{
    public class UserManagerMobile : IUserManagerPlatformWrapper
    {
        public XDGUser GetCurrentUser()
        {
            XDGUser resultUser = null;
            string currentUserString = null;
#if UNITY_ANDROID
            using (var accountBridgeClass = new AndroidJavaClass("com.xd.account.bridge.XDGAccountBridge"))
            {
                currentUserString = accountBridgeClass.CallStatic<string>("getCurrentUser");
            }
#elif UNITY_IOS
            IntPtr ptr = XDAccountBridgeGetUser();
            currentUserString = Marshal.PtrToStringAnsi(ptr);
#endif
            if (!string.IsNullOrEmpty(currentUserString))
            {
                try
                {
                    resultUser = new XDGUserMobile(currentUserString);
                }
                catch (Exception e)
                {
                    XDGLogger.Warn("convert current user error" + e.Message);
                }
            }
            return resultUser;
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern IntPtr XDAccountBridgeGetUser();
#endif
    }
}
#endif