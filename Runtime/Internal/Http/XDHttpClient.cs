using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using LC.Newtonsoft.Json;
using LeanCloud.Common;

namespace XD.SDK.Common.Internal {
    public class XDHttpClient {
        internal string Host { get; set; }

        internal HttpClient Client { get; set; }

        public Task<T> Get<T>(string path,
            Dictionary<string, object> headers = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse {

            return Request<T>(path, HttpMethod.Get, headers, null, queryParams);
        }

        public Task<T> Post<T>(string path,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse {
            return Request<T>(path, HttpMethod.Post, headers, data, queryParams);
        }

        public Task<T> Put<T>(string path,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse {
            return Request<T>(path, HttpMethod.Put, headers, data, queryParams);
        }

        public Task Delete(string path,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) {
            return Request<BaseResponse>(path, HttpMethod.Delete, headers, data, queryParams);
        }

        async Task<T> Request<T>(string path,
            HttpMethod method,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) where T : BaseResponse {
            HttpResponseMessage response = await Request(path, method, headers, data, queryParams);
            string resultString = await response.Content.ReadAsStringAsync();
            XDHttpUtils.PrintResponse(response, resultString);
            response.Dispose();

            if (response.IsSuccessStatusCode) {
                T ret = JsonConvert.DeserializeObject<T>(resultString,
                    LCJsonConverter.Default);
                ret.Headers = response.Headers;
                return ret;
            }

            throw HandleErrorResponse(response.StatusCode, resultString);
        }

        public async Task<HttpResponseMessage> Request(string path,
            HttpMethod method,
            Dictionary<string, object> headers = null,
            object data = null,
            Dictionary<string, object> queryParams = null) {
            string url = path;
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (!url.StartsWith("http://") && !url.StartsWith("https://")) {
                // 临时支持完整 url
                url = BuildUrl(path, queryParams, timestamp);
            }
            HttpRequestMessage request = new HttpRequestMessage {
                RequestUri = new Uri(url),
                Method = method,
            };
            FillHeaders(request.Headers, headers);

            Dictionary<string, string> commonHeaders = await XDHttpClientFactory.GetHeaders(request, timestamp);
            foreach (KeyValuePair<string, string> kv in commonHeaders) {
                request.Headers.Add(kv.Key, kv.Value);
            }

            string content = null;
            if (data != null) {
                content = JsonConvert.SerializeObject(data);
                StringContent requestContent = new StringContent(content);
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = requestContent;
            }

            try {
                HttpResponseMessage response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                return response;
            } finally {
                XDHttpUtils.PrintRequest(Client, request, content);
                request.Dispose();
            }
        }

        static XDException HandleErrorResponse(HttpStatusCode statusCode, string responseContent) {
            int httpStatusCode = (int)statusCode;
            string message = responseContent;
            Exception ex = null;
            try {
                // 尝试获取 LeanCloud 返回错误信息
                ErrorResponse error = JsonConvert.DeserializeObject<ErrorResponse>(responseContent,
                    LCJsonConverter.Default);
                if (error?.Code == 0 && error.Data == null && string.IsNullOrEmpty(error.Message) && string.IsNullOrEmpty(error.Detail))
                {
                    ex = new XDHttpException(httpStatusCode, httpStatusCode, message);
                }
                else
                {
                    if (error != null) ex = new XDHttpException(httpStatusCode, error.Code, error.Message, error.Data);
                }
            } catch (Exception e) {
                XDGLogger.Debug(e.ToString());
            }
            throw ex ?? new XDHttpException(httpStatusCode, httpStatusCode, message);
        }

        string BuildUrl(string path, Dictionary<string, object> queryParams, long timestamp) {
            StringBuilder urlSB = new StringBuilder(Host.TrimEnd('/'));
            urlSB.Append($"/{path}");
            string url = urlSB.ToString();

            Dictionary<string, object> queries = new Dictionary<string, object>();

            Dictionary<string, string> commonQueryParams = XDHttpClientFactory.GetQueryParams(url, timestamp);
            if (commonQueryParams != null) {
                commonQueryParams.ToList().ForEach(pair => queries[pair.Key] = pair.Value);
            }

            if (queryParams != null) {
                queryParams.ToList().ForEach(pair => queries[pair.Key] = pair.Value);
            }
            
            if (queries.Count > 0) {
                url = $"{url}?{XDUrlUtils.ToQueryString(queries)}";
            }

            return url;
        }

        static void FillHeaders(HttpRequestHeaders headers, Dictionary<string, object> reqHeaders = null) {
            // 额外 headers
            if (reqHeaders != null) {
                foreach (KeyValuePair<string, object> kv in reqHeaders) {
                    headers.Add(kv.Key, kv.Value.ToString());
                }
            }
        }
    }
}