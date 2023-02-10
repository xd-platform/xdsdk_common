using System.IO;
using UnityEditor;
using UnityEngine;

namespace XD.SDK.Common.Editor
{
    public static class XDGCommonEditorUtils
    {
        public static string GetXDConfigPath()
        {
            string xdconfigPath = null;
            
            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
            xdconfigPath = Path.Combine(parentFolder, "Assets/XDConfig.json");

            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find XDConfig.json on Assets folder");
            xdconfigPath = Path.Combine(parentFolder, "Assets/Plugins/XDConfig.json");
            
            if (File.Exists(xdconfigPath))
            {
                return xdconfigPath;
            }
            
            Debug.LogWarningFormat($"[XDSDK.Common] Can't find XDConfig.json on Assets folder or Assets/Plugins folder");

            bool find = false;
            var xdconfigGuids = AssetDatabase.FindAssets("XDConfig");
            foreach (var guid in xdconfigGuids)
            {
                var xdPath = AssetDatabase.GUIDToAssetPath(guid);
                var fileInfo = new FileInfo(xdPath);
                if (fileInfo.Name != "XDConfig.json") continue;
                xdconfigPath = Path.Combine(parentFolder, xdPath);
                find = true;
                break;
            }

            if (find)
            { 
                Debug.LogFormat($"[XDSDK.Common] Can find XDConfig.json at: {xdconfigPath}");
                return xdconfigPath;
            }
            Debug.LogFormat($"[XDSDK.Common] CAN NOT find XDConfig.json!!");
            return "";
        }
    }
}