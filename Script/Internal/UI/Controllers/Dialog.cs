#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class Dialog : UIElement {
        public static readonly string TITLE_KEY = "title";
        public static readonly string CONTENT_KEY = "content";
        public static readonly string PRIMARY_TEXT_KEY = "primaryText";
        public static readonly string SECONDARY_TEXT_KEY = "secondaryText";

        private void Start() {
            Text titleText = transform.Find("Modal/Header/TitleText").GetComponent<Text>();
            titleText.text = Extra[TITLE_KEY] as string;
            Text contentText = transform.Find("Modal/Content/ContentText").GetComponent<Text>();
            contentText.text = ProcessContent(Extra[CONTENT_KEY] as string);

            Button closeButton = transform.Find("Modal/Header/CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(OnCloseClicked);

            Button primaryButton = transform.Find("Modal/ButtonContainer/PrimaryButton").GetComponent<Button>();
            primaryButton.onClick.AddListener(OnPrimaryClicked);
            Text primaryText = primaryButton.transform.Find("Text").GetComponent<Text>();
            primaryText.text = Extra[PRIMARY_TEXT_KEY] as string;

            Button secondaryButton = transform.Find("Modal/ButtonContainer/SecondaryButton").GetComponent<Button>();
            secondaryButton.onClick.AddListener(OnSecondaryClicked);
            Text secondaryText = secondaryButton.transform.Find("Text").GetComponent<Text>();
            secondaryButton.gameObject.SetActive(Extra.ContainsKey(SECONDARY_TEXT_KEY));

            if (Extra.ContainsKey(SECONDARY_TEXT_KEY)) {
                secondaryText.text = Extra[SECONDARY_TEXT_KEY] as string;
            }
        }

        private void OnCloseClicked() {
            OnCallback(-1, null);
        }

        private void OnPrimaryClicked() {
            OnCallback(0, null);
        }

        private void OnSecondaryClicked() {
            OnCallback(1, null);
        }

        private static string ProcessContent(string content) {
            return content
                ?.Replace("<font color=", "<color=")
                .Replace("<span color=", "<color=")
                .Replace("</font>", "</color>")
                .Replace("</span>", "</color>")
                .Replace("<br>", "\n")
                .Replace(" ", "\u00A0");
        }
    }
}
#endif