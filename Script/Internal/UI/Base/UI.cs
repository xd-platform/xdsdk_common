#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    public static class UI{
        public static T GetComponent<T>(GameObject obj) where T : UnityEngine.Component {
            T component = obj.GetComponent<T>();

            if (component == null){
                component = obj.AddComponent<T>();
            }

            return component;
        }
    }
}
#endif