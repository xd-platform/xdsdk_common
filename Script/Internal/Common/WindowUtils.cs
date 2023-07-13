#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    internal class WindowUtils {
        internal static void BringToFront() {
#if UNITY_EDITOR
            Debug.LogWarning("Unsupport bring to front on Unity Editor.");
#elif UNITY_STANDALONE_OSX
            Application.OpenURL($"open-taptap-{ConfigModule.TapConfig.ClientId}://");
#elif UNITY_STANDALONE_WIN
            User32.ForceWindowIntoForeground();
#endif
        }
    }
}
#endif