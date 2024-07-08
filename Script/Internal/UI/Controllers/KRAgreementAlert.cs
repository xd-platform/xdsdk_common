#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class KRAgreementAlert : UIElement
    {
        private Button agreeAllButton;
        
        private Toggle userProtocolToggle;
        private Toggle privacyTermToggle;
        private Toggle ageToggle;
        private Toggle pushDayToggle;
        private Toggle pushNightToggle;
        private Button disagreeButton;
        private Button agreeButton;

        private Sprite agreeButtonNormalSprite;
        
        private Button uploadConfigButton;
        private int uploadConfigButtonClickedCount;
        private const int UploadConfigButtonClickTriggerCount = 10;

        protected void Awake()
        {
            agreeAllButton = transform.Find("AgreeAllOption/AgreementAllOptionButton").GetComponent<Button>();
            
            userProtocolToggle = transform.Find("UserProtocolOption/UserProtocolOptionToggle").GetComponent<Toggle>();
            privacyTermToggle = transform.Find("PrivacyTermOption/PrivacyTermOptionToggle").GetComponent<Toggle>();
            ageToggle = transform.Find("AgeOption/AgeOptionToggle").GetComponent<Toggle>();
            pushDayToggle = transform.Find("PushDayOption/PushDayOptionToggle").GetComponent<Toggle>();
            pushNightToggle = transform.Find("PushNightOption/PushNightOptionToggle").GetComponent<Toggle>();
            
            disagreeButton = transform.Find("ButtonConatiner/DisagreeButton").GetComponent<Button>();
            agreeButton = transform.Find("ButtonConatiner/AgreeButton").GetComponent<Button>();
            agreeButtonNormalSprite = agreeButton.image.sprite;
            
            uploadConfigButton = transform.Find("UploadConfigButton").GetComponent<Button>();
            uploadConfigButton.onClick.AddListener(OnUploadConfigButtonClicked);
        }

        protected void Start()
        {
            agreeAllButton.onClick.AddListener(() =>
            {
                bool newState = !IsFullAgree();
                userProtocolToggle.isOn = newState;
                privacyTermToggle.isOn = newState;
                ageToggle.isOn = newState;
                pushDayToggle.isOn = newState;
                pushNightToggle.isOn = newState;
            });
            
            UnityAction<bool> toggleAction = _ =>
            {
                UpdateUIState();
            };
            userProtocolToggle.onValueChanged.AddListener(toggleAction);
            privacyTermToggle.onValueChanged.AddListener(toggleAction);
            ageToggle.onValueChanged.AddListener(toggleAction);
            pushDayToggle.onValueChanged.AddListener(toggleAction);
            pushNightToggle.onValueChanged.AddListener(toggleAction);
            
            UpdateUIState();
            
            // LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            
            HyperlinkText userProtocolLabel = transform.Find("UserProtocolOption/UserProtocolOptionLabel").GetComponent<HyperlinkText>();
            string requiredText = $"<color=#888888>(필수)</color>";
            string agreementText = $"<color=#222222>게임 서비스 이용 약관에 대한 동의</color>";
            string linkText = $"<color=#15C5CE><a href=agreement>[내용보기]</a></color>";
            userProtocolLabel.text = $"{requiredText} {agreementText} {linkText}";
            userProtocolLabel.OnClicked = _ => {
                if (extra != null && extra.TryGetValue("userAgreement", out var url)) {
                    Application.OpenURL(url as string);
                } else {
                    XDGLogger.Error("UserAgreement URL is null");
                }
            };

            HyperlinkText privacyTermLabel = transform.Find("PrivacyTermOption/PrivacyTermOptionLabel").GetComponent<HyperlinkText>();
            agreementText = $"<color=#222222>개인정보 수집 및 이용에 대한 동의</color>";
            privacyTermLabel.text = $"{requiredText} {agreementText} {linkText}";
            privacyTermLabel.OnClicked = _ => {
                if (extra != null && extra.TryGetValue("privacyPolicy", out var url)) {
                    Application.OpenURL(url as string);
                }
                else
                {
                    XDGLogger.Error("PrivacyPolicy URL is null");
                }
            };
            
            var ageLabel = transform.Find("AgeOption/AgeOptionLabel").GetComponent<Text>();
            string optionalText = $"<color=#888888>(선택)</color>";
            agreementText = $"<color=#222222>만 14 세 이상입니다</color>";
            ageLabel.text = $"{optionalText} {agreementText}";
            
            var dayLabel = transform.Find("PushDayOption/PushDayOptionLabel").GetComponent<Text>();
            agreementText = $"<color=#222222>이벤트 푸시(8 시~21 시) 알림과 수신 동의</color>";
            dayLabel.text = $"{optionalText} {agreementText}";
            
            var nightLabel = transform.Find("PushNightOption/PushNightOptionLabel").GetComponent<Text>();
            agreementText = $"<color=#222222>야간 이벤트 푸시 (21 시~8 시) 알림과 수신 동의</color>";
            nightLabel.text = $"{optionalText} {agreementText}";
            
            disagreeButton.onClick.AddListener(OnDisagreeButtonClicked);
            agreeButton.onClick.AddListener(OnAgreeButtonClicked);
        }

        private void UpdateUIState() {
            var sprite = Resources.Load<Sprite>("Textures/checkbox_kr_empty");
            if (IsFullAgree())
            {
                sprite = Resources.Load<Sprite>("Textures/checkbox_kr_full");
            } else if (IsHalfAgree()) 
            {
                sprite = Resources.Load<Sprite>("Textures/checkbox_kr_half");
            }
            agreeAllButton.image.sprite = sprite;
            
            agreeButton.image.sprite = IsHalfAgree() ? agreeButtonNormalSprite : agreeButton.spriteState.disabledSprite;
            agreeButton.transition = IsFullAgree() ? Selectable.Transition.SpriteSwap : Selectable.Transition.None;
        }
        
        private bool IsHalfAgree() {
            return userProtocolToggle.isOn && privacyTermToggle.isOn && ageToggle.isOn;
        }
        
        private bool IsFullAgree() {
            return IsHalfAgree() && pushDayToggle.isOn && pushNightToggle.isOn;
        }

        private void OnAgreeButtonClicked() {
            if (!IsHalfAgree()) {
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
                            userProtocolToggle.isOn = true;
                            privacyTermToggle.isOn = true;
                            ageToggle.isOn = true;
                            pushDayToggle.isOn = true;
                            pushNightToggle.isOn = true;
                            OnAgreeButtonClicked();
                        }
                        else if (code == 1)
                        {
                            XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                        }
                    }); 
                return;
            }
            var objects = new Dictionary<string, object> {
                { "push_agreement", pushDayToggle.isOn && pushNightToggle },
                { "push_agreement_day", pushDayToggle.isOn},
                { "push_agreement_night", pushNightToggle.isOn}
            };
            OnCallback(0, objects);
        }

        private void OnDisagreeButtonClicked() {
            XD.SDK.Common.PC.Internal.UIManager.ShowUI<DisagreeConfirmAlert>("DisagreeConfirmAlert", null, (code, data) => {
                if (code == 0) {
                    XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                } else if (code == 1) {
                    OnCallback(1, null);
                }
            });
        }
        
        private void OnUploadConfigButtonClicked() {
            uploadConfigButtonClickedCount++;
            if (uploadConfigButtonClickedCount >= UploadConfigButtonClickTriggerCount) {
                UploadConfigModule.TriggerUploadConfig();
            }
        }
    }
}
#endif