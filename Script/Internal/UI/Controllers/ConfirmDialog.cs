#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class ConfirmDialog : UIElement {
        public static readonly string TITLE_KEY = "title";
        public static readonly string CONTENT_KEY = "content";
        public static readonly string CONFIRM_TEXT_KEY = "confirmText";

        private void Start() {
            Text titleText = transform.Find("Modal/Header/TitleText").GetComponent<Text>();
            titleText.text = Extra[TITLE_KEY] as string;
            Text contentText = transform.Find("Modal/Content/ContentText").GetComponent<Text>();
            contentText.text = Extra[CONTENT_KEY] as string;

            Button closeButton = transform.Find("Modal/Header/CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(OnCloseClicked);

            Button primaryButton = transform.Find("Modal/ButtonContainer/ConfirmButton").GetComponent<Button>();
            primaryButton.onClick.AddListener(OnConfirmClicked);
            Text primaryText = primaryButton.transform.Find("Text").GetComponent<Text>();
            primaryText.text = Extra[CONFIRM_TEXT_KEY] as string;
        }

        private void OnCloseClicked() {
            OnCallback(-1, null);
        }

        private void OnConfirmClicked() {
            OnCallback(0, null);
        }
    }
}
#endif