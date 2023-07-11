#if XD_STEAM_SUPPORT

using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace XD.Editor {
    public class SteamBuildProcessor : IPostprocessBuildWithReport {
        public int callbackOrder => 999;

        public void OnPostprocessBuild(BuildReport report) {
            BuildTarget buildPlatform = report.summary.platform;
            if (buildPlatform != BuildTarget.StandaloneOSX &&
                buildPlatform != BuildTarget.StandaloneWindows && buildPlatform != BuildTarget.StandaloneWindows64) {
                return;
            }

            string appIdTxt = "steam_appid.txt";
            string appIdTxtPath = Path.Combine(Application.dataPath, "..", appIdTxt);
            if (!File.Exists(appIdTxtPath)) {
                Debug.LogError($"请将 steam_appid.txt 放置在工程根目录下。");
            }

            string outPath = report.summary.outputPath;

            if (buildPlatform == BuildTarget.StandaloneOSX) {
                string destFilePath = Path.Combine(outPath, "Contents/MacOS", appIdTxt);
                File.Copy(appIdTxtPath, destFilePath);
            } else if (buildPlatform == BuildTarget.StandaloneWindows || buildPlatform == BuildTarget.StandaloneWindows64) {
                string destFilePath = Path.Combine(Directory.GetParent(outPath).FullName, appIdTxt);
                File.Copy(appIdTxtPath, destFilePath);
            }
        }
    }
}

#endif