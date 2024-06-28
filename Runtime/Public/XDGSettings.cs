using System;
using System.Linq;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common.Internal;

namespace XD.SDK.Common
{
    public class XDGSettings
    {
        private static readonly IXDGSettings platformWrapper;

        static XDGSettings() 
        {
            var interfaceType = typeof(IXDGSettings);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz)&& clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDGSettings;
            }
            else 
            {
                TapLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }

        public static void SetTargetCountryOrRegion(string region)
        {
            platformWrapper?.SetTargetCountryOrRegion(region);
        }
    }
}