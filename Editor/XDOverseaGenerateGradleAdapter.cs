#if UNITY_EDITOR && UNITY_ANDROID
using System;
using System.IO;
using LC.Newtonsoft.Json.Linq;
using TapTap.AndroidDependencyResolver.Editor;
using UnityEditor.Android;
using UnityEngine;
using XD.SDK.Common.Editor;

namespace XD.SDK.Mainland.Editor
{
    public class XDOverseaGenerateGradleAdapter : IPostGenerateGradleAndroidProject
    {
        private const string UnityLibraryName = "unityLibrary";
        private const string MatchText = "mavenCentral()";
        private const string Repo = "jcenter()";

        public int callbackOrder => AndroidGradleProcessor.CALLBACK_ORDER - 51;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            var xdConfigJsonPath = XDGCommonEditorUtils.GetXDConfigPath();
            if (!File.Exists(xdConfigJsonPath))
            {
                return;
            }

            var configObj = JObject.Parse(File.ReadAllText(xdConfigJsonPath));

            var rootPath = path;
            if (rootPath.Contains(UnityLibraryName))
            {
                rootPath = rootPath.Substring(0, rootPath.Length - UnityLibraryName.Length);
            }

            if (configObj["douyin"] != null)
            {
                FixRepositories(rootPath);
            }
        }

        private static void FixRepositories(string rootPath)
        {
#if UNITY_2022_2_OR_NEWER
            var settingsGradleFilePath = Path.Combine(rootPath, "settings.gradle");
            if (!File.Exists(settingsGradleFilePath)) return;

            var allContent = File.ReadAllText(settingsGradleFilePath);

            var startIndex = allContent.LastIndexOf(MatchText, StringComparison.Ordinal);
            if (startIndex == -1)
            {
                Debug.LogWarning(settingsGradleFilePath + "中没有找到字符串：" + MatchText);
                return;
            }

            var endIndex = startIndex + MatchText.Length;

            allContent = allContent.Insert(endIndex, "\n" + Repo);
            File.WriteAllText(settingsGradleFilePath, allContent);
#endif
        }
    }
}
#endif