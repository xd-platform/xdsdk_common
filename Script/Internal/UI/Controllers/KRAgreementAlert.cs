#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class KRAgreementAlert : GeneralAgreementAlert {
        internal readonly static string PRIVACY_URL_KEY = "privacyUrl";

        private Toggle collectionToggle;
        private Toggle pushToggle;

        protected override void Awake() {
            base.Awake();
            collectionToggle = transform.Find("CollectionOption/CollectionOptionToggle").GetComponent<Toggle>();
            pushToggle = transform.Find("PushOption/PushOptionToggle").GetComponent<Toggle>();
        }

        protected override void Start() {
            collectionToggle.onValueChanged.AddListener(OncollectionToggleChanged);
            UpdateAgreeButton();

            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();

            Text collectionLabel = transform.Find("CollectionOption/CollectionLabel").GetComponent<Text>();
            collectionLabel.text = $"<color=#888888>({localizableString.Required}) </color>{localizableString.Over14yearsOld}";

            HyperlinkText pushLabel = transform.Find("PushOption/PushLabel").GetComponent<HyperlinkText>();
            string optionalText = $"<color=#888888>({localizableString.Optional})</color>";
            string agreementText = localizableString.IAgreePersonalInformationAgreementReverse ?
                $"<color=#15C5CE><a href=agreement>{localizableString.PersonalInformationAgreement}</a></color>{localizableString.IAgree}" :
                $"{localizableString.IAgree}<color=#15C5CE><a href=agreement>{localizableString.PersonalInformationAgreement}</a></color>";
            pushLabel.text = $"{optionalText} {agreementText}";
            pushLabel.OnClicked = tag => {
                string privacyUrl = Extra[PRIVACY_URL_KEY] as string;
                Application.OpenURL(privacyUrl);
            };

            base.Start();
        }

        void OncollectionToggleChanged(bool isOn) {
            UpdateAgreeButton();
        }

        void UpdateAgreeButton() {
            agreeButton.image.sprite = collectionToggle.isOn ?
                agreeButtonNormalSprite : agreeButton.spriteState.disabledSprite;
            agreeButton.transition = collectionToggle.isOn ?
                Selectable.Transition.SpriteSwap : Selectable.Transition.None;
        }

        protected override void OnAgreeButtonClicked() {
            if (!collectionToggle.isOn) {
                LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
                XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.KRConfirmYourAge);
                UpdateAgreeButton();
                return;
            }
            Dictionary<string, object> extra = new Dictionary<string, object> {
                { "push_agreement", pushToggle.isOn }
            };
            OnCallback(0, extra);
        }
    }
}
#endif