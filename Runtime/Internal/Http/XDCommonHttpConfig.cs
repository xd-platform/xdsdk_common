using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XD.SDK.Common.Internal {
    public class XDCommonHttpConfig {
        private static IXDCommonHttpConfig platformWrapper;

        public static void Init() {
            var interfaceType = typeof(IXDCommonHttpConfig);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDCommonHttpConfig;

                XDHttpClientFactory.RegisterQueryParamsFunc(GetCommonQueryParams);
            } else {
                XDGLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }

        public static Dictionary<string, string> GetCommonQueryParams(string url, long timestamp) {
            return platformWrapper.GetCommonQueryParams(url, timestamp);
        }
    }
}
