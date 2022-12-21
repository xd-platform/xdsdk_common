#if UNITY_EDITOR && UNITY_ANDROID
using System;
using System.Linq;
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
            for (int i = (int)CustomTemplateType.AndroidManifest; i <= (int)CustomTemplateType.GradleProperties; i++)
            {
                AndroidUtils.ToggleCustomTemplateFile((CustomTemplateType)i,
                    gardleTemplateToggleRecord[(CustomTemplateType)i]);
            }
        }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            gardleTemplateToggleRecord.Clear();
            for (int i = (int)CustomTemplateType.AndroidManifest; i <= (int)CustomTemplateType.GradleProperties; i++)
            {
                var haveCustomGradleTemplate = AndroidUtils.HaveCustomTemplateFile((CustomTemplateType)i);
                gardleTemplateToggleRecord.Add((CustomTemplateType)i, haveCustomGradleTemplate);
            }
            
            var interfaceType = typeof(IAndroidGradleContextProvider);
            var gradleContextProviders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(clazz => interfaceType.IsAssignableFrom(clazz) && clazz.IsClass);

            if (gradleContextProviders == null) return;
            var providers = new List<IAndroidGradleContextProvider>();
            foreach (var gradleContextProviderType in gradleContextProviders)
            {
                var provider = Activator.CreateInstance(gradleContextProviderType) as IAndroidGradleContextProvider;
                providers.Add(provider);
            }
            providers.Sort((a,b)=>a.Priority.CompareTo(b.Priority));

            foreach (var provider in providers)
            {
                var gradleContexts = provider.GetAndroidGradleContext();
                if (gradleContexts != null)
                {
                    gradleContexts.Reverse();
                    foreach (var gradleContext in gradleContexts)
                    {
                        AndroidUtils.ProcessCustomGradleContext(gradleContext);
                    }
                }
            }
        }
    }
    
}
#endif