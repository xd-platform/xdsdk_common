#if UNITY_EDITOR && UNITY_ANDROID
namespace XD.SDK.Common.Editor
{
    public class XDGAndroidGradleContext
    {
        /// <summary>
        /// 替换位置
        /// </summary>
        public AndroidGradleLocationType locationType;
        
        /// <summary>
        /// Gradle 模板类型
        /// </summary>
        public CustomTemplateType templateType;
        
        /// <summary>
        /// 替换类型
        /// </summary>
        public AndroidGradleProcessType processType = AndroidGradleProcessType.Insert;
        
        /// <summary>
        /// 替换位置参数,
        /// </summary>
        public string locationParam;
        
        /// <summary>
        /// 替换内容
        /// </summary>
        public string processContent;
    }
}
#endif