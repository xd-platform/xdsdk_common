// #if UNITY_EDITOR && UNITY_ANDROID
// using System.IO;
// using UnityEditor.Android;
// using UnityEngine;
//
// public class XDGAndroidPaymentProcessor : IPostGenerateGradleAndroidProject{
//     void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path){
//         var projectPath = path;
//         if (path.Contains("unityLibrary")){
//             projectPath = path.Substring(0, path.Length - 12);
//         }
//
//         var parentFolder = Directory.GetParent(Application.dataPath)?.FullName;
//
//         //配置路径
//         var gradlePropertiesFile = projectPath + "/gradle.properties";
//         var baseProjectGradle = projectPath + "/build.gradle";
//         var launcherGradle = projectPath + "/launcher/build.gradle";
// #if UNITY_2019_3_OR_NEWER
//         var unityLibraryGradle = projectPath + "/unityLibrary/build.gradle";
// #else
//         var unityLibraryGradle = projectPath + "/build.gradle";
// #endif
//         
//         //implementation 可根据需要添加或删除
//         if (File.Exists(unityLibraryGradle))
//         {
//             Debug.Log("编辑 unityLibraryGradle");
//             var writerHelper = new XD.SDK.Common.Editor.XDGScriptHandlerProcessor(unityLibraryGradle);
//             writerHelper.WriteBelow(@"implementation 'com.google.code.gson:gson:2.8.6'", @"
//     implementation 'androidx.browser:browser:1.4.0'
//             ");
//         }
//         else
//         {
//             Debug.LogWarning("打包警告 ---  unityLibraryGradle 不存在");
//         }
//     }
//
//     public int callbackOrder{
//         get{ return 1098; }
//     }
// }
// #endif