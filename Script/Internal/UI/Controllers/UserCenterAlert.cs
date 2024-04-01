#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XD.SDK.Account;
using XD.SDK.Account.PC.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class UserCenterAlert : UIElement {
        private static readonly int MIN_BIND_COUNT = 1;
        private static readonly float TIMEOUT = 0.25f;

        private GameObject content;
        private Text bindTitleTxt;
        private Text titleTxt;
        private Text infoTitleTxt;
        private Text idTxt;
        private GameObject errorView;

        private LocalizableString localizableString;
        private XDUser user;

        public GameObject bindsContent;
        private static readonly float cellHeight = 36.0f;

        private List<AccountCell> cellList;
        private float _currentTime = 0;
        
        //绑定解绑回调
        public static Action<XD.SDK.Account.LoginType> bindCallback;
        public static Action<XDException> bindErrorCallback;
        public static Action<XD.SDK.Account.LoginType> unBindCallback;
        public static Action<XDException> unBindErrorCallback;

        private void Awake() {
            titleTxt = transform.Find("Header/TitleTxt").GetComponent<Text>();

            content = transform.Find("Content/Viewport/Content").gameObject;
            infoTitleTxt = transform.Find("Content/Viewport/Content/UserPanel/AccountContainer/InfotTitleTxt").GetComponent<Text>();
            idTxt = transform.Find("Content/Viewport/Content/UserPanel/IdContainer/IdText").GetComponent<Text>();
            errorView = transform.Find("LoadFailedView").gameObject;
            bindTitleTxt = transform.Find("Content/Viewport/Content/BindTitleContainer/BindTitleText").GetComponent<Text>();
            bindsContent = transform.Find("Content/Viewport/Content/BindsContent").gameObject;

            Button errorRetryButton = errorView.transform.Find("RetryButton").GetComponent<Button>();
            errorRetryButton.onClick.AddListener(ErrorViewTap);

            errorView.SetActive(false);

            Button copyBtn = transform.Find("Content/Viewport/Content/UserPanel/IdContainer/CopyBt").GetComponent<Button>();
            copyBtn.onClick.AddListener(CopyTaped);
            Button closeBtn = transform.Find("Header/CloseBt").GetComponent<Button>();
            closeBtn.onClick.AddListener(CloseTaped);

            Button logoutBtn = transform.Find("Content/Viewport/Content/LogoutButton").GetComponent<Button>();
            logoutBtn.onClick.AddListener(OnLogoutClicked);

            localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();

            Text logoutBtnText = logoutBtn.transform.Find("Text").GetComponent<Text>();
            logoutBtnText.text = localizableString.Logout2;
        }

        async void Start(){
            user = await UserModule.GetLocalUser() as XDUser;
            localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            titleTxt.text = GetTitle();
            bindTitleTxt.text = localizableString.BoundAccounts;

            infoTitleTxt.text = GetAccountInfo();
            idTxt.text = $"ID: {user.userId}";
            errorView.GetComponentInChildren<Text>().text = localizableString.NetworkErrorRetry;

            // TODO 延迟布局
            await Task.Delay(100);

            List<LoginTypeModel> supportTypes = GetSupportTypes();

            cellList = new List<AccountCell>(supportTypes.Count);

            for (int i = 0; i < supportTypes.Count; i++) {
                LoginTypeModel st = supportTypes[i];

                AccountCell.AccountCellModel model = new AccountCell.AccountCellModel {
                    LoginName = st.TypeName,
                    LoginType = st.Type,
                    BindType = BindType.None
                };

                GameObject gameObj = Instantiate(Resources.Load("Prefabs/AccountCell")) as GameObject;
                gameObj.name = "AccountCell" + i;
                gameObj.transform.SetParent(bindsContent.transform);
                gameObj.transform.localPosition = new Vector3(350, -cellHeight / 2 - (cellHeight * i), 0);
                gameObj.transform.localScale = Vector3.one;

                AccountCell cell = gameObj.AddComponent<AccountCell>();
                cell.cellIndex = i;
                cell.SetModel(model);
                cell.OnBind = cellIndex => {
                    if (model.BindType == BindType.Bind) {
                        // 限制最后一个不允许解绑
                        int bindCount = cellList.Where(c => c.model.BindType == BindType.Bind)
                            .Count();
                        if (bindCount <= MIN_BIND_COUNT) {
                            UIManager.ShowToast(localizableString.UnbindLastAccountTips);
                            return;
                        }

                        //开始解绑
                        var dic = new Dictionary<string, object>(){
                            {"loginType", model.LoginType},
                            {"alertType", (int)DeleteAlertType.Unbindthird},
                        };
                        XD.SDK.Common.PC.Internal.UIManager.ShowUI<DeleteAccountAlert>(dic,
                            (code, data) => {
                                UIManager.ShowLoading(true);
                                _currentTime = TIMEOUT;
                                Unbind(model.LoginType, cellIndex);
                            });
                    } else {
                        //开始绑定
                        UIManager.ShowLoading(true);
                        _currentTime = TIMEOUT;
                        Bind(model.LoginType, cellIndex);
                    }
                };

                cellList.Add(cell);
            }

            _ = RequestBinds();
        }

        private void OnDisable()
        {
            if (_currentTime > 0)
            {
                UIManager.DismissLoading();
                _currentTime = 0;
            }
        }

        private void Update() {
            titleTxt.text = content.transform.localPosition.y < 100 ?
                GetTitle() : GetAccountInfo();
            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime <= 0)
                {
                    UIManager.DismissLoading();
                    _currentTime = 0;
                }
            }
        }

        private string GetTitle() {
            return $"<b>{localizableString.AccountSafeInfo}</b>";
        }

        private string GetAccountInfo() {
            string account;
            XD.SDK.Account.LoginType type = user.getLoginType();
            if (type == LoginType.Guest) {
                account = localizableString.Guest;
            } else {
                account = LoginTypeModel.GetReadableName(type);
            }

            account = localizableString.AccountFormat.Replace("%s", account);

            return $"<b>{account}</b><size=12><color=#BFBFBF>（{localizableString.CurrentAccountPrefix}）</color></size>";
        }

        public void CloseTaped(){
            XD.SDK.Common.PC.Internal.UIManager.DismissAll();
        }

        public void CopyTaped(){
            GUIUtility.systemCopyBuffer = user.userId;
            XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.CopySuccess);
        }

        private async Task RequestBinds(){
            UIManager.ShowLoading();
            try {
                Dictionary<LoginType, Bind> binds = (await UserModule.GetBindList())
                    .ToDictionary(bind => (LoginType)bind.LoginType, bind => bind);

                for (int i = 0; i < cellList.Count; i++) {
                    AccountCell cell = cellList[i];
                    cell.model.BindType = binds.ContainsKey(cell.model.LoginType) ? BindType.Bind : BindType.UnBind;
                    Dictionary<string, object> extras = new Dictionary<string, object>();
                    if (binds.TryGetValue(cell.model.LoginType, out Bind bind)) {
                        if (!string.IsNullOrEmpty(bind.OpenId)) {
                            extras["openId"] = bind.OpenId;
                        }
                    }
                    cell.model.Extras = extras;
                    cell.RefreshState(cell.model);
                }
            } catch (Exception e) {
                XDLogger.Warn($"列表请求失败: {e}");
                errorView.SetActive(true);
            } finally {
                UIManager.DismissLoading();
            }
        }

        private async void Bind(XD.SDK.Account.LoginType loginType, int cellIndex) {
            XDLogger.Debug("绑定： " + loginType);
            try {
                await UserModule.Bind(loginType);
                await RequestBinds();

                XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.BindSuccess);

                XDGAccountPC.UserStatusChangeDelegate?.OnBind?.Invoke(loginType);
            } catch (XDException e) {
                ShowBindError(e);
            } catch (TaskCanceledException) {
                XDLogger.Debug("关闭绑定");
            } catch (Exception e) {
                XDLogger.Warn($"绑定失败: {e}");
                XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.BindError);
            }
        }

        private void ShowBindError(XDException error) {
            if (error.code == 80081){
                XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.LoginCancel);
            } else{
                if (string.IsNullOrEmpty(error.Message)){
                    XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.BindError);
                } else{
                    XD.SDK.Common.PC.Internal.UIManager.ShowToast(error.Message);
                }   
            }

            if (bindErrorCallback != null){
                bindErrorCallback(error);
            }
        }

        private async void Unbind(XD.SDK.Account.LoginType loginType, int cellIndex) {
            XDLogger.Debug("解绑： " + loginType);
            UIManager.ShowLoading();
            try {
                AccountCell cell = cellList[cellIndex];
                await UserModule.Unbind(loginType, cell.model.Extras);
                XDGAccountPC.UserStatusChangeDelegate?.OnUnbind?.Invoke(loginType);
                
                AccessToken accessToken = await AccessTokenModule.GetLocalAccessToken();
                if (loginType == user.getLoginType()) {
                    UIManager.ShowToast(localizableString.UnbindDeleteSuccessReturnSign);
                    UIManager.DismissAll();

                    await XDGAccountPC.InternalLogout();
                } else {
                    UIManager.ShowToast(localizableString.UnbindSuccess);
                    await RequestBinds();
                }

            } catch (XDException e) {
                XDLogger.Warn(e.ToString());
                if (string.IsNullOrEmpty(e.Message)) {
                    XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.UnbindError);
                } else {
                    XD.SDK.Common.PC.Internal.UIManager.ShowToast(e.Message);
                }
            } catch (TaskCanceledException) {
                XDLogger.Debug("关闭解绑");
            } catch (Exception e) {
                XDLogger.Warn($"解绑失败: {e}");
                XD.SDK.Common.PC.Internal.UIManager.ShowToast(localizableString.UnbindError);
            } finally {
                UIManager.DismissLoading();
            }
        }

        private List<LoginTypeModel> GetSupportTypes() {
            List<LoginTypeModel> list = new List<LoginTypeModel>();
            if (ConfigModule.BindEntries != null) {
                Dictionary<string, LoginTypeModel> sdkSupportedLoginTypes = SDKSupportedLoginTypes
                    .ToDictionary(item => item.TypeName.ToLower(), item => item);
                foreach (string bindType in ConfigModule.BindEntries) {
                    if (sdkSupportedLoginTypes.TryGetValue(bindType.ToLower(), out LoginTypeModel loginTypeModel)) {
                        list.Add(loginTypeModel);
                    }
                }
            }

            return list;
        }

        private static List<LoginTypeModel> SDKSupportedLoginTypes => new List<LoginTypeModel> {
            new LoginTypeModel(LoginType.Phone),
            new LoginTypeModel(LoginType.TapTap),
            new LoginTypeModel(LoginType.Google),
            new LoginTypeModel(LoginType.Apple),
            new LoginTypeModel(LoginType.Steam),
            new LoginTypeModel(LoginType.Facebook)
        };

        public void ErrorViewTap() {
            XDLogger.Debug("点击了 errorViewTap");
            errorView.SetActive(false);
            _ = RequestBinds();
        }

        private void OnLogoutClicked() {
            LocalizableString localizable = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            Dictionary<string, object> config = new Dictionary<string, object> {
                { Dialog.TITLE_KEY, localizable.ConfirmLogout },
                { Dialog.CONTENT_KEY, localizable.ConfirmLogoutCurrentAccount },
                { Dialog.SECONDARY_TEXT_KEY, localizable.Confirm2 },
                { Dialog.PRIMARY_TEXT_KEY, localizable.Cancel2 }
            };
            XD.SDK.Common.PC.Internal.UIManager.ShowUI<Dialog>("Dialog", config, async (code, data) => {
                UIManager.ShowLoading(true);
                _currentTime = TIMEOUT;
                if (code == 1) {
                    XD.SDK.Common.PC.Internal.UIManager.DismissAll();
                    await XDGAccountPC.InternalLogout();
                } else {
                    XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                }
            });
        }
    }
}
#endif