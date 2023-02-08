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
            int xdconfigFileCount = 0;
            var xdconfigGuids = AssetDatabase.FindAssets("XDConfig");
            string xdconfigPath = null;
            foreach (var guid in xdconfigGuids)
            {
                var xdPath = AssetDatabase.GUIDToAssetPath(guid);
                var fileInfo = new FileInfo(xdPath);
                var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
                var fullPath = Path.Combine(parentFolder, xdPath);
                if (fileInfo.Name != "XDConfig.json") continue;
                xdconfigFileCount++;
                xdconfigPath = fullPath;
            }

            if (xdconfigFileCount > 1)
            {
                Debug.LogError("[XD.Common] XDConfig.json 配置文件多余一个同时存在！");
            }
            
            if (xdconfigFileCount <= 0)
            {
                Debug.LogError("[XD.Common] XDConfig.json 配置文件不存在！");
                return null;
            }
            
            return xdconfigPath;
        }

        public int callbackOrder{
            get{ return 998; }
        }
    }
}
#endif