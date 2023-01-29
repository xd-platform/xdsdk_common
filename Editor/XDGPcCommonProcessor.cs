#if UNITY_EDITOR && (!UNITY_IOS && !UNITY_ANDROID)
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Application = UnityEngine.Application;

namespace XD.SDK.Common.Editor
{
    public class XDGPcCommonProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        
        public int callbackOrder
        {
            get => 199;
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            var platform = report.summary.platform;
            if (platform == BuildTarget.StandaloneWindows || platform == BuildTarget.StandaloneWindows64 ||
                platform == BuildTarget.StandaloneLinux64 || platform == BuildTarget.StandaloneOSX)
            {
                DeleteStreamingAssetsFile("XDConfig.json");
                AssetDatabase.Refresh();
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            var platform = report.summary.platform;
            if (platform == BuildTarget.StandaloneWindows || platform == BuildTarget.StandaloneWindows64 ||
                platform == BuildTarget.StandaloneLinux64 || platform == BuildTarget.StandaloneOSX)
            {
                PrepareStreamingAssets();
                Copy2StreamingAssets(Application.dataPath, "XDConfig.json");
                AssetDatabase.Refresh();
            }
        }

        private void PrepareStreamingAssets()
        {
            var folderPath = Application.streamingAssetsPath;
            if (Directory.Exists(folderPath)) return;
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }
        
        private void Copy2StreamingAssets(string folderPath, string fileName)
        {
            var streamingAssetsFolder = Application.streamingAssetsPath;
            if (false == Directory.Exists(streamingAssetsFolder))
            {
                return;
            }
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                AssetDatabase.Refresh();
            }
            File.Copy(Path.Combine(folderPath, fileName), path);
        }
        
        private void DeleteStreamingAssetsFile(string fileName)
        {
            if (!Directory.Exists(Application.streamingAssetsPath)) return;
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
#endif