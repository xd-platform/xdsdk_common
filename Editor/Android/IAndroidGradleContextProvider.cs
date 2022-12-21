#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;

namespace XD.SDK.Common.Editor
{
    public interface IAndroidGradleContextProvider
    {
        List<XDGAndroidGradleContext> GetAndroidGradleContext();
        
        int Priority { get; }
    }
}
#endif