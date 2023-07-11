#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;

namespace XD.SDK.Common.PC.Internal {
    public class GeneralAgreementAlert : UIElement {
        private const float OPEN_URL_TIMEOUT = 0.25f;
        protected CanvasWebViewPrefab canvasWebViewPrefab;
        protected Button disagreeButton;
        protected Button agreeButton;
        protected GameObject netErrorGO;
        protected GameObject loadingGO;

        protected Sprite agreeButtonNormalSprite;

        string URL => Extra["url"] as string;

        private float _currentTime = -1;

        protected virtual void Awake() {
            GameObject webviewGO = Instantiate(Resources.Load("CanvasWebViewPrefab")) as GameObject;
            canvasWebViewPrefab = webviewGO.GetComponent<CanvasWebViewPrefab>();

            webviewGO.transform.SetParent(transform.Find("WebViewContainer"));
            RectTransform rectTransform = webviewGO.GetComponent<RectTransform>();
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            disagreeButton = transform.Find("ButtonConatiner/DisagreeButton").GetComponent<Button>();
            agreeButton = transform.Find("ButtonConatiner/AgreeButton").GetComponent<Button>();

            LocalizableString localizableString = XD.SDK.Common.PC.Internal.Localization.GetCurrentLocalizableString();
            disagreeButton.transform.Find("Text").GetComponent<Text>().text = localizableString.AgreementDisagree;
            agreeButton.transform.Find("Text").GetComponent<Text>().text = localizableString.AgreementAgree;

            netErrorGO = transform.Find("LoadFailedView").gameObject;
            Text netErrorTipsText = netErrorGO.transform.Find("ErrorText").GetComponent<Text>();
            netErrorTipsText.text = localizableString.AgreementLoadFailed;
            Button retryButton = netErrorGO.transform.Find("RetryButton").GetComponent<Button>();
            retryButton.onClick.AddListener(OnNetErrorViewClicked);
            netErrorGO.SetActive(false);

            loadingGO = transform.Find("SmallLoadingView").gameObject;
            loadingGO.SetActive(false);

            agreeButtonNormalSprite = agreeButton.image.sprite;
        }

        protected virtual async void Start() {
            loadingGO.SetActive(true);

            await canvasWebViewPrefab.WaitUntilInitialized();

            loadingGO.SetActive(false);

            canvasWebViewPrefab.WebView.UrlChanged += OnUrlChanged;

            disagreeButton.onClick.AddListener(OnDisagreeButtonClicked);
            agreeButton.onClick.AddListener(OnAgreeButtonClicked);

            await LoadURL();
        }

        private void OnDisable()
        {
            if (_currentTime > 0)
            {
                UIManager.DismissLoading();
                _currentTime = 0;
            }
        }

        private void OnDestroy() {
            if (canvasWebViewPrefab != null && canvasWebViewPrefab.WebView != null) {
                canvasWebViewPrefab.WebView.UrlChanged -= OnUrlChanged;
            }
        }

        private void Update()
        {
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

        void OnDisagreeButtonClicked() {
            XD.SDK.Common.PC.Internal.UIManager.ShowUI<DisagreeConfirmAlert>("DisagreeConfirmAlert", null, (code, data) => {
                if (code == 0) {
                    XD.SDK.Common.PC.Internal.UIManager.Dismiss();
                } else if (code == 1) {
                    OnCallback(1, null);
                }
            });
        }

        protected virtual void OnAgreeButtonClicked() {
            OnCallback(0, null);
        }

        void OnUrlChanged(object sender, UrlChangedEventArgs eventArgs) {
            string url = eventArgs.Url;
            if (url != URL) {
                // TRICK 解决当前页面事件失效的问题
                canvasWebViewPrefab.WebView.StopLoad();
                canvasWebViewPrefab.WebView.LoadUrl(URL);

                url = $"{url}?client_id={ConfigModule.ClientId}&language={XD.SDK.Common.PC.Internal.Localization.GetLanguageKey()}";
                _currentTime = OPEN_URL_TIMEOUT;
                UIManager.ShowLoading(true);
                Application.OpenURL(url);

                // 打开隐私详情页面
                //Dictionary<string, object> data = new Dictionary<string, object> {
                //    { "url", url }
                //};
                //XD.SDK.Common.PC.Internal.UIManager.ShowUI<AgreementDetailAlert>("AgreementDetailAlert", data, null);
            }
        }

        async void OnNetErrorViewClicked() {
            netErrorGO.SetActive(false);
            await LoadURL();
        }

        async Task LoadURL() {
            try {
                loadingGO.SetActive(true);

                // Hack Ping first
                HttpClient client = new HttpClient {
                    Timeout = TimeSpan.FromSeconds(5)
                };
                await client.GetAsync(URL);
                
                canvasWebViewPrefab.WebView.LoadUrl(URL);
                await canvasWebViewPrefab.WebView.WaitForNextPageLoadToFinish();
            } catch (Exception e) {
                Debug.LogWarning(e);
                netErrorGO.SetActive(true);
            } finally {
                if (loadingGO != null)
                    loadingGO.SetActive(false);
            }
        }
    }
}
#endif