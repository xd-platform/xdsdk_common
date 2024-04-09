using System.IO;
using UnityEditor;
using UnityEngine;

namespace XDSDK.Mobile.ADSubPackage.Editor
{
    public static class Utils
    {
        public static string GetXDConfigPath(string configFileName = "XDConfig", bool searchAllAssets = true)
        {
            var parentDirectory = Directory.GetParent(Application.dataPath)?.FullName;

            if (parentDirectory != null)
            {
                var xdConfigPath = Path.Combine(parentDirectory, $"Assets/{configFileName}.json");

                if (File.Exists(xdConfigPath))
                {
                    return xdConfigPath;
                }
            
                Debug.LogWarningFormat($"[XDSDK.ADSubPackage] Can't find {configFileName}.json on Assets folder");
                xdConfigPath = Path.Combine(parentDirectory, $"Assets/Plugins/{configFileName}.json");

                if (File.Exists(xdConfigPath))
                {
                    return xdConfigPath;
                }
            
                Debug.LogWarningFormat($"[XDSDK.ADSubPackage] Can't find {configFileName}.json on Assets folder or Assets/Plugins folder");
                if (searchAllAssets == false)
                {
                    return "";
                }

                var find = false;
                var xdConfigGuids = AssetDatabase.FindAssets(configFileName);
                foreach (var guid in xdConfigGuids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var fileInfo = new FileInfo(path);
                    if (fileInfo.Name != $"{configFileName}.json")
                    {
                        continue;
                    }

                    xdConfigPath = Path.Combine(parentDirectory, path);
                    find = true;
                    break;
                }

                if (find)
                {
                    Debug.LogFormat($"[XDSDK.ADSubPackage] Can find {configFileName}.json at: {xdConfigPath}");
                    return xdConfigPath;
                }
            }

            Debug.LogFormat($"[XDSDK.ADSubPackage] CAN NOT find {configFileName}.json!!");
            return "";
        }
    }
}