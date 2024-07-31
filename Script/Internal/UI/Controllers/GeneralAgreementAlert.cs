#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal
{
    public class GeneralAgreementAlert : UIElement
    {
        private VerticalLayoutGroup _summaryGroup;
        private GameObject _optionGroup;
        private AgreementOptionAll _allOption;

        private Text _titleText;
        private HyperlinkText _summaryText;

        private HyperlinkText _promptText;
        
        // private 
        private Button _disagreeButton;
        private Button _agreeButton;

        private Sprite _agreeButtonNormalSprite;

        private const int UploadConfigButtonClickTriggerCount = 10;
        private Button _uploadConfigButton;
        private int _uploadConfigButtonClickedCount;

        private bool FirstCheck => Extra["firstCheck"] as bool? ?? false;
        private Agreement LatestAgreement => Extra["agreement"] as Agreement;

        private List<AgreementOptionNormal> _requiredOptions;
        private List<AgreementOptionNormal> _optionalOptions;

        protected virtual void Awake()
        {
            _summaryGroup = transform.Find("ContentGroup/SummaryGroup").GetComponent<VerticalLayoutGroup>();
            _optionGroup = transform.Find("ContentGroup/OptionGroup").gameObject;

            _titleText = transform.Find("ContentGroup/TitleGroup/TitleText").GetComponent<Text>();
            _summaryText = _summaryGroup.transform.Find("SummaryBG/ScrollView/Viewport/SummaryText").GetComponent<HyperlinkText>();
            _promptText = transform.Find("ContentGroup/PromptText").GetComponent<HyperlinkText>();

            _disagreeButton = transform.Find("ButtonGroup/DisagreeButton").GetComponent<Button>();
            _agreeButton = transform.Find("ButtonGroup/AgreeButton").GetComponent<Button>();

            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            _agreeButton.transform.Find("Text").GetComponent<Text>().text = localizableString.AgreementAgree;
            _disagreeButton.transform.Find("Text").GetComponent<Text>().text = localizableString.AgreementDisagree;

            _uploadConfigButton = transform.Find("UploadConfigButton").GetComponent<Button>();
            _uploadConfigButton.onClick.AddListener(OnUploadConfigButtonClicked);

            _agreeButtonNormalSprite = _agreeButton.image.sprite;
        }

        protected virtual void Start()
        {
            _disagreeButton.onClick.AddListener(OnDisagreeButtonClicked);
            _agreeButton.onClick.AddListener(OnAgreeButtonClicked);

            if (LatestAgreement.AgreeAll != null && !string.IsNullOrEmpty(LatestAgreement.AgreeAll))
            {
                var allOptionGameObject = Instantiate(Resources.Load("Prefabs/AgreementOptionAll")) as GameObject;
                if (allOptionGameObject != null)
                {
                    allOptionGameObject.name = "AgreementOptionAll";
                    allOptionGameObject.transform.SetParent(_optionGroup.transform);
                    _allOption = allOptionGameObject.AddComponent<AgreementOptionAll>();
                    EnableRichText(_allOption.label,LatestAgreement.AgreeAll);
                    _allOption.AddListener(() =>
                    {
                        var newState = !IsRequiredAndOptionalOptionAgreed();
                        if (_requiredOptions != null && _requiredOptions.Count > 0)
                        {
                            foreach (var option in _requiredOptions)
                            {
                                option.toggle.isOn = newState;
                            }
                        }
                        if (_optionalOptions != null && _optionalOptions.Count > 0)
                        {
                            foreach (var option in _optionalOptions)
                            {
                                option.toggle.isOn = newState;
                            }
                        }
                        UpdateUIState();
                    });
                }
            }

            if (LatestAgreement.Options != null && LatestAgreement.Options.Count > 0)
            {
                _requiredOptions = new List<AgreementOptionNormal>();
                _optionalOptions = new List<AgreementOptionNormal>();
                foreach (var option in LatestAgreement.Options)
                {
                    if (string.IsNullOrEmpty(option.Title))
                    {
                        continue;
                    }

                    var optionGameObject = Instantiate(Resources.Load("Prefabs/AgreementOptionNormal")) as GameObject;
                    if (optionGameObject != null)
                    {
                        optionGameObject.name = "AgreementOptionNormal" + option.Title;
                        optionGameObject.transform.SetParent(_optionGroup.transform);
                        var agreementOption = optionGameObject.AddComponent<AgreementOptionNormal>();
                        agreementOption.optionName = option.Name;
                        EnableRichText(agreementOption.label, option.Title);
                        agreementOption.AddListener((isOn) =>
                        {
                            UpdateUIState();
                        });

                        if (option.Required)
                        {
                            _requiredOptions.Add(agreementOption);
                        }
                        else
                        {
                            _optionalOptions.Add(agreementOption);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(FirstCheck ? LatestAgreement.SummaryFirst : LatestAgreement.Summary))
            {
                _summaryGroup.gameObject.SetActive(false);
            }

            _titleText.text = FirstCheck ? LatestAgreement.TitleFirst : LatestAgreement.Title;
            var summaryText = FirstCheck ? LatestAgreement.SummaryFirst : LatestAgreement.Summary;
            var promptText= FirstCheck ? LatestAgreement.PromptFirst : LatestAgreement.Prompt;
            if (!string.IsNullOrEmpty(LatestAgreement.Agree))
            {
                _agreeButton.transform.Find("Text").GetComponent<Text>().text = LatestAgreement.Agree;    
            }
            if (!string.IsNullOrEmpty(LatestAgreement.Disagree))
            {
                _disagreeButton.transform.Find("Text").GetComponent<Text>().text = LatestAgreement.Disagree;    
            }

            var generator = _summaryText.cachedTextGenerator;
            var settings = _summaryText.GetGenerationSettings(_summaryText.rectTransform.rect.size);
            float width = 700 - 24;
            float height = 0;
            if (string.IsNullOrEmpty(summaryText))
            {
                height = 0;
            }
            else
            {
                height = generator.GetPreferredHeight(summaryText, settings) / _summaryText.pixelsPerUnit + 24;
                if (height < 140)
                {
                    height = 140;
                }
            }
            var rectTransform = _summaryText.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);
            EnableRichText(_summaryText, summaryText);
            EnableRichText(_promptText, promptText);
            
            //  不知道为啥默认状态不在顶部，只能手动滚动到顶部
            var scrollView = transform.Find("ContentGroup/SummaryGroup/SummaryBG/ScrollView").GetComponent<ScrollRect>();
            scrollView.verticalNormalizedPosition = 1;
            
            UpdateUIState();
        }
        
        private void EnableRichText(HyperlinkText textView, string content)
        {
            if (textView == null || string.IsNullOrEmpty(content))
            {
                return;
            }
            textView.text = content.Replace("<font color=", "<color=").Replace("</font>", "</color>");
            textView.colorStartStr = "<color=#15C5CE>";
            textView.colorEndStr = "</color>";
            UnityAction<string> onHrefClick = (url) =>
            {
                if (string.IsNullOrEmpty(url)) return;
                // 去除url 前后多余的引号
                url = url.Trim('\"');
                url = url.Trim('\'');
                Application.OpenURL(url);
            };
            textView.onHrefClick.AddListener(onHrefClick);
        }

        private void OnDestroy()
        {
        }

        private void OnDisagreeButtonClicked()
        {
            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            Dictionary<string, object> config = new Dictionary<string, object> {
                { DisagreeConfirmAlert.CONTENT_KEY, localizableString.AgreementDisagreeConfirmContent },
                { DisagreeConfirmAlert.SECONDARY_TEXT_KEY, localizableString.AgreementDisagreeConfirmBack },
                { DisagreeConfirmAlert.PRIMARY_TEXT_KEY, localizableString.AgreementDisagreeConfirmExit }
            };
            XD.SDK.Common.PC.Internal.UIManager.ShowUI<DisagreeConfirmAlert>("DisagreeConfirmAlert", config,
                (code, data) =>
                {
                    if (code == 0)
                    {
                        XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                    }
                    else if (code == 1)
                    {
                        OnCallback(1, null);
                    }
                });
        }

        private void OnAgreeButtonClicked()
        {
            if (!IsRequiredOptionAgreed())
            {
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
                            if (_requiredOptions != null && _requiredOptions.Count > 0)
                            {
                                foreach (var option in _requiredOptions)
                                {
                                    option.toggle.isOn = true;
                                }
                            }
                            if (_optionalOptions != null && _optionalOptions.Count > 0)
                            {
                                foreach (var option in _optionalOptions)
                                {
                                    option.toggle.isOn = true;
                                }
                            }
                            OnAgreeButtonClicked();
                        }
                        else if (code == 1)
                        {
                            XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                        }
                    }); 
                return;
            }

            var objects = new Dictionary<string, object>();
            
            var dayPush = false;
            var nightPush = false;
            
            ProcessOptions(_requiredOptions, objects, ref dayPush, ref nightPush);
            ProcessOptions(_optionalOptions, objects, ref dayPush, ref nightPush);
            
            OnCallback(0, objects);
        }
        
        private void ProcessOptions(List<AgreementOptionNormal> options, Dictionary<string, object> objects, ref bool dayPush, ref bool nightPush)
        {
            if (options == null || options.Count == 0)
            {
                return;
            }
            foreach (var option in options)
            {
                objects.Add(option.optionName, option.toggle.isOn);
                switch (option.optionName)
                {
                    case "dayPush":
                        dayPush = option.toggle.isOn;
                        break;
                    case "dightPush":
                        nightPush = option.toggle.isOn;
                        break;
                }
            }
        }

        private bool IsRequiredOptionAgreed()
        {
            if (_requiredOptions != null && _requiredOptions.Count > 0)
            {
                foreach (var option in _requiredOptions)
                {
                    if (!option.toggle.isOn)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsRequiredAndOptionalOptionAgreed()
        {
            if (IsRequiredOptionAgreed())
            {
                if (_optionalOptions != null && _optionalOptions.Count > 0)
                {
                    foreach (var option in _optionalOptions)
                    {
                        if (!option.toggle.isOn)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        private void UpdateUIState()
        {
            var sprite = Resources.Load<Sprite>("Textures/checkbox_square_empty");
            if (IsRequiredAndOptionalOptionAgreed())
            {
                sprite = Resources.Load<Sprite>("Textures/checkbox_square_full");
            }
            else if (IsRequiredOptionAgreed())
            {
                sprite = Resources.Load<Sprite>("Textures/checkbox_square_half");
            }

            if (_allOption != null && _allOption.toggleButton != null && _allOption.toggleButton.image != null)
            {
                _allOption.toggleButton.image.sprite = sprite;    
            }
            
            _agreeButton.image.sprite = IsRequiredOptionAgreed() ? _agreeButtonNormalSprite : _agreeButton.spriteState.disabledSprite;
            _agreeButton.transition = IsRequiredAndOptionalOptionAgreed() ? Selectable.Transition.SpriteSwap : Selectable.Transition.None;
        }

        private void OnUploadConfigButtonClicked()
        {
            _uploadConfigButtonClickedCount++;
            if (_uploadConfigButtonClickedCount >= UploadConfigButtonClickTriggerCount)
            {
                UploadConfigModule.TriggerUploadConfig();
            }
        }
    }
}
#endif