#if UNITY_EDITOR && UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Regex = System.Text.RegularExpressions.Regex;
using LC.Newtonsoft.Json;

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

        public static void SaveProvider(string path, BaseAndroidGradleContextProvider provider, bool assetDatabaseRefresh = true)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;
            serializer.DefaultValueHandling = DefaultValueHandling.Include;
            using (var sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, provider);
            }
            if (assetDatabaseRefresh) AssetDatabase.Refresh();
        }
        
        [MenuItem("Android/Load")]
        public static List<BaseAndroidGradleContextProvider> Load()
        {
            var guids = AssetDatabase.FindAssets("TapAndroidProvider");
            if (guids == null) return null;
            var providers = new List<BaseAndroidGradleContextProvider>();
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                using (var file = File.OpenText(assetPath))
                {
                    try
                    {
                        var serializer = new JsonSerializer();
                        var gradleContextProvider = (BaseAndroidGradleContextProvider)serializer.Deserialize(file, typeof(BaseAndroidGradleContextProvider));
                        if (gradleContextProvider.AndroidGradleContext != null)
                        {
                            foreach (var gradleContext in gradleContextProvider.AndroidGradleContext)
                            {
                                if (gradleContext.processContent != null) gradleContext.processContent.Reverse();
                            }
                        }
                        gradleContextProvider.AndroidGradleContext.Reverse();
                        providers.Add(gradleContextProvider);
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat($"[Tap::AndroidGradleProcessor] Deserialize AndroidGradleContextProvider Error! Error Msg:\n{e.Message}\nError Stack:\n{e.StackTrace}");
                    }
                }
            }
            providers.Sort((a,b)=> a.Priority.CompareTo(b.Priority));
            return providers;
        }
        
        [MenuItem("Android/Test")]
        private static void Test()
        {
            var providers = Load();
            if (providers == null) return;
            
            foreach (var provider in providers)
            {
                if (provider.AndroidGradleContext == null) continue;
                foreach (var context in provider.AndroidGradleContext)
                {
                    try
                    {
                        ProcessCustomGradleContext(context);
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat($"[Tap::AndroidGradleProcessor] Process Custom Gradle Context Error! Error Msg:\n{e.Message}\nError Stack:\n{e.StackTrace}");
                    }
                }
            }
        }
                
        // [MenuItem("Android/Regex")]
        // private static void Regex()
        // {
        //     string packageNamePatter =
        //         "\\s*([-\\w]{0,62}\\.\\s*)+[-\\w]{0,62}\\s*:\\s*[-\\w]{0,62}\\s*";
        //     string versionNumberPatter =
        //         "\\s*(\\d{1,3}\\.\\s*){1,3}\\d{1,3}\\s*";
        //     string headerPatter = "^\\s*(\\/){0}\\s*\\w+\\s";
        //     string packageAllPattern = headerPatter + "['\"]" + packageNamePatter + ":" + versionNumberPatter + "['\"]";
        //         // "^\\s*(\\/){0}\\s*\\w+\\s['\"]\\s*([-\\w]{0,62}\\.\\s*)+[-\\w]{0,62}\\s*:\\s*[-\\w]{0,62}\\s*:\\s*(\\d{1,3}\\.\\s*){1,3}\\d{1,3}\\s*['\"]";
        //     
        //     var importMatch =
        //         System.Text.RegularExpressions.Regex.Match(
        //             $"    implementation 'com.google.firebase:firebase-core:18.0.0'", packageAllPattern);
        //     Debug.LogFormat($"importmatch result: {importMatch.Success}");
        //     var pkgNameMatch =
        //         System.Text.RegularExpressions.Regex.Match(
        //             $"//    implementation 'com.google.firebase:firebase-core:18.0.0'", packageNamePatter);
        //     Debug.LogFormat($"importmatch result: {pkgNameMatch.Success}");
        //     var verNumberMatch =
        //         System.Text.RegularExpressions.Regex.Match(
        //             $"//    implementation 'com.google.firebase:firebase-core:18.0.0'", versionNumberPatter);
        //     Debug.LogFormat($"importmatch result: {verNumberMatch.Success}");
        //
        //     var providers = Load();
        //     if (providers == null) return;
        //     providers.Sort((a,b)=> a.Priority.CompareTo(b.Priority));
        //     foreach (var provider in providers)
        //     {
        //         if (provider.AndroidGradleContext == null) continue;
        //         foreach (var context in provider.AndroidGradleContext)
        //         {
        //             var fileInfo = ToggleCustomTemplateFile(context.templateType, true);
        //             if (fileInfo == null) return;
        //             var contents = File.ReadAllText(fileInfo.FullName);
        //             foreach (var processContent in context.processContent)
        //             {
        //                 var debug = processContent.Contains("    implementation 'com.google.firebase:firebase-core:18.0.0'");
        //                 if (debug == false) continue;
        //                 // 已经替换过的情况
        //                 var importPkgNameMatch = "com.google.firebase:firebase-core";
        //                 var pattern = "^\\s*(\\/){0}\\s*\\w+\\s['\"]" + importPkgNameMatch + ":" + versionNumberPatter +
        //                               "['\"]";
        //                 var builtinMatches = System.Text.RegularExpressions.
        //                     Regex.Matches(contents, pattern, RegexOptions.Multiline);
        //                 Debug.LogFormat($"FileInfo builtinMatches Result Count: {builtinMatches.Count}");
        //                 var normalMatches = System.Text.RegularExpressions.
        //                     Regex.Matches(contents, pattern, RegexOptions.Multiline);
        //                 Debug.LogFormat($"FileInfo normalMatches Result Count: {normalMatches.Count}");
        //                 
        //             }
        //         }
        //     }
        //
        // }
        
        public static void ProcessCustomGradleContext(XDGAndroidGradleContext gradleContext)
        {
            if (gradleContext == null) return;
            var fileInfo = ToggleCustomTemplateFile(gradleContext.templateType, true);
            if (fileInfo == null) return;
            for (int i = 0; i < gradleContext.processContent.Count; i++)
            {
                var content = gradleContext.processContent[i];
                bool appendNewline = gradleContext.processType == AndroidGradleProcessType.Insert && i == 0;
                ProcessEachContext(gradleContext, content, fileInfo, appendNewline);
            }
        }
        
        private static void ProcessEachContext(XDGAndroidGradleContext gradleContext, string eachContext, FileInfo gradleTemplateFileInfo, bool apeendNewline = false)
        {
            if (UnityVersionValidate(gradleContext) == false) return;
            
            // 已经替换过的情况
            var contents = File.ReadAllText(gradleTemplateFileInfo.FullName);
            Match match = null;

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

            var needImport = CheckNeedImportPackage(gradleContext, eachContext, contents, gradleTemplateFileInfo, ref index, out string fixedContents);
            if (needImport == false) return;
            if (false == string.IsNullOrEmpty(fixedContents)) contents = fixedContents;
            // 替换新的修改内容
            string newContents = null;
            if (gradleContext.processType == AndroidGradleProcessType.Insert)
            {
                newContents = contents.Insert(index, string.Format("\n{0}{1}", eachContext, (apeendNewline?"\n":"")));
            }
            else if (gradleContext.processType == AndroidGradleProcessType.Replace)
            {
                string replaceContent = contents.Replace(contents.Substring(index, match.Length),
                    string.Format("{0}{1}", eachContext, apeendNewline ? "\n" : ""));
                newContents = replaceContent;
            }
                
            File.WriteAllText(gradleTemplateFileInfo.FullName, newContents);
        }
        
        private static bool UnityVersionValidate(XDGAndroidGradleContext gradleContext)
        {
            // 版本检查
            var unityVersionCompatibleType = gradleContext.unityVersionCompatibleType;
            if (unityVersionCompatibleType == UnityVersionCompatibleType.Unity_2019_3_Above)
            {
#if !UNITY_2019_3_OR_NEWER
                return false;
#endif
            }
            if (unityVersionCompatibleType == UnityVersionCompatibleType.Unity_2019_3_Beblow)
            {
#if UNITY_2019_3_OR_NEWER
                return false;
#endif
            }

            return true;
        }
        
        /// <summary>
        /// 检查是否有引入包重复情况
        /// </summary>
        /// <param name="gradleContext"></param>
        /// <param name="eachContext"></param>
        /// <param name="contents"></param>
        /// <returns>是否需要继续引入包</returns>
        private static bool CheckNeedImportPackage(XDGAndroidGradleContext gradleContext, string eachContext, string contents, FileInfo gradleTemplateFileInfo, ref int insertIndex, out string newContents)
        {
            newContents = null;
            // 是否已经写过
            bool hadWrote = false;
            
            if (gradleContext.locationType == AndroidGradleLocationType.End)
            {
                var text = contents.TrimEnd();
                text = text.Substring(text.Length - eachContext.Length, eachContext.Length);
                hadWrote = text == eachContext;
            }
            else
            {
                if (gradleContext.processType == AndroidGradleProcessType.Insert)
                {
                    var temp = System.Text.RegularExpressions.Regex.Match(contents, string.Format("^{0}", eachContext), RegexOptions.Multiline, TimeSpan.FromSeconds(2));
                    hadWrote = temp.Success;
                }
                else if (gradleContext.processType == AndroidGradleProcessType.Replace)
                {
                    hadWrote = contents.Substring(insertIndex, eachContext.Length) == eachContext;
                }
            }
            if (hadWrote) return false;
            
            string headerPattern = "^.*";
            string packageNamePattern =
                "\\s*([-\\w]{0,62}\\.\\s*)+[-\\w]{0,62}\\s*:\\s*[-\\w]{0,62}\\s*";
            string versionNumberPattern =
                "\\s*(\\d{1,3}\\.\\s*){1,3}\\d{1,3}\\s*";
            string packageAllPattern = headerPattern + "['\"]" + packageNamePattern + ":" + versionNumberPattern + "['\"]";

            var importMatch = System.Text.RegularExpressions.Regex.Match(eachContext, packageAllPattern);
            if (importMatch == null || string.IsNullOrEmpty(importMatch.Value)) return true;

            var importPkgNameMatch = System.Text.RegularExpressions.Regex.Match(importMatch.Value, packageNamePattern);
            if (importPkgNameMatch == null || string.IsNullOrEmpty(importPkgNameMatch.Value)) return true;

            // if (eachContext.Contains("com.google.firebase:firebase-crashlytics-gradle"))
            // {
            //     Debug.LogFormat("Detected");
            // }
            try
            {
                var pattern = headerPattern + "['\"]" + importPkgNameMatch.Value + ":" + versionNumberPattern + "['\"]";
                // 已经存在的 gradle 文件有没有对应的 package 内容
                var builtinMatches = System.Text.RegularExpressions.
                    Regex.Matches(contents, pattern, RegexOptions.Multiline, TimeSpan.FromSeconds(2));
                if (builtinMatches == null || builtinMatches.Count == 0) return true;

                var importVersionMatch = System.Text.RegularExpressions.
                    Regex.Match(importMatch.Value, versionNumberPattern);
                var importVersion = new Version(importVersionMatch.Value);

                foreach (Match builtinMatch in builtinMatches)
                {
                    // 检查前面是否已经有了注释, -2 是防止末尾的\n
                    var matchEndIdx = builtinMatch.Index + builtinMatch.Length - 2;
                    var headerIdx = contents.LastIndexOf('\n', matchEndIdx);
                    var commentIdx = contents.LastIndexOf("//", matchEndIdx, matchEndIdx - headerIdx);
                    if (commentIdx >= 0) continue;
                    
                    // 版本相同不添加
                    var builtinVersionMatch = System.Text.RegularExpressions.
                        Regex.Match(builtinMatch.Value, versionNumberPattern);
                    if (builtinVersionMatch.Success == false) continue;
                    var builtinVersion = new Version(builtinVersionMatch.Value);
                    if (importVersion == builtinVersion) return false;
                    // 如果是替换的话,不需要添加注释
                    if (gradleContext.processType == AndroidGradleProcessType.Replace) return true;
                    Debug.LogWarningFormat("[TapTap:AndroidGradlePostProcessor] Detect Package Collision! Gradle File: {0} Process Content: {1} Collision Content: {2}", gradleContext.templateType, eachContext, builtinMatch.Value);
                    // 引入版本更低,那就不用引入了
                    if (importVersion < builtinVersion) return false;
                    // 引入版本更高,把已经存在的版本注释掉
                    newContents = contents.Insert(headerIdx + 1, "//");
                    File.WriteAllText(gradleTemplateFileInfo.FullName, newContents);
                    if (gradleContext.locationType == AndroidGradleLocationType.End) insertIndex = newContents.Length;
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return true;
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
#if UNITY_2019_3_OR_NEWER
             m_AndroidEditorModulePath =
                    BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None);
#else
            var buildPipelineType = typeof(BuildPipeline);
            var methodInfo = buildPipelineType.GetMethod("GetBuildToolsDirectory", BindingFlags.Static | BindingFlags.NonPublic);
            var temp = methodInfo.Invoke(null, new object[] { BuildTarget.Android }) as string;
            m_AndroidEditorModulePath = temp.Substring(0, temp.LastIndexOf('/'));
#endif
            m_PluginPath = Path.Combine("Assets", "Plugins", "Android");
            
#if UNITY_2019_3_OR_NEWER
            string builtinAPK = Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None), "Apk");
#else
            string builtinAPK = Path.Combine(m_AndroidEditorModulePath, "Apk");
#endif
            m_InternalMainManifest = Path.Combine(builtinAPK, "UnityManifest.xml");
            m_CustomMainManifest = Path.Combine(m_PluginPath, "AndroidManifest.xml");
            
#if UNITY_2019_3_OR_NEWER
            m_InternalLauncherManifest = Path.Combine(builtinAPK, "LauncherManifest.xml");
            m_CustomLauncherManifest = Path.Combine(m_PluginPath, "LauncherManifest.xml");
#else
            m_InternalLauncherManifest = Path.Combine(builtinAPK, "UnityManifest.xml");
            m_CustomLauncherManifest = Path.Combine(m_PluginPath, "AndroidManifest.xml");
#endif
            
            string builtinGradleTemplates = Path.Combine(m_AndroidEditorModulePath, "Tools", "GradleTemplates");
            m_InternalMainGradleTemplate = Path.Combine(builtinGradleTemplates, "mainTemplate.gradle");
            m_CustomMainGradleTemplate = Path.Combine(m_PluginPath, "mainTemplate.gradle");
            
#if UNITY_2019_3_OR_NEWER
            m_InternalLauncherGradleTemplate = Path.Combine(builtinGradleTemplates, "launcherTemplate.gradle");
            m_CustomLauncherGradleTemplate = Path.Combine(m_PluginPath, "launcherTemplate.gradle");
#else
            m_InternalLauncherGradleTemplate = Path.Combine(builtinGradleTemplates, "mainTemplate.gradle");
            m_CustomLauncherGradleTemplate = Path.Combine(m_PluginPath, "mainTemplate.gradle");
#endif
            
#if UNITY_2019_3_OR_NEWER
            m_InternalBaseGradleTemplate = Path.Combine(builtinGradleTemplates, "baseProjectTemplate.gradle");
            m_CustomBaseGradleTemplate = Path.Combine(m_PluginPath, "baseProjectTemplate.gradle");
#else
            m_InternalBaseGradleTemplate = Path.Combine(builtinGradleTemplates, "mainTemplate.gradle");
            m_CustomBaseGradleTemplate = Path.Combine(m_PluginPath, "mainTemplate.gradle");
#endif
            
#if UNITY_2019_3_OR_NEWER
            m_InternalGradlePropertiesTemplate = Path.Combine(builtinGradleTemplates, "gradleTemplate.properties");
            m_CustomGradlePropertiesTemplate = Path.Combine(m_PluginPath, "gradleTemplate.properties");
#endif
            
        }
    }
}
#endif