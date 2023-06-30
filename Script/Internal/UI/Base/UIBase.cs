#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    public class UIBase : MonoBehaviour {
        public virtual void OnEnter(){
        }

        public virtual void OnPause(){
            gameObject.SetActive(false);
        }

        public virtual void OnResume(){
            gameObject.SetActive(true);
        }

        public virtual void OnExit(){
            Destroy(gameObject);
        }
    }
}
#endif