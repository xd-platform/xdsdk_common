#if UNITY_EDITOR || UNITY_STANDALONE
using TapTap.Common;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class PayHintAlert : UIElement {
        private void Start() {
            Text titleText = transform.Find("TitleText").GetComponent<Text>();
            Text subTitleText = transform.Find("SubTitle").GetComponent<Text>();
            Text contentText = transform.Find("Panel/Container/MsgText").GetComponent<Text>();
            HyperlinkText bottomText = transform.Find("BottomText").GetComponent<HyperlinkText>();

            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();

            titleText.text = localizableString.RefundLoginRestrictTitle;
            subTitleText.text = localizableString.RefundLoginRestrictSubtitle;
            bottomText.text =
                $"<color=#222222>{localizableString.RefundCustomServiceTip}</color><color=#15C5CE><a href=custom>{localizableString.RefundContactCustomService}</a></color>";
            bottomText.OnClicked = (data) => {
                XDGCommon.Report("", "", "");
            };

            var hasIOS = SafeDictionary.GetValue<bool>(extra, "hasIOS");
            var hasAndroid = SafeDictionary.GetValue<bool>(extra, "hasAndroid");
            if (hasIOS && hasAndroid) {
                contentText.text = localizableString.RefundAllPayTip;
            } else if (hasIOS) {
                contentText.text = localizableString.RefundIOSPayTip;
            } else {
                contentText.text = localizableString.RefundAndroidPayTip;
            }
        }
    }
}
#endif