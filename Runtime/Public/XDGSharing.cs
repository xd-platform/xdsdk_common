using System;
using System.Linq;
using XD.SDK.Share.Internal;

namespace XD.SDK.Share {
    public static class XDGSharing {
        private static readonly IXDSharePlatform sharePlatform = null;

        static XDGSharing() {
            var interfaceType = typeof(IXDSharePlatform);
            Type platformInterfaceType = AppDomain.CurrentDomain.GetAssemblies()
                .Where(asssembly => asssembly.GetName().FullName.StartsWith("XD.SDK.Share"))
                .SelectMany(assembly => assembly.GetTypes())
                .SingleOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);
            if (platformInterfaceType != null) {
                sharePlatform = Activator.CreateInstance(platformInterfaceType) as IXDSharePlatform;
            }
        }

        public static void Share(XDGBaseShareParam shareParam, XDGShareCallback callback) {
            sharePlatform?.Share(shareParam, callback);
        }

        public static bool IsAppInstalled(XDGSharePlatformType platformType) {
            return sharePlatform?.IsAppInstalled(platformType) ?? false;
        }
    }
}
