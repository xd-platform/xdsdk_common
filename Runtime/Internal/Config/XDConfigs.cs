using System;
using System.Linq;
using UnityEngine;

namespace XD.SDK.Common.Internal {
    public static class XDConfigs {
        private static IXDConfigs platformWrapper;

        public static void Init() {
            var interfaceType = typeof(IXDConfigs);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDConfigs;
            } else {
                XDGLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }

        public static bool IsCN => platformWrapper.IsCN;
    }
}
