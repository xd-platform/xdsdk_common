#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    public class AccountCell : MonoBehaviour {
        public class AccountCellModel {
            public string LoginName { get; set; }
            public XD.SDK.Account.LoginType LoginType { get; set; }
            public BindType BindType { get; set; }
            public Dictionary<string, object> Extras { get; set; }
        }

        private Image iconImage;
        private Text nameText;
        private Text bindFlagText;
        private Button bindBt;

        public int cellIndex = 0;

        internal AccountCellModel model;

        internal Action<int> OnBind;

        public void SetModel(AccountCellModel model) {
            this.model = model;
        }

        public void RefreshState(AccountCellModel model) {
            this.model = model;
            Start();
        }

        private void Awake() {
            iconImage = transform.Find("IconImage").GetComponent<Image>();
            nameText = transform.Find("AccountContainer/NameText").GetComponent<Text>();
            bindFlagText = transform.Find("AccountContainer/BindFlagText").GetComponent<Text>();
            bindFlagText.gameObject.SetActive(false);
            bindBt = transform.Find("BindButton").GetComponent<Button>();
            bindBt.onClick.AddListener(BindButtonTap);
        }

        void Start() {
            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            bindFlagText.text = localizableString.Bound;
            if (model != null) {
                if (model.LoginType == LoginType.Phone) {
                    // TODO 翻译
                    if (model.Extras != null && model.Extras.TryGetValue("openId", out object openId)) {
                        nameText.text = $"手机号码 ({model.Extras["openId"]})";
                    } else {
                        nameText.text = "手机号码";
                    }
                } else {
                    nameText.text = localizableString.AccountFormat.Replace("%s", model.LoginName);
                }
                string iconAssets;
                switch (model.LoginType) {
                    case XD.SDK.Account.LoginType.TapTap:
                        iconAssets = "Images/type_icon_tap";
                        break;
                    case XD.SDK.Account.LoginType.Google:
                        iconAssets = "Images/type_icon_google";
                        break;
                    case XD.SDK.Account.LoginType.Apple:
                        iconAssets = "Images/type_icon_apple";
                        break;
                    case XD.SDK.Account.LoginType.Steam:
                        iconAssets = "Images/type_icon_steam";
                        break;
                    case LoginType.Phone:
                        iconAssets = "Images/type_icon_phone";
                        break;
                    default:
                        iconAssets = "Images/type_icon_tap";
                        break;
                }
                iconImage.sprite = Resources.Load(iconAssets, typeof(Sprite)) as Sprite;

                if (model.BindType == BindType.Bind) {
                    bindBt.transform.Find("Text").GetComponent<Text>().text = localizableString.Unbind;
                    bindBt.transform.Find("Text").GetComponent<Text>().color = new Color(0.1333f, 0.1333f, 0.1333f);
                    bindFlagText.gameObject.SetActive(true);
                } else if (model.BindType == BindType.UnBind) {
                    bindBt.transform.Find("Text").GetComponent<Text>().text = localizableString.Bind;
                    bindBt.transform.Find("Text").GetComponent<Text>().color = new Color(0.0823f, 0.7725f, 0.8078f);
                    bindFlagText.gameObject.SetActive(false);
                }

                foreach (var netMd in ConfigModule.BindEntryConfigs) {
                    if (model.LoginName.ToLower() == netMd.EntryName.ToLower()) {
                        bool show = model.BindType != BindType.None;
                        bindBt.gameObject.SetActive(show);
                        break;
                    }
                }
            }
        }

        public void BindButtonTap() {
            OnBind?.Invoke(cellIndex);
        }
    }
}
#endif