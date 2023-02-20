using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XD.SDK.Common.PC.Internal {
    public class PhoneNumberInputAlert : UIElement {
        public class Data {
            public string AreaCode { get; set; }
            public string PhoneNumber { get; set; }
        }

        public const int NEXT_CODE = 0;
        public const int CLOSE_CODE = -1;

        private static readonly string PHONE_NUMBER_REGEX = @"^1[0-9]{10}$";

        private Button closeButton;
        private Text titleText;
        private Image inputBGImage;
        private InputField inputNumberInput;
        private Text errorNumberTipsText;
        private Button nextButton;
        // 输入手机号
        public static Task Input(Func<string, Task<RequestVerifyCodeData>> requestVerifyCodeFunc, Func<string, Task> verifyFunc) {
            TaskCompletionSource<Data> tcs = new TaskCompletionSource<Data>();
            UIManager.ShowUI<PhoneNumberInputAlert>("XDPhoneNumberInputAlert", null, async (code, data) => {
                if (code == CLOSE_CODE) {
                    UIManager.Dismiss();
                    tcs.TrySetCanceled();
                } else if (code == NEXT_CODE) {
                    Data input = data as Data;
                    await VerifyCodeInputAlert.Verify(input.PhoneNumber, requestVerifyCodeFunc, verifyFunc);
                }
            });
            return tcs.Task;
        }

        private void Awake() {
            closeButton = transform.Find("Alert/CloseButton").GetComponent<Button>();
            titleText = transform.Find("Alert/TitleText").GetComponent<Text>();
            inputBGImage = transform.Find("Alert/Input").GetComponent<Image>();
            inputNumberInput = inputBGImage.transform.Find("NumberInput").GetComponent<InputField>();
            errorNumberTipsText = transform.Find("Alert/ErrorNumberTips").GetComponent<Text>();
            nextButton = transform.Find("Alert/NextButton").GetComponent<Button>();

            errorNumberTipsText.gameObject.SetActive(false);

            closeButton.onClick.AddListener(OnCloseClicked);
            nextButton.onClick.AddListener(OnNextClicked);
        }

        private void Start() {
            inputNumberInput.Select();
            AliyunTrack.LoginOpenMobilePanel();
        }

        public bool CheckPhoneNumberValid(string phoneNumber) {
            // 正则检查
            bool valid = Regex.IsMatch(phoneNumber, PHONE_NUMBER_REGEX);
            if (valid) {
                inputBGImage.color = new Color(217.0f / 255, 217.0f / 255, 217.0f / 255);
            } else {
                inputBGImage.color = new Color(246.0f / 255, 76.0f / 255, 76.0f / 255);
            }
            errorNumberTipsText.gameObject.SetActive(!valid);
            return valid;
        }

        #region UI Events

        private void OnCloseClicked()
        {
            AliyunTrack.LoginMobileInputBtnClick(3);
            OnCallback(CLOSE_CODE, null);
        }

        private void OnNextClicked() {
            string areaCode = "86";
            string phoneNumber = inputNumberInput.text;
            
            if (!CheckPhoneNumberValid(phoneNumber)) {
                return;
            }

            AliyunTrack.MobileNumberString = phoneNumber;
            AliyunTrack.LoginMobileInputBtnClick(1);
            
            OnCallback(NEXT_CODE, new Data {
                AreaCode = areaCode,
                PhoneNumber = phoneNumber
            });
        }

        #endregion
    }
}
