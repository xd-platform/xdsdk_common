#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour {
        public static readonly int RESULT_FAILED = -1;
        public static readonly int RESULT_SUCCESS = 0;
        public static readonly int RESULT_BACK = 1;
        public static readonly int RESULT_CLOSE = 2;

        private static UIManager instance;
        private Transform windowStack;
        private GameObject mask;
        private GameObject toast;
        private GameObject loading;

        private readonly Stack<UIElement> uiElements = new Stack<UIElement>();

        public static void Dismiss() {
            Instance.PopUIElement();
        }

        public static void DismissAll() {
            while (Instance.uiElements.Count > 0) {
                Instance.PopUIElement();
            }
        }

        public static T ShowUI<T>(Dictionary<string, object> configs, Action<int, object> callback)
            where T : UIElement {
            return Instance.PushUIElement<T>(typeof(T).Name, configs, callback);
        }

        public static T ShowUI<T>(string prefabName, Dictionary<string, object> configs, Action<int, object> callback)
            where T : UIElement {
            return Instance.PushUIElement<T>(prefabName, configs, callback);
        }

        public static void ShowToast(string msg) {
            GameObject toast = Instance.toast;
            toast.transform.Find("Text").GetComponent<Text>().text = msg;
            toast.transform.SetAsLastSibling();
            toast.SetActive(true);
            Instance.CancelInvoke(nameof(DismissToast));
            Instance.Invoke(nameof(DismissToast), 1);
        }

        private void DismissToast() {
            toast.transform.SetSiblingIndex(1);
            toast.SetActive(false);
        }

        public static void ShowLoading(){
            Instance.loading.transform.SetAsLastSibling();
            Instance.loading.SetActive(true);
            Instance.UpdateMask();
        }
        
        public static void DismissLoading(){
            Instance.loading.transform.SetSiblingIndex(1);
            Instance.loading.SetActive(false);
            Instance.UpdateMask();
        }

        private static UIManager Instance {
            get {
                if (instance == null) {
                    GameObject managerObject = Instantiate(Resources.Load("Prefabs/XDSDKUI")) as GameObject;
                    DontDestroyOnLoad(managerObject);

                    managerObject.name = "XDSDKUI";
                    instance = managerObject.AddComponent<UIManager>();
                    managerObject.AddComponent<ContainerWindow>();

                    instance.mask = managerObject.transform.Find("Background").gameObject;

                    instance.windowStack = managerObject.transform.Find("WindowStack");

                    instance.toast = managerObject.transform.Find("Toast").gameObject;
                    instance.toast.gameObject.SetActive(false);

                    instance.loading = managerObject.transform.Find("LoadingView").gameObject;
                    instance.loading.gameObject.SetActive(false);
                }

                return instance;
            }
        }

        private T PushUIElement<T>(
            string prefabName,
            Dictionary<string, object> extra,
            Action<int, object> callback) where T : UIElement {
            GameObject gameObj = Instantiate(Resources.Load("Prefabs/" + prefabName)) as GameObject;
            if (gameObj == null) {
                XDLogger.Debug("没找到 prefab named： \"" + prefabName + "\"");
                return null;
            } else {
                gameObj.name = prefabName;
                DontDestroyOnLoad(gameObj);
                T element = UI.GetComponent<T>(gameObj);
                element.Extra = extra;
                element.Callback += callback;
                element.transform.SetParent(Instance.windowStack, false);

                uiElements.Push(element);

                UIAnimator animator = UI.GetComponent<UIAnimator>(gameObj);
                animator.DoEnterAnimation(null, element, () => {
                    UpdateMask();
                });
                return element;
            }
        }

        private void PopUIElement(){
            if (uiElements.Count == 0){
                XDLogger.Debug("没有 UIElement 子类可处理.");
            } else{
                UIElement element = uiElements.Pop();

                UIAnimator animator = UI.GetComponent<UIAnimator>(element.gameObject);
                animator.DoExitAnimation(element, null, () => {
                    Destroy(element.gameObject);
                    UpdateMask();
                });
            }
        }

        private void UpdateMask() {
            bool showMask = uiElements.Count > 0 || loading.activeSelf;
            mask.SetActive(showMask);
        }
    }
}
#endif