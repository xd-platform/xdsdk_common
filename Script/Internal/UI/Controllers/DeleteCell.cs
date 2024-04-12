#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XD.SDK.Account;
using XD.SDK.Common.PC;

namespace XD.SDK.Common.PC.Internal {
    public class DeleteCell : UIElement {
        public Button deleteBt;

        private void Awake() {
            deleteBt = GameObject.Find("DeleteBt").GetComponent<Button>();
            deleteBt.onClick.AddListener(DeleteTaped);
        }

        public void SetDeleteText(string text) {
            deleteBt.transform.Find("Text").GetComponent<Text>().text = text;
        }

        public void DeleteTaped() {
            var dic = new Dictionary<string, object>() {
                {"loginType", (int)XD.SDK.Account.LoginType.Guest},
                {"alertType", (int)DeleteAlertType.DeleteGuest}, 
            };
            XD.SDK.Common.PC.Internal.UIManager.ShowUI<DeleteAccountAlert>(dic, (code, msg) => { OnCallback(XD.SDK.Common.PC.Internal.UIManager.RESULT_SUCCESS, "删除账号"); });
        }
    }
}
#endif