#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class DisagreeConfirmAlert : UIElement {
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
            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            contentText.text = localizableString.AgreementDisagreeConfirmContent;
            exitButton.transform.Find("Text").GetComponent<Text>().text = localizableString.AgreementDisagreeConfirmExit;
            backButton.transform.Find("Text").GetComponent<Text>().text = localizableString.AgreementDisagreeConfirmBack;
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
