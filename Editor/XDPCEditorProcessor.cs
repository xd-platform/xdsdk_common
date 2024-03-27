#if UNITY_ANDROID || UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using LC.Newtonsoft.Json;

namespace XD.SDK.PC.Editor
{
    public class XDPCEditorProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        
        private static List<string> excludedFolders = new List<string> {};

        public void OnPreprocessBuild(BuildReport report)
        {
            Application.logMessageReceived += OnBuildError;
            ProcessFolder();
        }

    
        public void OnPostprocessBuild(BuildReport report)
        {
            // IF BUILD FINISHED AND SUCCEEDED, STOP LOOKING FOR ERRORS
            Application.logMessageReceived -= OnBuildError;
            RecoverFolder();
            
        }

        // CALLED DURING BUILD TO CHECK FOR ERRORS
        private void OnBuildError(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Error)
            {
                Debug.LogError("FAILED TO BUILD, STOP LISTENING FOR ERRORS");
                Application.logMessageReceived -= OnBuildError;
                RecoverFolder();
            }
        }

        private List<string> LoadExcludeFolders()
        {
            var result = new List<string>();
            var guids = AssetDatabase.FindAssets("XDPCExcludeFolderSetting");
            if (guids == null) return null;
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                using (var file = File.OpenText(assetPath))
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<List<string>>(file.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat(string.Format(
                            "[XD::PCEditor Processor] Deserialize excludedFolders Error! Error Msg:\n{0}\nError Stack:\n{1}",
                            e.Message, e.StackTrace));
                    }

                    break;
                }
            }

            return result;
        }

        private void ProcessFolder()
        {
            excludedFolders = LoadExcludeFolders();
            if (excludedFolders == null) return;
            foreach (var removedFolder in excludedFolders)
            {
                var folder = Path.Combine(Application.dataPath, removedFolder);
                if (Directory.Exists(folder))
                {
                    if (Directory.Exists(folder + "~"))
                        Directory.Delete(folder + "~", true);
                    Directory.Move(folder, folder + "~");
                    Debug.LogFormat($"[XDSDK::PC] Temp remove Folder of {folder}");
                }
                else
                {
                    Debug.LogWarningFormat($"[XDSDK::PC] Can't Find Folder of {folder} to remove");
                }
                if (File.Exists(folder + ".meta"))
                    File.Delete(folder + ".meta");
            }
            AssetDatabase.Refresh();
        }

        private void RecoverFolder()
        {
            if (excludedFolders == null) return;
            foreach (var removedFolder in excludedFolders)
            {
                var folder = Path.Combine(Application.dataPath, removedFolder);
                var fixedFolder = folder + "~";
                if (Directory.Exists(fixedFolder))
                    Directory.Move(fixedFolder, folder);
            }
            AssetDatabase.Refresh();
        }

        public int callbackOrder{
            get{ return 890; }
        }
    }
}
#endif