#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;
using Vuplex.WebView.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class CancelAccountAlert : UIElement {
        internal const int CANCEL_DONE_CODE = 0;

        static readonly string[] ALLOWN_KEYS = new string[] {
            "Tab", "ArrowUp", "ArrowDown", "ArrowRight", "ArrowLeft", "Escape", "Delete", "Home", "End", "Insert", "PageUp", "PageDown", "Help", "Backspace"
        };

        CanvasWebViewPrefab canvasWebViewPrefab;
        NativeKeyboardListener nativeKeyboardListener;

        Text titleText;
        Button closeButton;

        GameObject loadFailedGO;
        Text loadFailedTipsText;
        Button loadFaieldRetryButton;

        private bool webInputFocused;

        private bool hasCancelled;

        private void Awake() {
            Input.imeCompositionMode = IMECompositionMode.On;

            titleText = transform.Find("Header/Title").GetComponent<Text>();

            GameObject webviewGO = Instantiate(Resources.Load("CanvasWebViewPrefab")) as GameObject;
            canvasWebViewPrefab = webviewGO.GetComponent<CanvasWebViewPrefab>();
            canvasWebViewPrefab.Resolution = 1;

            webviewGO.transform.SetParent(transform.Find("WebViewContainer"));
            RectTransform rectTransform = webviewGO.GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            canvasWebViewPrefab = webviewGO.GetComponent<CanvasWebViewPrefab>();

            closeButton = GameObject.Find("Header/CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(OnCloseButtonClicked);

            loadFailedGO = GameObject.Find("LoadFailedView");
            loadFailedTipsText = loadFailedGO.transform.Find("TipsText").GetComponent<Text>();
            loadFaieldRetryButton = loadFailedGO.transform.Find("RetryButton").GetComponent<Button>();
            loadFaieldRetryButton.onClick.AddListener(OnRetryButtonClicked);

            LocalizableString localizableString = Localization.GetCurrentLocalizableString();
            loadFailedTipsText.text = localizableString.LoadError;

            Text retryText = loadFaieldRetryButton.transform.Find("Container/Text").GetComponent<Text>();
            retryText.text = localizableString.Retry;

            loadFailedGO.SetActive(false);
        }

        private async void Start() {
            await canvasWebViewPrefab.WaitUntilInitialized();

            canvasWebViewPrefab.WebView.FocusedInputFieldChanged += OnFocusedInputFieldChanged;
            canvasWebViewPrefab.WebView.UrlChanged += OnUrlChanged;
            canvasWebViewPrefab.WebView.PageLoadFailed += OnPageLoadFailed;
            canvasWebViewPrefab.WebView.TitleChanged += OnTitleChanged;

            canvasWebViewPrefab.KeyboardEnabled = false;

            nativeKeyboardListener = NativeKeyboardListener.Instantiate();
            nativeKeyboardListener.KeyDownReceived += OnKeyDownReceived;
            nativeKeyboardListener.KeyUpReceived += OnKeyUpReceived;

            LoadCancelAccountPage();
        }

        private void Update() {
            if (!webInputFocused) {
                return;
            }

            Event curEvt = new Event();
            while (Event.PopEvent(curEvt)) {
                char ch = curEvt.character;
                if (ch == '\0') {
                    continue;
                }
                canvasWebViewPrefab.WebView.SendKey(ch.ToString());
            }
        }

        private void OnDestroy() {
            nativeKeyboardListener.KeyDownReceived -= OnKeyDownReceived;
            nativeKeyboardListener.KeyUpReceived -= OnKeyUpReceived;

            if (nativeKeyboardListener.gameObject) {
                Destroy(nativeKeyboardListener?.gameObject);
            }

            canvasWebViewPrefab.WebView.FocusedInputFieldChanged -= OnFocusedInputFieldChanged;
            canvasWebViewPrefab.WebView.UrlChanged -= OnUrlChanged;
            canvasWebViewPrefab.WebView.PageLoadFailed -= OnPageLoadFailed;
            canvasWebViewPrefab.WebView.TitleChanged -= OnTitleChanged;
        }

        void OnTitleChanged(object sender, EventArgs<string> args) {
            titleText.text = args.Value;
        }

        void OnUrlChanged(object sender, UrlChangedEventArgs eventArgs) {
            string url = eventArgs.Url;
            Uri uri = new Uri(url);
            string path = uri.AbsolutePath;
            if (!string.IsNullOrEmpty(path) && path.Contains("success")) {
                // 注销完成
                XDLogger.Debug("注销成功");
                hasCancelled = true;
            }
        }

        void OnPageLoadFailed(object sender, EventArgs eventArgs) {
            ShowError(Localization.GetCurrentLocalizableString().LoadError);
        }

        void OnCloseButtonClicked() {
            UIManager.Dismiss();
            if (hasCancelled) {
                OnCallback(CANCEL_DONE_CODE, null);
            }
        }

        void OnRetryButtonClicked() {
            loadFailedGO.SetActive(false);
            LoadCancelAccountPage();
        }

        void LoadCancelAccountPage() {
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                // 没网
                ShowError(Localization.GetCurrentLocalizableString().NetError);
            } else {
                string url = Extra["url"] as string;
                canvasWebViewPrefab.WebView.LoadUrl(url);
            }
        }

        void ShowError(string message) {
            loadFailedTipsText.text = message;
            loadFailedGO.SetActive(true);
        }

        void OnKeyDownReceived(object sender, KeyboardEventArgs eventArgs) {
            if (!IsAllownKey(eventArgs)) {
                return;
            }

            IWithKeyDownAndUp webViewWithKeyDown = canvasWebViewPrefab.WebView as IWithKeyDownAndUp;
            webViewWithKeyDown?.KeyDown(eventArgs.Key, eventArgs.Modifiers);
        }

        void OnKeyUpReceived(object sender, KeyboardEventArgs eventArgs) {
            if (!IsAllownKey(eventArgs)) {
                return;
            }
            IWithKeyDownAndUp webViewWithKeyUp = canvasWebViewPrefab.WebView as IWithKeyDownAndUp;
            webViewWithKeyUp?.KeyUp(eventArgs.Key, eventArgs.Modifiers);
        }

        void OnFocusedInputFieldChanged(object sender, FocusedInputFieldChangedEventArgs args) {
            FocusedInputFieldType focusedType = args.Type;
            webInputFocused = focusedType == FocusedInputFieldType.Text;
        }

        static bool IsAllownKey(KeyboardEventArgs eventArgs) {
            string key = eventArgs.Key;
            KeyModifier modifier = eventArgs.Modifiers;
            if (modifier == KeyModifier.None) {
                foreach (string allownKey in ALLOWN_KEYS) {
                    if (allownKey.Equals(key)) {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }
    }
}
#endif
