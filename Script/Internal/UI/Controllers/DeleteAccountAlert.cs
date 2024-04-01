#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;
using TapTap.Common;
using XD.SDK.Account;

namespace XD.SDK.Common.PC.Internal {
    public class DeleteAccountAlert : UIElement {
        public GameObject pannelOne;
        public Text titleOne;
        public Text msgOne;
        public Button cancelOne;
        public Button sureOne;

        public GameObject pannelTwo;
        public InputField fieldTwo;
        public Text hintTxt;
        public Text titleTwo;
        public Text msgTwo;
        public Button cancelTwo;
        public Button sureTwo;

        private XD.SDK.Account.LoginType loginType = XD.SDK.Account.LoginType.Guest;
        private DeleteAlertType alertType = DeleteAlertType.DeleteGuest;
        private LocalizableString langMd;
        private bool inputError = false;

        private void Awake() {
            pannelOne = transform.Find("PanelOne").gameObject;
            titleOne = transform.Find("PanelOne/TitleText").GetComponent<Text>();
            msgOne = transform.Find("PanelOne/MsgText").GetComponent<Text>();
            cancelOne = transform.Find("PanelOne/ActionContainer/CancelButton").GetComponent<Button>();
            sureOne = transform.Find("PanelOne/ActionContainer/SureButton").GetComponent<Button>();

            pannelTwo = transform.Find("PanelTwo").gameObject;
            titleTwo = transform.Find("PanelTwo/TitleText").GetComponent<Text>();
            msgTwo = transform.Find("PanelTwo/MsgText").GetComponent<Text>();
            fieldTwo = transform.Find("PanelTwo/InputField").GetComponent<InputField>();
            hintTxt = transform.Find("PanelTwo/HintText").GetComponent<Text>();
            cancelTwo = transform.Find("PanelTwo/ActionContainer/CancelButton").GetComponent<Button>();
            sureTwo = transform.Find("PanelTwo/ActionContainer/SureButton").GetComponent<Button>();

            cancelOne.onClick.AddListener(CancelOneTap);
            sureOne.onClick.AddListener(SureOneTap);
            cancelTwo.onClick.AddListener(CancelTwoTap);
            sureTwo.onClick.AddListener(SureTwoTap);

            Button closeButton = transform.Find("PanelOne/CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CancelOneTap);
            Button closeButton2 = transform.Find("PanelTwo/CloseButton").GetComponent<Button>();
            closeButton2.onClick.AddListener(CancelTwoTap);

            pannelTwo.SetActive(false);
        }

        void Start() {
            langMd = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            loginType = SafeDictionary.GetValue<XD.SDK.Account.LoginType>(extra, "loginType");
            alertType = (DeleteAlertType) SafeDictionary.GetValue<int>(extra, "alertType");
            
            if (alertType == DeleteAlertType.DeleteGuest) { //删除游客
                titleOne.text = langMd.DeleteAccountTitle;
                msgOne.text = langMd.DeleteContent;
                cancelOne.transform.Find("Text").GetComponent<Text>().text = langMd.Cancel;
                sureOne.transform.Find("Text").GetComponent<Text>().text = langMd.DeleteAccountSure;

                //two
                titleTwo.text = langMd.DeleteAccount;
                msgTwo.text = langMd.DeleteConfirmContent;
                cancelTwo.transform.Find("Text").GetComponent<Text>().text = langMd.Cancel;
                sureTwo.transform.Find("Text").GetComponent<Text>().text = langMd.DeleteAccount;
                
            } else if (alertType == DeleteAlertType.Unbindthird) { //解绑第三方
                titleOne.text = langMd.UnbindAccountTitle;
                msgOne.text = langMd.UnbindContent.Replace("%s", LoginTypeModel.GetReadableName(loginType));
                cancelOne.transform.Find("Text").GetComponent<Text>().text = langMd.Cancel;
                sureOne.transform.Find("Text").GetComponent<Text>().text = langMd.UnbindAccount;

                //two
                titleTwo.text = langMd.UnbindAccount;
                msgTwo.text = langMd.UnbindConfirmContent;
                cancelTwo.transform.Find("Text").GetComponent<Text>().text = langMd.Cancel;
                sureTwo.transform.Find("Text").GetComponent<Text>().text = langMd.UnbindAccountButton;
                
            } else { //删除第三方
                titleOne.text = langMd.DeleteAccountTitle;
                msgOne.text = langMd.UnbindDeleteContent.Replace("%s", new LoginTypeModel(loginType).TypeName);
                cancelOne.transform.Find("Text").GetComponent<Text>().text = langMd.Cancel;
                sureOne.transform.Find("Text").GetComponent<Text>().text = langMd.DeleteAccountSure;

                //two
                titleTwo.text = langMd.DeleteAccount;
                msgTwo.text = langMd.DeleteConfirmContent;
                cancelTwo.transform.Find("Text").GetComponent<Text>().text = langMd.Cancel;
                sureTwo.transform.Find("Text").GetComponent<Text>().text = langMd.DeleteAccount;
                
            }

            fieldTwo.onValueChanged.AddListener((param) => { OnInputFieldChange(param); });
            Invoke("updatePosition", 0.1f);
        }

        private void UpdatePosition() {
            var x = cancelOne.transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            sureOne.transform.localPosition = new Vector3(-x, -60f, 0);
            
            var x2 = 10 + sureTwo.transform.GetComponent<RectTransform>().sizeDelta.x / 2;
            cancelTwo.transform.localPosition = new Vector3(-x2, -60f, 0);
        }

        private void OnInputFieldChange(string txt) {
            if (inputError){
                hintTxt.text = "";
                fieldTwo.GetComponent<Image>().sprite = Resources.Load("Images/delete_account_input_bg", typeof(Sprite)) as Sprite;
            }

            inputError = false;
        }

        public void CancelOneTap() {
            XD.SDK.Common.PC.Internal.UIManager.Dismiss();
        }

        public void SureOneTap() {
            pannelOne.SetActive(false);
            pannelTwo.SetActive(true);
        }

        public void CancelTwoTap() {
            XD.SDK.Common.PC.Internal.UIManager.Dismiss();
        }

        public void SureTwoTap() {
            var str = fieldTwo.text;
            if (alertType == DeleteAlertType.DeleteGuest || alertType == DeleteAlertType.DeleteThird) { //删除游客或第三方
                if (!"Delete".Equals(str)) {
                    hintTxt.text = langMd.InputError;
                    fieldTwo.GetComponent<Image>().sprite =
                        Resources.Load("Images/delete_account_input_red_bg", typeof(Sprite)) as Sprite;
                    inputError = true;
                } else {
                    OnCallback(XD.SDK.Common.PC.Internal.UIManager.RESULT_SUCCESS, "确认删除或解绑");
                    XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                }
            } else {
                if (!"Confirm".Equals(str)) {
                    inputError = true;
                    fieldTwo.GetComponent<Image>().sprite =
                        Resources.Load("Images/delete_account_input_red_bg", typeof(Sprite)) as Sprite;
                    hintTxt.text = langMd.InputError;
                } else {
                    OnCallback(XD.SDK.Common.PC.Internal.UIManager.RESULT_SUCCESS, "确认删除或解绑");
                    XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                }
            }
        }
    }

    public enum DeleteAlertType { 
        DeleteGuest = 0,   //删除游客
        DeleteThird = 1,   //删除第三方
        Unbindthird = 2,   //解绑第三方
    }
}
#endif