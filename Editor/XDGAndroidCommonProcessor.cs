#if UNITY_EDITOR && UNITY_ANDROID
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

namespace XD.SDK.Common.Editor{
    public class XDGAndroidCommonProcessor : IPostGenerateGradleAndroidProject{
        void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path){
            var projectPath = path;
            if (path.Contains("unityLibrary")){
                projectPath = path.Substring(0, path.Length - 12);
            }

            var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;

            
            //拷贝 SDK json 文件，必须的
            var configJson = GetXDConfigPath();
            if (File.Exists(configJson)){
    #if UNITY_2019_3_OR_NEWER
                File.Copy(configJson, projectPath + "/unityLibrary/src/main/assets/XDConfig.json", true);
    #else
                File.Copy(configJson, projectPath + "/src/main/assets/XDConfig.json", true);
    #endif
            } else{
                Debug.LogWarning("打包警告 ---  拷贝的json配置文件不存在");
            }
            
    #if !UNITY_2019_3_OR_NEWER
            Debug.Log("修改 gradlePropertiesFile");
            var gradlePropertiesFile = projectPath + "/gradle.properties";
            bool containsAndroidX = false;
            using(StreamReader sr = File.OpenText(gradlePropertiesFile))
            {
                string s = "";
                s = sr.ReadToEnd();
                containsAndroidX = s.Contains("android.useAndroidX=true") && s.Contains("android.enableJetifier=true");
            }

            if (false == containsAndroidX)
            {
                using(StreamWriter sw = File.AppendText(gradlePropertiesFile))
                {
                    sw.WriteLine("\nandroid.useAndroidX=true");
                    sw.WriteLine("android.enableJetifier=true");
                }
            }
    #endif
        }

        private string GetXDConfigPath()
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

        public int callbackOrder{
            get{ return 998; }
        }
    }
}
#endif