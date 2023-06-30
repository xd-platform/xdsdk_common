using System.IO;
using UnityEditor;
using UnityEngine;

namespace XD.SDK.Common.Editor
{
    public static class XDGCommonEditorUtils
    {
        public static string GetXDConfigPath(string configFile = "XDConfig", bool searchAllAssets = true)
        {
            string xdconfigPath = null;
            
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            xdconfigPath = Path.Combine(parentFolder, $"Assets/{configFile}.json");

            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find {configFile}.json on Assets folder");
            xdconfigPath = Path.Combine(parentFolder, $"Assets/Plugins/{configFile}.json");
            
            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find {configFile}.json on Assets folder or Assets/Plugins folder");
            if (searchAllAssets == false) return "";
            bool find = false;
            var xdconfigGuids = AssetDatabase.FindAssets(configFile);
            foreach (var guid in xdconfigGuids)
            {
                var xdPath = AssetDatabase.GUIDToAssetPath(guid);
                var fileInfo = new FileInfo(xdPath);
                if (fileInfo.Name != $"{configFile}.json") continue;
                xdconfigPath = Path.Combine(parentFolder, xdPath);
                find = true;
                break;
            }

            if (find)
            { 
                Debug.LogFormat($"[XDSDK.Common] Can find {configFile}.json at: {xdconfigPath}");
                return xdconfigPath;
            }
            Debug.LogFormat($"[XDSDK.Common] CAN NOT find {configFile}.json!!");
            return "";
        }
    }
}