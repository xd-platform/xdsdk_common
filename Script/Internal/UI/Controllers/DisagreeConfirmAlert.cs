#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class DisagreeConfirmAlert : UIElement {
        public static readonly string CONTENT_KEY = "content";
        public static readonly string PRIMARY_TEXT_KEY = "primaryText";
        public static readonly string SECONDARY_TEXT_KEY = "secondaryText";
        
        Text contentText;
        Button exitButton;
        Button backButton;

        private void Awake() {
            contentText = GameObject.Find("DialogBG/ContentText").GetComponent<Text>();
            exitButton = GameObject.Find("DialogBG/ButtonContainer/ExitButton").GetComponent<Button>();
            backButton = GameObject.Find("DialogBG/ButtonContainer/BackButton").GetComponent<Button>();

            exitButton.onClick.AddListener(OnExitButtonClicked);
            backButton.onClick.AddListener(OnViewButtonClicked);
        }

        private void Start() {
            contentText.text = Extra[CONTENT_KEY] as string;
            exitButton.transform.Find("Text").GetComponent<Text>().text = Extra[PRIMARY_TEXT_KEY] as string;
            backButton.transform.Find("Text").GetComponent<Text>().text = Extra[SECONDARY_TEXT_KEY] as string;
        }

        void OnExitButtonClicked() {
            OnCallback(1, null);
        }

        void OnViewButtonClicked() {
            OnCallback(0, null);
        }
    }
}
#endif
