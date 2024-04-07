#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;

namespace XD.SDK.Common.PC.Internal {
    public class PaymentAlert : UIElement {
        CanvasWebViewPrefab canvasWebViewPrefab;
        Button closeButton;

        GameObject loadFailedGO;
        Text loadFailedTipsText;
        Button loadFaieldRetryButton;

        private void Awake() {
            GameObject webviewGO = Instantiate(Resources.Load("CanvasWebViewPrefab")) as GameObject;
            canvasWebViewPrefab = webviewGO.GetComponent<CanvasWebViewPrefab>();
            canvasWebViewPrefab.Resolution = 2;

            webviewGO.transform.SetParent(transform.Find("WebViewContainer"));
            RectTransform rectTransform = webviewGO.GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            canvasWebViewPrefab = webviewGO.GetComponent<CanvasWebViewPrefab>();

            closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(OnCloseButtonClicked);

            loadFailedGO = GameObject.Find("PayLoadFailedView");
            loadFailedTipsText = loadFailedGO.transform.Find("TipsText").GetComponent<Text>();
            loadFaieldRetryButton = loadFailedGO.transform.Find("RetryButton").GetComponent<Button>();
            loadFaieldRetryButton.onClick.AddListener(OnRetryButtonClicked);

            Text retryText = loadFaieldRetryButton.transform.Find("Container/Text").GetComponent<Text>();
            retryText.text = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString().Retry;

            loadFailedGO.SetActive(false);
        }

        private async void Start() {
            await canvasWebViewPrefab.WaitUntilInitialized();

            canvasWebViewPrefab.WebView.UrlChanged += OnUrlChanged;
            canvasWebViewPrefab.WebView.PageLoadFailed += OnPageLoadFailed;

            LoadPaymentPage();
        }

        void LoadPaymentPage() {
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                // 没网
                ShowError(XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString().NetError);
            } else {
                string url = Extra["url"] as string;
                canvasWebViewPrefab.WebView.LoadUrl(url);
            }
        }

        private void OnDestroy() {
            canvasWebViewPrefab.WebView.UrlChanged -= OnUrlChanged;
            canvasWebViewPrefab.WebView.PageLoadFailed -= OnPageLoadFailed;
        }

        void OnUrlChanged(object sender, UrlChangedEventArgs eventArgs) {
            string url = eventArgs.Url;
            Uri uri = new Uri(url);
            string fragment = uri.Fragment;
            if (!string.IsNullOrEmpty(fragment)) {
                // 只有返回 fragment 才认为是完成
                XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                OnCallback(PayModule.PAY_DONE, fragment);
            }
        }

        void OnPageLoadFailed(object sender, EventArgs eventArgs) {
            ShowError(XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString().LoadError);
        }

        void OnCloseButtonClicked() {
            XD.SDK.Common.PC.Internal.UIManager.Dismiss();
            OnCallback(PayModule.PAY_CANCEL, null);
        }

        void OnRetryButtonClicked() {
            loadFailedGO.SetActive(false);
            LoadPaymentPage();
        }

        void ShowError(string message) {
            loadFailedTipsText.text = message;
            loadFailedGO.SetActive(true);
        }
    }
}
#endif
