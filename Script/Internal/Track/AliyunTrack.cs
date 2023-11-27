#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LC.Newtonsoft.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;
using TapTap.TapDB;

namespace XD.SDK.Common.PC.Internal {
    internal class AliyunTrackBadResponse 
    {
        [JsonProperty("errorCode")]
        public string errorCode { get; private set; }
        
        [JsonProperty("errorMessage")]
        public string errorMessage { get; private set; }
    }
    
    internal class AliyunTrackRequest
    {
        [JsonProperty("__logs__")]
        public Dictionary<string, string>[] content { get; set; }
    }

    internal partial class AliyunTrack
    {
        private static readonly string GlobalTrackHost = "https://event-tracking-global.ap-southeast-1.log.aliyuncs.com"; 
        private static readonly string ChinaTrackHost = "https://event-tracking-cn.cn-beijing.log.aliyuncs.com"; 
        
        private static readonly string TestLogStoreUrl = "logstores/sdk6-test/track";
        private static readonly string FormalLogStoreUrl = "logstores/sdk6-prod/track";
        
        // 首次打开时间戳 key
        private const string FIRST_OPEN_KEY = "unity_pc_first_open_timestamp";
        private const string CREATE_TIME_KEY = "unity_create_time";
        private static string FirstOpenTimestamp { get; set; }
        private static DateTime UtcTime => new DateTime(1970, 1, 1);

        private static bool isGolbal;
        // 是否为测试服,否则为正式服
        private static bool isDevServer;
        
        private static Dictionary<string, object> Headers =>
            new Dictionary<string, object>()
            {
                ["x-log-apiversion"] = "0.6.0",
            };

        public static string TrackUrl
        {
            get => (isGolbal ? GlobalTrackHost : ChinaTrackHost) + "/" + (isDevServer ? TestLogStoreUrl : FormalLogStoreUrl);
        } 
        
        private static Dictionary<string, string> content = new Dictionary<string, string>();
        
        private static Dictionary<string, string>[] contents = new Dictionary<string, string>[1] {content};

        private static List<AliyunTrackRequest> requestList = new List<AliyunTrackRequest>();

        private static bool startSending;

        private static long EventIndex = 1;
        
        /// <summary>
        /// 每次启动更新一次 session_uuid
        /// </summary>
        public static string CurrentSessionUUId { get; private set; }

        private static long GetUtcTimeStamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }

        internal static string GetNewEventSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        internal static void Init()
        {
            CurrentSessionUUId = Guid.NewGuid().ToString();
            isGolbal = ConfigModule.IsGlobal;
            CheckFirstOpen();
        }
        
        private static void CheckFirstOpen()
        {
            if (IsFirstOpen(out string createTime))
            {
                RecordFirstOpen(createTime);
            }
            else
            {
                FirstOpenTimestamp = PlayerPrefs.GetString(FIRST_OPEN_KEY);
            }
        }

        private static bool IsFirstOpen(out string createTimeString)
        {
            var isFirstOpen = false;
            var createTimeRecord = PlayerPrefs.GetString(CREATE_TIME_KEY, "");
            var createTimeCurrent = (long)GetRootFolderInfo().CreationTimeUtc.Subtract(UtcTime).TotalSeconds;
            createTimeString = createTimeCurrent.ToString();

            if (createTimeRecord != createTimeString) 
                isFirstOpen = true;
            
            return isFirstOpen;
        }

        private static DirectoryInfo GetRootFolderInfo()
        {
            return new DirectoryInfo(Application.dataPath);
        }
        
        private static void RecordFirstOpen(string createTimeCurrentStr)
        {
            var firstOpenTime = (long)DateTime.UtcNow.Subtract(UtcTime).TotalMilliseconds;
            FirstOpenTimestamp = firstOpenTime.ToString();
            PlayerPrefs.SetString(FIRST_OPEN_KEY, FirstOpenTimestamp);
            PlayerPrefs.SetString(CREATE_TIME_KEY, createTimeCurrentStr);
        }

        private static string GetLogId(string eventSessionId, string eventName)
        {
            return string.Format($"{eventSessionId}{eventName}{GetUtcTimeStamp()}");
        }

        private static bool SignedAgreementValid()
        {
            return AgreementModule.SignedAgreementValid;
        }

        private static void RefreshPresetPropertiesAsync()
        {
            RefreshStaticPresetProperties();
            var userAgreeGetDeviceInfo = SignedAgreementValid();
            if (userAgreeGetDeviceInfo)
                RefreshDeviceInfos();
        }
        
        private static void RefreshStaticPresetProperties()
        {
            content["channel"] = ConfigModule.Channel;
            content["session_uuid"] = CurrentSessionUUId;
            content["app_version"] = Application.version;
            content["sdk_version"] = XDGCommonPC.SDKVersion;
            content["xd_client_id"] = ConfigModule.ClientId;
            content["source"] = "client";
            content["os"] = GetPlatform();
            content["game_first_open_timestamp"] = FirstOpenTimestamp;
        }
        
        public static string GetPlatform() 
        {
            string os = "Linux";
#if UNITY_STANDALONE_OSX
            os = "MacOS";
#elif UNITY_STANDALONE_WIN
            os = "Windows";
#elif UNITY_ANDROID
            os = "Android";
#elif UNITY_IOS
            os = "iOS";
#endif
            return os;
        }
        
        private static void RefreshDeviceInfos()
        {
            content["device_id"] = SystemInfo.deviceUniqueIdentifier;
            content["os_version"] = SystemInfo.operatingSystem;
            content["brand"] = SystemInfo.deviceModel;
            content["model"] = SystemInfo.processorType;
        }
        
        private static void RefreshCommonPropertiesAsync()
        {
            content["event_index"] = EventIndex.ToString();
            EventIndex++;
            content["time"] = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + "";
            if (SignedAgreementValid())
            {
                content["orientation"] = "unknown";
                var orientationString = Screen.orientation.ToString().ToLower();
                if (orientationString.Contains("protrait"))
                    content["orientation"] = "protrait";
                else if (orientationString.Contains("landscape"))
                    content["orientation"] = "landscape";
                content["width"] = Screen.currentResolution.width.ToString();
                content["height"] = Screen.currentResolution.height.ToString();
                content["lang"] = StandaloneDeviceInfo.GetLanguage();
                content["loc"] = StandaloneDeviceInfo.GetLocation();

                var isInit = false;
                XDGCommon.IsInitialized((b => isInit = b));
                if (!isInit) return;

                content["device_id_in_db"] = DeviceIdInDB == null ? string.Empty : DeviceIdInDB;

                var user = UserModule.current;
                if (user == null) return;

                content["account"] = user.userId;
                if (!content.ContainsKey("isLoginModule") || content["isLoginModule"] != "true")
                    content["login_type"] = !string.IsNullOrEmpty(loginTypeString) ? loginTypeString : user.getLoginType().ToString();
            }
            else
            {
                content["orientation"] = "";
                content["width"] = "";
                content["height"] = "";
                content["lang"] = "";
                content["account"] = "";
                if (!content.ContainsKey("isLoginModule") || content["isLoginModule"] != "true")
                    content["login_type"] = "";
            }
        }

        internal static void LogEventAsync(string eventName, Dictionary<string, string> contents)
        {
            XDLogger.Debug(string.Format($"Track Event: [{eventName}]"));
            try
            {
                content.Clear();
                if (contents != null)
                {
                    foreach (var kv in contents)
                    {
                        content[kv.Key] = kv.Value;
                    }
                }

                content["name"] = eventName;
                RefreshPresetPropertiesAsync();
                RefreshCommonPropertiesAsync();
                TrackAsync();
            }
            catch (Exception e)
            {
                XDLogger.Error($"[XD::LogEventAsync] TrackAsync Event Error! Message: {e.Message}\n{e.StackTrace}");
            }
        }

        private static async Task SendTrackInfo()
        {
            startSending = true;
            while (true)
            {
                if (requestList.Count > 0)
                {
                    var request = requestList[0];
                    try
                    {
                        var responseMessage = await TrackHttpClient.Request(TrackUrl, HttpMethod.Post, Headers, request);
                        if (!responseMessage.IsSuccessStatusCode)
                        {
                            string resultString = await responseMessage.Content.ReadAsStringAsync();
                            var badResponse = JsonConvert.DeserializeObject<AliyunTrackBadResponse>(resultString);
                            XDLogger.Error(string.Format($"[XD::TrackAsync] request failed! \n statusCode: {responseMessage.StatusCode} errorCode : {badResponse?.errorCode} errorMessage : {badResponse?.errorMessage}"));
                        }

                        responseMessage.Dispose();
                        requestList.RemoveAt(0);
                    }
                    catch (Exception e)
                    {
                        XDLogger.Error(string.Format($"[XD::TrackAsync] request failed! Message: {e.Message}\n {e.StackTrace}"));
                        requestList.RemoveAt(0);
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }

        private static void TrackAsync()
        {
            var tempDic = new List<Dictionary<string, string>>();
            foreach (var ct in contents)
            {
                var infoStr = JsonConvert.SerializeObject(ct);
                tempDic.Add(new Dictionary<string, string>()
                {
                    {"content", infoStr}
                });
            }
            var request = new AliyunTrackRequest()
            {
                content = tempDic.ToArray(),
            };
            
            ProcessContents(request);
            DebugContentsInfo();
            
            requestList.Add(request);
            if (!startSending)
                _ = SendTrackInfo();
        }

        private static void ProcessContents(AliyunTrackRequest requestContent)
        {
            if (requestContent != null)
            {
                var nullKeys = new HashSet<string>();
                foreach (var dic in requestContent.content)
                {
                    nullKeys.Clear();
                    foreach (var kv in dic)
                    {
                        if (dic[kv.Key] == null)
                            nullKeys.Add(kv.Key);
                    }

                    foreach (var nullKey in nullKeys)
                    {
                        dic[nullKey] = "null";
                    }
                }
            }
        }
        
        [Conditional("UNITY_EDITOR")]
        private static void DebugContentsInfo()
        {
            var sb = new StringBuilder();
            if (contents != null)
            {
                foreach (var dic in contents)
                {
                    foreach (var kv in dic)
                    {
                        sb.Append($"{kv.Key} : {kv.Value}\n");
                    }
                }
            }

            try
            {
                Debug.LogFormat($"TrackAsync Info:\n{sb.ToString()}");
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat($"{e.Message}\n{e.StackTrace}");
            }
        }

        private static string DeviceIdInDB {
            get {
                Type TapDBType = typeof(TapDB);
                FieldInfo dbField = TapDBType.GetField("db", BindingFlags.Static | BindingFlags.NonPublic);
                object db = dbField.GetValue(null);

                Type dbType = db.GetType();
                FieldInfo identityField = dbType.GetField("Identity", BindingFlags.Static | BindingFlags.NonPublic);
                object identity = identityField.GetValue(null);

                if (identity == null) {
                    return null;
                }

                Type identityType = identity.GetType();
                PropertyInfo deviceIdProperty = identityType.GetProperty("DeviceId");
                object deviceId = deviceIdProperty.GetValue(identity);

                if (deviceId == null) {
                    return null;
                }

                return deviceId as string;
            }
        }
    }
}
#endif