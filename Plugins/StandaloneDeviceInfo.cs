using System;
using System.Runtime.InteropServices;
using System.Globalization;

namespace XD.SDK.Common.PC.Internal {
    public class StandaloneDeviceInfo {

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        [DllImport("MacDeviceInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetDeviceLocation();

        [DllImport("MacDeviceInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetDeviceLanguage();
#endif

        public static string GetLocation() {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            return Marshal.PtrToStringAnsi(GetDeviceLocation());
#else
        return RegionInfo.CurrentRegion.TwoLetterISORegionName;
#endif
        }

        public static string GetLanguage() {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            return Marshal.PtrToStringAnsi(GetDeviceLanguage());
#else
        return CultureInfo.CurrentUICulture.IetfLanguageTag;
#endif
        }
    }
}
