#if UNITY_EDITOR && UNITY_ANDROID
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XD.SDK.Common.Editor
{
    public static class AndroidUtils
    {
        private static string m_PluginPath;
        private static string m_AndroidEditorModulePath;
        
        private static string m_CustomMainManifest;
        private static string m_InternalMainManifest;
        private static string m_InternalLauncherManifest;
        private static string m_CustomLauncherManifest;
        
        private static string m_InternalMainGradleTemplate;
        private static string m_CustomMainGradleTemplate;
        
        private static string m_InternalLauncherGradleTemplate;
        private static string m_CustomLauncherGradleTemplate;
        private static string m_InternalBaseGradleTemplate;
        private static string m_CustomBaseGradleTemplate;
        private static string m_InternalGradlePropertiesTemplate;
        private static string m_CustomGradlePropertiesTemplate;
        
        public static void ProcessCustomGradleContext(XDGAndroidGradleContext gradleContext)
        {
            if (gradleContext == null) return;
            var fileInfo = ToggleCustomTemplateFile(gradleContext.templateType, true);
            if (fileInfo == null) return;
            
            // 已经替换过的情况
            var contents = File.ReadAllText(fileInfo.FullName);
            Match match = null;
            match = new Regex($"{gradleContext.processContent}").Match(contents);
            if (match.Success) return;
            
            // 寻找修改位置
            switch (gradleContext.locationType)
            {
                case AndroidGradleLocationType.Builtin:
                    match = new Regex($"\\*\\*{gradleContext.locationParam}\\*\\*").Match(contents);
                    break;
                case AndroidGradleLocationType.Custom:
                    match = new Regex($"{gradleContext.locationParam}").Match(contents);
                    break;
                case AndroidGradleLocationType.End:
                    break;
            }

            int index = 0;
            if (gradleContext.locationType != AndroidGradleLocationType.End)
            {
                if (match.Success == false)
                {
                    Debug.LogWarningFormat($"Couldn't find Custom Gradle Template Location! Gradle Type: {gradleContext.templateType} Location Type: {gradleContext.locationType} Location Param: {gradleContext.locationParam}");
                    return;
                }
                
                if (gradleContext.processType == AndroidGradleProcessType.Insert)
                    index = match.Index + match.Length;
                else if (gradleContext.processType == AndroidGradleProcessType.Replace)
                    index = match.Index;
            }
            else
            {
                index = contents.Length;
            }
            
            // 替换新的修改内容
            string newContents = null;
            if (gradleContext.processType == AndroidGradleProcessType.Insert)
            {
                string insertContent = string.Format("\n{0}\n", gradleContext.processContent); 
                newContents = contents.Insert(index, insertContent);
            }
            else if (gradleContext.processType == AndroidGradleProcessType.Replace)
            {
                string replaceContent = contents.Replace(contents.Substring(index, match.Length),
                    gradleContext.processContent);
                newContents = replaceContent;
            }
            
            File.WriteAllText(fileInfo.FullName, newContents);
        }

        public static FileInfo ToggleCustomTemplateFile(CustomTemplateType templateType, bool create)
        {
            if (string.IsNullOrEmpty(m_AndroidEditorModulePath))
            {
                Init();
            }

            GetTemplatePath(templateType, out string internalPath, out string customPath);

            return ToggleAsset(internalPath, customPath, create);
        }
        
        public static bool HaveCustomTemplateFile(CustomTemplateType templateType)
        {
            if (string.IsNullOrEmpty(m_AndroidEditorModulePath))
            {
                Init();
            }

            GetTemplatePath(templateType, out _, out string customPath);

            return File.Exists(customPath);
        }
        
        private static FileInfo ToggleAsset(string originalFile, string assetFile, bool create)
        {
            if (string.IsNullOrEmpty(originalFile) || string.IsNullOrEmpty(assetFile)) return null;
            string str = assetFile + ".DISABLED";
            string directoryName = Path.GetDirectoryName(assetFile);
            if (create)
            {
                if (File.Exists(str))
                {
                    AssetDatabase.MoveAsset(str, assetFile);
                    return new FileInfo(assetFile);
                }

                if (File.Exists(assetFile) == false)
                {
                    if (!Directory.Exists(directoryName))
                        Directory.CreateDirectory(directoryName);
                    if (File.Exists(originalFile))
                        File.Copy(originalFile, assetFile);
                    else
                        File.Create(assetFile).Dispose();
                    AssetDatabase.Refresh();
                }
                return new FileInfo(assetFile);
            }
            else
            {
                AssetDatabase.MoveAsset(assetFile, str);
                return new FileInfo(str);
            }
        }

        private static void GetTemplatePath(CustomTemplateType templateType, out string internalPath,
            out string customPath)
        {
            internalPath = null;
            customPath = null;
            switch (templateType)
            {
                case CustomTemplateType.AndroidManifest:
                    internalPath = m_InternalMainManifest;
                    customPath = m_CustomMainManifest;
                    break;
                case CustomTemplateType.BaseGradle:
                    internalPath = m_InternalBaseGradleTemplate;
                    customPath = m_CustomBaseGradleTemplate;
                    break;
                case CustomTemplateType.GradleProperties:
                    internalPath = m_InternalGradlePropertiesTemplate;
                    customPath = m_CustomGradlePropertiesTemplate;
                    break;
                case CustomTemplateType.LauncherGradle:
                    internalPath = m_InternalLauncherGradleTemplate;
                    customPath = m_CustomLauncherGradleTemplate;
                    break;
                case CustomTemplateType.LauncherManifest:
                    internalPath = m_InternalLauncherManifest;
                    customPath = m_CustomLauncherManifest;
                    break;
                case CustomTemplateType.UnityMainGradle:
                    internalPath = m_InternalMainGradleTemplate;
                    customPath = m_CustomMainGradleTemplate;
                    break;
            }
        }
        private static void Init()
        {
#if UNITY_2019_1_OR_NEWER
             m_AndroidEditorModulePath =
                    BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None);
#else
            var buildPipelineType = typeof(BuildPipeline);
            var methodInfo = buildPipelineType.GetMethod("GetBuildToolsDirectory", BindingFlags.Static | BindingFlags.NonPublic);
            var temp = methodInfo.Invoke(null, new object[] { BuildTarget.Android }) as string;
            m_AndroidEditorModulePath = temp.Substring(0, temp.LastIndexOf('/'));
#endif
            m_PluginPath = Path.Combine("Assets", "Plugins", "Android");
            
#if UNITY_2019_1_OR_NEWER
            string builtinAPK = Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None), "Apk");
#else
            string builtinAPK = Path.Combine(m_AndroidEditorModulePath, "Apk");
#endif
            m_InternalMainManifest = Path.Combine(builtinAPK, "UnityManifest.xml");
            m_CustomMainManifest = Path.Combine(m_PluginPath, "AndroidManifest.xml");
            
#if UNITY_2019_1_OR_NEWER
            m_InternalLauncherManifest = Path.Combine(builtinAPK, "LauncherManifest.xml");
            m_CustomLauncherManifest = Path.Combine(m_PluginPath, "LauncherManifest.xml");
#else
            m_InternalLauncherManifest = Path.Combine(builtinAPK, "UnityManifest.xml");
            m_CustomLauncherManifest = Path.Combine(m_PluginPath, "AndroidManifest.xml");
#endif
            
            string builtinGradleTemplates = Path.Combine(m_AndroidEditorModulePath, "Tools", "GradleTemplates");
            m_InternalMainGradleTemplate = Path.Combine(builtinGradleTemplates, "mainTemplate.gradle");
            m_CustomMainGradleTemplate = Path.Combine(m_PluginPath, "mainTemplate.gradle");
            
#if UNITY_2019_1_OR_NEWER
            m_InternalLauncherGradleTemplate = Path.Combine(builtinGradleTemplates, "launcherTemplate.gradle");
            m_CustomLauncherGradleTemplate = Path.Combine(m_PluginPath, "launcherTemplate.gradle");
#else
            m_InternalLauncherGradleTemplate = Path.Combine(builtinGradleTemplates, "mainTemplate.gradle");
            m_CustomLauncherGradleTemplate = Path.Combine(m_PluginPath, "mainTemplate.gradle");
#endif
            
#if UNITY_2019_1_OR_NEWER
            m_InternalBaseGradleTemplate = Path.Combine(builtinGradleTemplates, "baseProjectTemplate.gradle");
            m_CustomBaseGradleTemplate = Path.Combine(m_PluginPath, "baseProjectTemplate.gradle");
#else
            m_InternalBaseGradleTemplate = Path.Combine(builtinGradleTemplates, "mainTemplate.gradle");
            m_CustomBaseGradleTemplate = Path.Combine(m_PluginPath, "mainTemplate.gradle");
#endif
            
#if UNITY_2019_1_OR_NEWER
            m_InternalGradlePropertiesTemplate = Path.Combine(builtinGradleTemplates, "gradleTemplate.properties");
            m_CustomGradlePropertiesTemplate = Path.Combine(m_PluginPath, "gradleTemplate.properties");
#endif
            
        }
    }
}
#endif