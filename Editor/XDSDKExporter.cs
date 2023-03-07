using System.IO;
using UnityEngine;
using UnityEditor;

namespace XD.SDK.PC.Editor
{
    public class XDSDKExporter
    {
        [MenuItem("XD/Export PC SDK")]
        static void Export()
        {
            string path = EditorUtility.OpenFolderPanel("Export path", "", "");
            string[] assetPaths = new string[]
            {
                "Assets/XDSDK/PC/Editor",
                "Assets/XDSDK/PC/Plugins",
                "Assets/XDSDK/PC/Resources",
                "Assets/XDSDK/PC/Script",
                "Assets/XDSDK/PC/Textures",
                "Assets/XDSDK/PC/Wrapper",
                "Assets/XDSDK/PC/CHANGELOG.md",
                "Assets/XDSDK/PC/package.json",
                "Assets/XDSDK/PC/README.md"
            };
            string exportPath = Path.Combine(path, "xd-pc-sdk.unitypackage");
            AssetDatabase.ExportPackage(assetPaths, exportPath,
                ExportPackageOptions.Recurse);
            Debug.Log("Export done.");
        }
    }
}