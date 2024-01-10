#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class ScrollRectListener : MonoBehaviour, IEndDragHandler {
        public void OnEndDrag(PointerEventData eventData) {
            Button[] buttons = transform.GetComponentsInChildren<Button>();
            Debug.Log(buttons.Length);
            foreach (Button button in buttons) {
                button.OnDeselect(null);
            }
        }
    }
}
#endif