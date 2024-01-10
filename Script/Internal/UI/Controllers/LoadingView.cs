#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class LoadingView : UIElement {

        public Image LoadingImage;
        public float rotateSpeed = 150;

        void Update() {
            LoadingImage.transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }
}
#endif