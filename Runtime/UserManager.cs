using System;
using System.Linq;
using UnityEngine;
using XD.SDK.Account.Internal;
using XD.SDK.Common;

namespace XD.SDK.Account
{
    public class UserManager
    {
        private static IUserManagerPlatformWrapper _platformWrapper;

        public static void Init()
        {
            var interfaceType = typeof(IUserManagerPlatformWrapper);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);

            if (platformInterfaceType != null)
            {
                _platformWrapper = Activator.CreateInstance(platformInterfaceType) as IUserManagerPlatformWrapper;
            }
            else
            {
                XDGLogger.Error($"No class implements {interfaceType} Type. Current Platform : {Application.platform}, if you are using Editor, , please check if you have installed XDSDK pc module.");
            }
        }

        public static XDGUser GetCurrentUser()
        {
            return _platformWrapper.GetCurrentUser();
        }
    }
}