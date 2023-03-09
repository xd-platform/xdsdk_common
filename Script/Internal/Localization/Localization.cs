#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using UnityEngine;
using LC.Newtonsoft.Json;
using LeanCloud.Common;
using XD.SDK.Common.PC;
using LangType = XD.SDK.Common.LangType;

namespace XD.SDK.Common.PC.Internal {
    public class Localization {
        private static Dictionary<string, LocalizableString> localizableStrings;

        private static Dictionary<string, LocalizableString> LocalizableStrings {
            get {
                if (localizableStrings == null) {
                    var txtAsset = Resources.Load("Localization/LocalizableString") as TextAsset;
                    localizableStrings = JsonConvert.DeserializeObject<Dictionary<string, LocalizableString>>(txtAsset.text,
                        LCJsonConverter.Default);
                }
                return localizableStrings;
            }
        }

        public static LangType CurrentLang {
            get; set;
        }

        public static LocalizableString GetCurrentLocalizableString() {
            return LocalizableStrings[GetLanguageKey()];
        }

        public static string GetLanguageKey() {
            switch (CurrentLang) {
                case LangType.ZH_CN:
                    return @"zh_CN";
                case LangType.ZH_TW:
                    return @"zh_TW";
                case LangType.EN:
                    return @"en_US";
                case LangType.TH:
                    return @"th_TH";
                case LangType.ID:
                    return @"in_ID";
                case LangType.KR:
                    return @"ko_KR";
                case LangType.JP:
                    return @"ja_JP";
                case LangType.DE:
                    return @"de_DE";
                case LangType.FR:
                    return @"fr_FR";
                case LangType.PT:
                    return @"pt_PT";
                case LangType.ES:
                    return @"es_ES";
                case LangType.TR:
                    return @"tr_TR";
                case LangType.RU:
                    return @"ru_RU";
                case LangType.VI:
                    return @"vi_VN";
                default:
                    return @"en_US";
            }
        }
        
        public static string GetCustomerCenterLang() {
            switch (CurrentLang) {
                case LangType.ZH_CN:
                    return @"cn";
                case LangType.ZH_TW:
                    return @"tw";
                case LangType.EN:
                    return @"us";
                case LangType.TH:
                    return @"th";
                case LangType.ID:
                    return @"id";
                case LangType.KR:
                    return @"kr";
                case LangType.JP:
                    return @"jp";
                case LangType.DE:
                    return @"de";
                case LangType.FR:
                    return @"fr";
                case LangType.PT:
                    return @"pt";
                case LangType.ES:
                    return @"es";
                case LangType.TR:
                    return @"tr";
                case LangType.RU:
                    return @"ru";
                default:
                    return @"us";
            }
        }
    }
}
#endif