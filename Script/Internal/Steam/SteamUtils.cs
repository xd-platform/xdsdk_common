using System;
using System.Linq;
using System.Threading.Tasks;
using TapTap.Common;
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    /// <summary>
    /// Steam SDK 封装接口
    /// </summary>
    public interface ISteamSDKWrapper {
        Task<string> GetAuthTicket();
        string GetSteamId();
        string GetSteamLanguage();
        Task<string> GetMicroTxn(ulong orderId);
    }

    public class SteamUtils {
        internal static ISteamSDKWrapper Instance { get; private set; }

        static SteamUtils() {
            Type interfaceType = typeof(ISteamSDKWrapper);
            Type steamWrapperType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);
            if (steamWrapperType != null) {
                Instance = Activator.CreateInstance(steamWrapperType) as ISteamSDKWrapper;
            } else {
                TapLogger.Error($"No class implements {interfaceType} Type. Current Platform: {Application.platform}, if you are using Editor, please check if you have installed XDSDK pc module.");
            }
        }

        /// <summary>
        /// 是否有 SDK 支持，即 Steam 包
        /// </summary>
        internal static bool IsSDKSupported => Instance != null;
    }
}