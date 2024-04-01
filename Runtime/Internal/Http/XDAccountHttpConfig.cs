using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using XD.SDK.Common;
using XD.SDK.Common.Internal;
using UnityEngine;

namespace XD.SDK.Account.Internal {
    public class XDAccountHttpConfig {
        private static IXDAccountHttpConfig platformWrapper;

        public static void Init() {
            var interfaceType = typeof(IXDAccountHttpConfig);
            var platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);
            if (platformInterfaceType != null) {
                platformWrapper = Activator.CreateInstance(platformInterfaceType) as IXDAccountHttpConfig;

                XDHttpClientFactory.RegisterHeadersTaskFunc(GetAuthorizationHeaders);
            } else {
                XDGLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }

        public static Task<Dictionary<string, string>> GetAuthorizationHeaders(HttpRequestMessage request, long timestamp) {
            return platformWrapper.GetAuthorizationHeaders(request, timestamp);
        }
    }
}
