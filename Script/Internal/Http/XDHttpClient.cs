#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using LC.Newtonsoft.Json;
using LeanCloud.Common;
using Random = System.Random;

namespace XD.SDK.Common.PC.Internal {
    public class XDHttpClient {
        private static readonly string LetterStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // 正式
        // 国内
        private readonly static string CN_HOST = "https://xdsdk-6.xd.cn";
        // 国际
        private readonly static string GLOBAL_HOST = "https://xdsdk-intnl-6.xd.com";

        private static HttpClient client;

        internal static HttpClient Client {
            get {
                if (client == null) {
                    client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(XD.SDK.Common.PC.Internal.Localization.GetLanguageKey()));
                    client.Timeout = TimeSpan.FromSeconds(10);
                }
                return client;
            }
        }

        public static Task<T> Get<T>(string path,
            Dictionary<string, object> headers = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse {

            return Request<T>(path, HttpMethod.Get, headers, null, queryParams);
        }

        public static Task<T> Post<T>(string path,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse {
            return Request<T>(path, HttpMethod.Post, headers, data, queryParams);
        }

        public static Task<T> Put<T>(string path,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse {
            return Request<T>(path, HttpMethod.Put, headers, data, queryParams);
        }

        public static Task Delete(string path,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) {
            return Request<BaseResponse>(path, HttpMethod.Delete, headers, data, queryParams);
        }

        static async Task<T> Request<T>(string path,
            HttpMethod method,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse
        {
            var response = await Request(path, method, headers, data, queryParams);
            string resultString = await response.Content.ReadAsStringAsync();
            XDHttpUtils.PrintResponse(response, resultString);
            response.Dispose();

            if (response.IsSuccessStatusCode) {
                T ret = JsonConvert.DeserializeObject<T>(resultString,
                    LCJsonConverter.Default);
                return ret;
            }

            throw HandleErrorResponse(response.StatusCode, resultString);
        }
        
        public static async Task<HttpResponseMessage> Request(string path,
            HttpMethod method,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) {
            string url = path;
            if (!url.StartsWith("http://") && !url.StartsWith("https://")) {
                // 临时支持完整 url
                url = BuildUrl(path, queryParams);
            }
            HttpRequestMessage request = new HttpRequestMessage {
                RequestUri = new Uri(url),
                Method = method,
            };
            FillHeaders(request.Headers, headers);

            // 鉴权
            string auth = await GetMacToken(url, method);
            if (!string.IsNullOrEmpty(auth)) {
                request.Headers.Add("Authorization", auth);
            }

            string content = null;
            if (data != null) {
                content = JsonConvert.SerializeObject(data);
                StringContent requestContent = new StringContent(content);
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = requestContent;
            }
            XDHttpUtils.PrintRequest(Client, request, content);
            HttpResponseMessage response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            request.Dispose();

            return response;
        }

        static XDException HandleErrorResponse(HttpStatusCode statusCode, string responseContent) {
            int code = (int)statusCode;
            string message = responseContent;
            Dictionary<string, object> data = null;
            try {
                // 尝试获取 LeanCloud 返回错误信息
                ErrorResponse error = JsonConvert.DeserializeObject<ErrorResponse>(responseContent,
                    LCJsonConverter.Default);
                code = error.Code;
                message = error.Message;
                data = error.Data;
            } catch (Exception e) {
                XDGLogger.Debug(e.ToString());
            }
            return new XDException(code, message, data);
        }

        static string BuildUrl(string path, Dictionary<string, object> queryParams) {
            string apiServer = ConfigModule.IsGlobal ? GLOBAL_HOST : CN_HOST;
            StringBuilder urlSB = new StringBuilder(apiServer.TrimEnd('/'));
            urlSB.Append($"/{path}");
            string url = urlSB.ToString();

            Dictionary<string, object> queries = new Dictionary<string, object>();

            Dictionary<string, string> commonQueryParams = GetCommonParam(apiServer);
            if (commonQueryParams != null) {
                commonQueryParams.ToList().ForEach(pair => queries[pair.Key] = pair.Value);
            }

            if (queryParams != null) {
                queryParams.ToList().ForEach(pair => queries[pair.Key] = pair.Value);
            }

            if (queries.Count > 0) {
                url = $"{url}?{EncodeQueryParams(queries)}";
            }

            return url;
        }

        internal static string EncodeQueryParams(Dictionary<string, object> queryParams) {
            IEnumerable<string> queryPairs = queryParams.Select(kv => $"{kv.Key}={kv.Value}");
            string queries = string.Join("&", queryPairs);
            return queries;
        }

        static void FillHeaders(HttpRequestHeaders headers, Dictionary<string, object> reqHeaders = null) {
            // 额外 headers
            if (reqHeaders != null) {
                foreach (KeyValuePair<string, object> kv in reqHeaders) {
                    headers.Add(kv.Key, kv.Value.ToString());
                }
            }
        }

        private static Task<string> GetMacToken(string url, HttpMethod method) {
            if (method == HttpMethod.Get) {
                return GetMacToken(url, "GET");
            } else if (method == HttpMethod.Post) {
                return GetMacToken(url, "POST");
            }
            return null;
        }

        private static async Task<string> GetMacToken(string url, string method) {
            var accessToken = await AccessTokenModule.GetLocalAccessToken();
            string authToken = null;
            if (accessToken != null) {
                var uri = new Uri(url);
                var timeStr = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + "";
                var nonce = GetRandomStr(5);
                var md = method.ToUpper();

                var pathAndQuery = uri.PathAndQuery;
                var domain = uri.Host.ToLower();
                var port = uri.Port + "";

                var dataStr = $"{timeStr}\n{nonce}\n{md}\n{pathAndQuery}\n{domain}\n{port}\n";
                var mac = Base64WithSecret(accessToken.macKey, dataStr);
                authToken = $"MAC id=\"{accessToken.kid}\",ts=\"{timeStr}\",nonce=\"{nonce}\",mac=\"{mac}\"";
            }

            return authToken;
        }

        private static string GetRandomStr(int length) {
            StringBuilder SB = new StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < length; i++) {
                SB.Append(LetterStr.Substring(rd.Next(0, LetterStr.Length), 1));
            }

            return SB.ToString();
        }

        private static string Base64WithSecret(string secret, string data) {
            Byte[] secretBytes = UTF8Encoding.UTF8.GetBytes(secret);
            HMACSHA1 hmac = new HMACSHA1(secretBytes);
            Byte[] dataBytes = UTF8Encoding.UTF8.GetBytes(data);
            Byte[] calcHash = hmac.ComputeHash(dataBytes);
            String calcHashString = Convert.ToBase64String(calcHash);
            return calcHashString;
        }

        private static Dictionary<string, string> GetCommonParam(string url) {
            if (url.Contains("ip.xindong.com")) {
                return null;
            }

            var clientId = ConfigModule.ClientId;
            var time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            Dictionary<string, string> param = new Dictionary<string, string>{
                {"clientId", string.IsNullOrEmpty(clientId) ? "" : clientId},
                {"appId", ConfigModule.AppId == null ? "" : ConfigModule.AppId },
                {"region", ConfigModule.RegionType},
                {"did", SystemInfo.deviceUniqueIdentifier},
                {"sdkLang", XD.SDK.Common.PC.Internal.Localization.GetLanguageKey()},
                {"time", time + ""},
                {"chn", "PC"},
                {"locationInfoType", "ip"},
                {"mem", SystemInfo.systemMemorySize / 1024 + "GB"},
                {"res", Screen.width + "_" + Screen.height},
                {"mod", SystemInfo.deviceModel},
                {"sdkVer", XDGCommonPC.SDKVersion},
                {"pkgName", Application.identifier},
                {"brand", SystemInfo.deviceModel},
                {"os", SystemInfo.operatingSystem},
                {"pt", GetPlatform()},
                {"appVer", Application.version},
                {"appVerCode", Application.version},
                {"cpu", SystemInfo.processorType},
                { "lang", StandaloneDeviceInfo.GetLanguage() },
                { "loc", StandaloneDeviceInfo.GetLocation() }
            };
            if (!string.IsNullOrEmpty(ConfigModule.region)) {
                param["countryCode"] = ConfigModule.region;
            }
            return param;
        }

        private static string GetPlatform() {
            string os = "Linux";
#if UNITY_STANDALONE_OSX
            os = "macOS";
#endif
#if UNITY_STANDALONE_WIN
            os = "Windows";
#endif
            return os;
        }
    }
}
#endif
