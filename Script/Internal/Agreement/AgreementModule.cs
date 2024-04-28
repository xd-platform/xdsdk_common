#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace XD.SDK.Common.PC.Internal {
    public enum SignType {
        NEW, // 首次签署
        RENEW, // 续签
        NO, // 不需要签署
    }

    public class AgreementModule {
        private static readonly string KR = "KR";
        private static readonly string US = "US";

        private static readonly string FETACH_AGREEMENT = "api/init/v1/agreement";
        private static readonly string SIGN_AGREEMENT = "api/account/v1/agreement/confirm";

        private static readonly Persistence latestAgreementPersistence = new Persistence(Path.Combine(
            Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "latest_agreement"));

        private static readonly Persistence localSignedAgreementPersistence = new Persistence(Path.Combine(
            Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "agreement"));

        /// <summary>
        /// 线上最新协议
        /// </summary>
        private static Agreement latestAgreement;

        /// <summary>
        /// 当前签署协议
        /// </summary>
        private static Agreement currentSignedAgreement;

        /// <summary>
        /// 当前签署协议是否有效
        /// </summary>
        public static bool SignedAgreementValid { get; private set; }

        private static async Task<Agreement> LoadLatestAgreement() {
            try {
                AgreementResponse response = await XDHttpClient.Get<AgreementResponse>(FETACH_AGREEMENT);
                latestAgreement = response.Agreement;
                await latestAgreementPersistence.Save(latestAgreement);
            } catch (Exception e) {
                XDLogger.Warn($"拉取最新协议失败 {e}");
                latestAgreement = await latestAgreementPersistence.Load<Agreement>();
            }
            return latestAgreement;
        }

        internal static async Task ConfirmAgreement()
        {
            SignedAgreementValid = false;
            await LoadLatestAgreement();

            Tuple<SignType, Agreement> tuple = await GetNeedSignAgreement();
            SignType signType = tuple.Item1;
            Agreement needSignAgreement = tuple.Item2;

            if (signType == SignType.NO) {
                // 不需要签署
                SignedAgreementValid = true;
                return;
            }

            // 需要签署协议
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            bool newSign = signType != SignType.RENEW;
            string url = $"{needSignAgreement.Url}?firstCheck={newSign.ToString().ToLower()}&client_id={ConfigModule.ClientId}&language={XD.SDK.Common.PC.Internal.Localization.GetLanguageKey()}";
            Dictionary<string, object> data = new Dictionary<string, object> {
                { "url", url }
            };
            Action<int, object> callback = (code, result) =>
            {
                var trackInfo = AliyunTrack.AgreementMapExtra2TrackInfo(result as Dictionary<string, object>);
                if (code == 0) {
                    SignedAgreementValid = true;
                    currentSignedAgreement = needSignAgreement;
                    currentSignedAgreement.Extra = result as Dictionary<string, object>;
                    XD.SDK.Common.PC.Internal.UIManager.DismissAll();
                    tcs.TrySetResult(true);
                    AliyunTrack.AgreementSign(trackInfo);
                } else
                {
                    SignedAgreementValid = false;
                    Application.Quit();
                    tcs.TrySetException(new Exception("Disagree"));
                }
            };
            
            if (needSignAgreement.Region == KR) {
                data[KRAgreementAlert.PRIVACY_URL_KEY] = needSignAgreement.KRCollectionAgreementUrl;
                XD.SDK.Common.PC.Internal.UIManager.ShowUI<KRAgreementAlert>("KRAgreementAlert", data, callback);
                AliyunTrack.AgreementAsk(AliyunTrack.AgreementCountryType.Korea, needSignAgreement.Region, needSignAgreement.Version);
            } else if (needSignAgreement.Region == US) {
                XD.SDK.Common.PC.Internal.UIManager.ShowUI<USAgreementAlert>("USAgreementAlert", data, callback);
                AliyunTrack.AgreementAsk(AliyunTrack.AgreementCountryType.US, needSignAgreement.Region, needSignAgreement.Version);
            } else {
                XD.SDK.Common.PC.Internal.UIManager.ShowUI<GeneralAgreementAlert>("GeneralAgreementAlert", data, callback);
                AliyunTrack.AgreementAsk(AliyunTrack.AgreementCountryType.Default, needSignAgreement.Region, needSignAgreement.Version);
            }
            
            
            await tcs.Task;
        }

        internal static async Task<Tuple<SignType, Agreement>> GetNeedSignAgreement() {
            if (CurrentAgreement == null) {
                return new Tuple<SignType, Agreement>(SignType.NO, null);
            }

            LocalSignedAgreements local = await localSignedAgreementPersistence.Load<LocalSignedAgreements>();
            if (local == null ||
                !local.AgreementVersions.TryGetValue(CurrentAgreement.Region, out string localVersion) ||
                string.IsNullOrEmpty(localVersion)) {
                // 如果本地没有签署缓存或没有地区对应签署缓存，则需要签署
                return new Tuple<SignType, Agreement>(SignType.NEW, CurrentAgreement);
            }

            // 比较版本号大小
            Version localVer = new Version(localVersion);
            if (string.IsNullOrEmpty(CurrentAgreement.Version)) {
                // 当本地签署过，但没有本地缓存，并且拉取也失败
                return new Tuple<SignType, Agreement>(SignType.RENEW, CurrentAgreement);
            }

            Version currentVer = new Version(CurrentAgreement.Version);
            if (localVer < currentVer) {
                return new Tuple<SignType, Agreement>(SignType.RENEW, CurrentAgreement);
            }

            return new Tuple<SignType, Agreement>(SignType.NO, null);
        }

        /// <summary>
        /// 签署协议
        /// </summary>
        /// <param name="region"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static async Task SignAgreement(string userId) {
            if (currentSignedAgreement == null) {
                return;
            }

            string region = currentSignedAgreement.Region;
            string version = currentSignedAgreement.Version;
            Dictionary<string, object> data = new Dictionary<string, object>() {
                { "clientId", ConfigModule.ClientId },
                { "deviceCode", SystemInfo.deviceUniqueIdentifier },
                { "agreementRegion", region },
                { "userId", userId }
            };
            if (!string.IsNullOrEmpty(version)) {
                data["agreementVersion"] = version;
            }
            if (currentSignedAgreement.Extra != null) {
                data["extra"] = currentSignedAgreement.Extra;
            }
            AgreementConfirmResponse response = await XDHttpClient.Post<AgreementConfirmResponse>(SIGN_AGREEMENT, data: data);
            if (!response.Confirm.IsSuccess) {
                return;
            }
            
            LocalSignedAgreements localSignedAgreements = await localSignedAgreementPersistence.Load<LocalSignedAgreements>();
            if (localSignedAgreements == null) {
                localSignedAgreements = new LocalSignedAgreements();
            }
            if (localSignedAgreements.AgreementVersions == null) {
                localSignedAgreements.AgreementVersions = new Dictionary<string, string>();
            }
            localSignedAgreements.AgreementVersions[region] = response.Confirm.Version;
            // 持久化
            await localSignedAgreementPersistence.Save(localSignedAgreements);

            currentSignedAgreement = null;
        }

        internal static void ClearLocalSignedAgreements()
        {
            string agreementPath = Path.Combine(Application.persistentDataPath, XDGCommonPC.XD_PERSISTENCE_NAME, "agreement");
            if (File.Exists(agreementPath)) {
                File.Delete(agreementPath);
            }
        }

        internal static Agreement CurrentAgreement {
            get {
                if (latestAgreement != null) {
                    return latestAgreement;
                }
                return FallbackAgreement;
            }
        }

        internal static Agreement FallbackAgreement => ConfigModule.IsGlobal ? null : new Agreement {
            Region = "DF",
            Url = "https://protocol.xd.cn/sdk/merger.html",
            SubAgreements = new List<XDAgreementPC> {
                new XDAgreementPC {
                    type = "terms-of-service",
                    url = "https://protocol.xd.cn/sdk-agreement-1.0.html"
                },
                new XDAgreementPC {
                    type = "privacy-policy",
                    url = "https://protocol.xd.cn/sdk-privacy-1.0.html"
                },
                new XDAgreementPC {
                    type = "children-protection-rules",
                    url = "https://protocol.xd.cn/sdk-child-protection-1.0.html"
                },
                new XDAgreementPC {
                    type = "sharing-checklist",
                    url = "https://protocol.xd.cn/sdk-share-1.0.html"
                }
            }
        };
    }
}
#endif
