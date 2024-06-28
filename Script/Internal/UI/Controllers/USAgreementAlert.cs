#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    public class USAgreementAlert : GeneralAgreementAlert {
        protected Toggle optionToggle;

        protected override void Awake() {
            base.Awake();
            optionToggle = transform.Find("OptionToggle").GetComponent<Toggle>();
        }

        protected override void Start() {
            optionToggle.onValueChanged.AddListener(OnOptionToggleChanged);
            UpdateAgreeButton();

            Text label = optionToggle.transform.Find("Label").GetComponent<Text>();

            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            string text = localizableString.IsAdultAgreement;

            // 计算字符宽度
            TextGenerator generator = label.cachedTextGenerator;
            TextGenerationSettings settings = label.GetGenerationSettings(Vector2.zero);
            // Windows 下有误差
            float width = generator.GetPreferredWidth(text, settings) / label.pixelsPerUnit + 40;
            float height = 20;

            RectTransform panelRectTransform = gameObject.GetComponent<RectTransform>();
            float maxWidth = panelRectTransform.rect.width - 30;

            RectTransform toggleRectTransform = optionToggle.GetComponent<RectTransform>();

            if (width > maxWidth) {
                width = maxWidth;
                height = 40;
            }

            toggleRectTransform.sizeDelta = new Vector2(width, height);

            label.text = text;

            LayoutRebuilder.ForceRebuildLayoutImmediate(toggleRectTransform);

            base.Start();
        }

        void OnOptionToggleChanged(bool isOn) {
            UpdateAgreeButton();
        }

        void UpdateAgreeButton() {
            agreeButton.image.sprite = optionToggle.isOn ?
                agreeButtonNormalSprite : agreeButton.spriteState.disabledSprite;
            agreeButton.transition = optionToggle.isOn ?
                Selectable.Transition.None : Selectable.Transition.SpriteSwap;
        }

        protected override void OnAgreeButtonClicked() {
            if (!optionToggle.isOn) {
                LocalizableString localizable = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
                Dictionary<string, object> config = new Dictionary<string, object> {
                    { DisagreeConfirmAlert.CONTENT_KEY, localizable.AgreementOptionNotAgreeAlertContent },
                    { DisagreeConfirmAlert.SECONDARY_TEXT_KEY, localizable.AgreementOptionNotAgreeAlertConfirm },
                    { DisagreeConfirmAlert.PRIMARY_TEXT_KEY, localizable.AgreementOptionNotAgreeAlertCancel }
                };
                XD.SDK.Common.PC.Internal.UIManager.ShowUI<DisagreeConfirmAlert>("DisagreeConfirmAlert", config,
                    (code, data) =>
                    {
                        if (code == 0)
                        {
                            
                            XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                            optionToggle.isOn = true;
                            OnAgreeButtonClicked();
                        }
                        else if (code == 1)
                        {
                            XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                        }
                    }); 
                return;
                return;
            }
            base.OnAgreeButtonClicked();
        }
    }
}
#endif