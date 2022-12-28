#if UNITY_EDITOR && UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace XD.SDK.Common.Editor
{
    public class XDGAndroidGradleProcessor : IPreprocessBuildWithReport, IPostGenerateGradleAndroidProject
    {
        private static Dictionary<CustomTemplateType, bool> gardleTemplateToggleRecord = new Dictionary<CustomTemplateType, bool>();

        public int callbackOrder => 1100;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            // for (int i = (int)CustomTemplateType.AndroidManifest; i <= (int)CustomTemplateType.GradleProperties; i++)
            // {
            //     AndroidUtils.ToggleCustomTemplateFile((CustomTemplateType)i,
            //         gardleTemplateToggleRecord[(CustomTemplateType)i]);
            // }
        }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            gardleTemplateToggleRecord.Clear();
            for (int i = (int)CustomTemplateType.AndroidManifest; i <= (int)CustomTemplateType.GradleProperties; i++)
            {
                var haveCustomGradleTemplate = AndroidUtils.HaveCustomTemplateFile((CustomTemplateType)i);
                gardleTemplateToggleRecord.Add((CustomTemplateType)i, haveCustomGradleTemplate);
            }
            
            var providers = AndroidUtils.Load();
            if (providers == null) return;
            
            foreach (var provider in providers)
            {
                if (provider.AndroidGradleContext == null) continue;
                foreach (var context in provider.AndroidGradleContext)
                {
                    try
                    {
                        AndroidUtils.ProcessCustomGradleContext(context);
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat($"[Tap::AndroidGradleProcessor] Process Custom Gradle Context Error! Error Msg:\n{e.Message}\nError Stack:\n{e.StackTrace}");
                    }
                }
            }
        }
    }
    
}
#endif