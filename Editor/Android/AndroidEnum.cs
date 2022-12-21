#if UNITY_EDITOR && UNITY_ANDROID
namespace XD.SDK.Common.Editor
{
    public enum CustomTemplateType
    {
        None = 0,
        AndroidManifest = 1,
        LauncherManifest = 2,
        UnityMainGradle = 3,
        LauncherGradle = 4,
        BaseGradle = 5,
        GradleProperties = 6,
    }

    public enum AndroidGradleLocationType
    {
        Builtin = 1,
        Custom = 2,
        End = 3,
    }

    public enum AndroidGradleProcessType
    {
        Insert = 1,
        Replace = 2,
    }
}
#endif