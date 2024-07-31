#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LC.Newtonsoft.Json;
using LeanCloud.Common;
using UnityEngine;

namespace XD.SDK.Common.PC.Internal
{
    public enum SignType
    {
        NEW, // 首次签署
        RENEW, // 续签
        NO, // 不需要签署
    }

    public class AgreementModule
    {
        private const string FetchAgreementApiPath = "api/init/v2/agreement";
        private const string SignAgreementApiPath = "api/account/v1/agreement/confirm";

        private const string AgreementFileNameCn = "Agreement_local_cn";
        private const string AgreementFileNameKr = "Agreement_local_kr";
        private const string AgreementFileNameDefault = "Agreement_local_df";

        private static readonly Persistence LatestAgreementPersistence = new Persistence(Path.Combine(
            Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "latest_agreement"));

        private static readonly Persistence LocalSignedAgreementPersistence = new Persistence(Path.Combine(
            Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "agreement"));

        /// <summary>
        /// 线上最新协议
        /// </summary>
        private static Agreement _latestAgreement;

        /// <summary>
        /// 当前签署协议
        /// </summary>
        private static Agreement _currentSignedAgreement;

        /// <summary>
        /// 当前签署协议是否有效
        /// </summary>
        public static bool SignedAgreementValid { get; private set; }

        internal static async Task ConfirmAgreement()
        {
            SignedAgreementValid = false;

            try
            {
                // 获取服务端最新数据
                var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(3));
                var response =
                    await XDHttpClient.Get<AgreementResponse>(FetchAgreementApiPath, cancellationToken: cts.Token);
                _latestAgreement = response.Agreement;
                // 服务端数据保存到本地
                await LatestAgreementPersistence.Save(_latestAgreement);
            }
            catch (Exception e)
            {
                XDLogger.Warn($"拉取最新协议失败 {e}");
            }

            if (_latestAgreement == null
                || string.IsNullOrEmpty(_latestAgreement.Title)
                || string.IsNullOrEmpty(_latestAgreement.TitleFirst))
            {
                XDLogger.Warn("fetch agreement failed, try to get cache. ");
                // 服务端拉取失败，读取本地缓存
                _latestAgreement = await LatestAgreementPersistence.Load<Agreement>();
            }

            if (_latestAgreement == null
                || string.IsNullOrEmpty(_latestAgreement.Title)
                || string.IsNullOrEmpty(_latestAgreement.TitleFirst))
            {
                XDLogger.Warn("get cache failed, try to get local. ");
                _latestAgreement = await LoadLocalConfig();
            }

            var tuple = await GetNeedSignAgreement();
            var signType = tuple.Item1;
            var needSignAgreement = tuple.Item2;

            if (signType == SignType.NO)
            {
                // 不需要签署
                SignedAgreementValid = true;
                return;
            }

            // 需要签署协议
            var tcs = new TaskCompletionSource<bool>();
            var newSign = signType != SignType.RENEW;
            var data = new Dictionary<string, object>
            {
                { "firstCheck", newSign },
                { "agreement", needSignAgreement }
            };
            XD.SDK.Common.PC.Internal.UIManager.ShowUI<GeneralAgreementAlert>("GeneralAgreementAlert", data,
                (code, result) =>
                {
                    var trackInfo = AliyunTrack.AgreementMapExtra2TrackInfo(result as Dictionary<string, object>);
                    if (code == 0)
                    {
                        SignedAgreementValid = true;
                        _currentSignedAgreement = needSignAgreement;
                        _currentSignedAgreement.Extra = result as Dictionary<string, object>;
                        XD.SDK.Common.PC.Internal.UIManager.DismissAll();
                        tcs.TrySetResult(true);
                        AliyunTrack.AgreementSign(trackInfo);
                    }
                    else
                    {
                        SignedAgreementValid = false;
                        Application.Quit();
                        tcs.TrySetException(new Exception("Disagree"));
                    }
                });

            var countryType = AliyunTrack.AgreementCountryType.Default;
            if (needSignAgreement.Region.Equals("KR", StringComparison.OrdinalIgnoreCase))
            {
                countryType = AliyunTrack.AgreementCountryType.Korea;
            } else if (needSignAgreement.Region.Equals("US", StringComparison.OrdinalIgnoreCase))
            {
                countryType = AliyunTrack.AgreementCountryType.US;
            }
            AliyunTrack.AgreementAsk(countryType, needSignAgreement.Region,
                needSignAgreement.Version);
            await tcs.Task;
        }

        private static async Task<Agreement> LoadLocalConfig()
        {
            if (!ConfigModule.IsGlobal)
            {
                return GetAgreement(AgreementFileNameCn, XD.SDK.Common.PC.Internal.Localization.GetLanguageKey(LangType.ZH_CN));
            }

            var currentRegion = "DF";
            // 指定了地区就用该地区
            if (!string.IsNullOrEmpty(ConfigModule.region))
            {
                currentRegion = ConfigModule.region.ToUpper();
            }
            else
            {
                // 没有指定则尝试从 ip 中获取地区
                var ipInfo = await IPInfoModule.GetIpInfo();
                if (ipInfo != null)
                {
                    currentRegion = ipInfo.countryCode.ToUpper();
                }
            }
            // 非 US 和 KR 都还原到 DF
            if (!currentRegion.Equals("US", StringComparison.OrdinalIgnoreCase) && !currentRegion.Equals("KR", StringComparison.OrdinalIgnoreCase))
            {
                currentRegion = "DF";
            }
            
            XDLogger.Warn("get local config : " + currentRegion);

            if (currentRegion.Equals("KR", StringComparison.OrdinalIgnoreCase))
            {
                return GetAgreement(AgreementFileNameKr, XD.SDK.Common.PC.Internal.Localization.GetLanguageKey(LangType.KR));
            }

            var currentLang = XD.SDK.Common.PC.Internal.Localization.GetLanguageKey();
            // todo 海外缺少越南翻译，降级为英文
            if (currentLang.Equals(XD.SDK.Common.PC.Internal.Localization.GetLanguageKey(LangType.VI)))
            {
                currentLang = XD.SDK.Common.PC.Internal.Localization.GetLanguageKey(LangType.EN);
            }

            var tempAgreement = GetAgreement(AgreementFileNameDefault, currentLang);
            // 从 df 文件中加载后非美国时去除勾选
            if (tempAgreement != null && !currentRegion.Equals("US", StringComparison.OrdinalIgnoreCase))
            {
                tempAgreement.Region = currentRegion;
                tempAgreement.Options = null;
            }

            return tempAgreement;
        }

        private static Agreement GetAgreement(string jsonFileName, string language)
        {
            var txtAsset = Resources.Load("Localization/" + jsonFileName) as TextAsset;
            if (txtAsset == null) return null;
            var localizableStrings = JsonConvert.DeserializeObject<Dictionary<string, Agreement>>(txtAsset.text,
                LCJsonConverter.Default);
            return localizableStrings?[language];
        }

        private static async Task<Tuple<SignType, Agreement>> GetNeedSignAgreement()
        {
            if (_latestAgreement == null)
            {
                return new Tuple<SignType, Agreement>(SignType.NO, null);
            }

            var local = await LocalSignedAgreementPersistence.Load<LocalSignedAgreements>();
            if (local == null ||
                !local.AgreementVersions.TryGetValue(_latestAgreement.Region, out string localVersion) ||
                string.IsNullOrEmpty(localVersion))
            {
                // 如果本地没有签署缓存或没有地区对应签署缓存，则需要签署
                return new Tuple<SignType, Agreement>(SignType.NEW, _latestAgreement);
            }

            if (string.IsNullOrEmpty(_latestAgreement.Version))
            {
                // 当本地签署过，但没有本地缓存，并且拉取也失败
                return new Tuple<SignType, Agreement>(SignType.RENEW, _latestAgreement);
            }

            // 比较版本号大小
            var localVer = new Version(localVersion);
            var latestVer = new Version(_latestAgreement.Version);
            if (localVer < latestVer)
            {
                return new Tuple<SignType, Agreement>(SignType.RENEW, _latestAgreement);
            }

            return new Tuple<SignType, Agreement>(SignType.NO, null);
        }

        /// <summary>
        /// 签署协议
        /// </summary>
        /// <param name="userId">用户 xdid</param>
        /// <returns></returns>
        internal static async Task SignAgreement(string userId)
        {
            if (_currentSignedAgreement == null)
            {
                return;
            }

            string region = _currentSignedAgreement.Region;
            string version = _currentSignedAgreement.Version;
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "clientId", ConfigModule.ClientId },
                { "deviceCode", SystemInfo.deviceUniqueIdentifier },
                { "agreementRegion", region },
                { "userId", userId }
            };
            if (!string.IsNullOrEmpty(version))
            {
                data["agreementVersion"] = version;
            }

            if (_currentSignedAgreement.Extra != null)
            {
                data["extra"] = _currentSignedAgreement.Extra;
            }

            AgreementConfirmResponse response =
                await XDHttpClient.Post<AgreementConfirmResponse>(SignAgreementApiPath, data: data);
            if (!response.Confirm.IsSuccess)
            {
                return;
            }

            var localSignedAgreements =
                await LocalSignedAgreementPersistence.Load<LocalSignedAgreements>();
            if (localSignedAgreements == null)
            {
                localSignedAgreements = new LocalSignedAgreements();
            }

            if (localSignedAgreements.AgreementVersions == null)
            {
                localSignedAgreements.AgreementVersions = new Dictionary<string, string>();
            }

            localSignedAgreements.AgreementVersions[region] = response.Confirm.Version;
            // 持久化
            await LocalSignedAgreementPersistence.Save(localSignedAgreements);

            _currentSignedAgreement = null;
        }

        internal static void ClearLocalSignedAgreements()
        {
            var agreementPath = Path.Combine(Application.persistentDataPath, XDGCommonPC.XD_PERSISTENCE_NAME,
                "agreement");
            if (File.Exists(agreementPath))
            {
                File.Delete(agreementPath);
            }
        }
    }
}
#endif