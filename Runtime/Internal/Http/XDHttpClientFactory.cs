using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace XD.SDK.Common.Internal {
    public class XDHttpClientFactory {
        private static readonly string CnBaseURL = "https://xdsdk-6.xd.cn";
        private static readonly string GlobalBaseURL = "https://xdsdk-intnl-6.xd.com";

        internal static List<Func<string, long, Dictionary<string, string>>> QueryParamsFuncs { get; private set; }

        internal static List<Func<HttpRequestMessage, long, Task<Dictionary<string, string>>>> HeadersAsyncFuncs { get; private set; }

        private static volatile XDHttpClient _xdDefaultClient;
        private static readonly object Locker = new object();

        static XDHttpClientFactory() {
            QueryParamsFuncs = new List<Func<string, long, Dictionary<string, string>>>();
            HeadersAsyncFuncs = new List<Func<HttpRequestMessage, long, Task<Dictionary<string, string>>>>();
        }

        public static XDHttpClient GetDefaultXdHttpClient()
        {
            if (_xdDefaultClient != null) return _xdDefaultClient;
            lock (Locker)
            {
                if (_xdDefaultClient == null) {
                    var defaultHost = XDConfigs.IsCN ? CnBaseURL : GlobalBaseURL;
                    _xdDefaultClient = CreateClient(defaultHost);
                }
            }

            return _xdDefaultClient;
        }
        
        public static XDHttpClient CreateClient(string host , HttpMessageHandler handler = null) {
            HttpClient httpClient;
            if (handler == null) {
                httpClient = new HttpClient(CreateDefaultHandler());
            } else {
                httpClient = new HttpClient(handler);
            }
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            XDHttpClient client = new XDHttpClient {
                Host = host,
                Client = httpClient
            };
            return client;
        }

        public static void RegisterQueryParamsFunc(Func<string, long, Dictionary<string, string>> func) {
            QueryParamsFuncs.Add(func);
        }

        public static void RegisterHeadersTaskFunc(Func<HttpRequestMessage, long, Task<Dictionary<string, string>>> asyncFunc) {
            HeadersAsyncFuncs.Add(asyncFunc);
        }

        public static Dictionary<string, string> GetQueryParams(string url, long timestamp) {
            return QueryParamsFuncs.Select(func => func.Invoke(url, timestamp))
                .SelectMany(dict => dict)
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
        }

        public static async Task<Dictionary<string, string>> GetHeaders(HttpRequestMessage request, long timestamp) {
            Dictionary<string, string>[] results = await Task.WhenAll(HeadersAsyncFuncs.Select(func => func.Invoke(request, timestamp)));
            return results.SelectMany(dict => dict)
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
        }

        private static XdDelegatingHandler CreateDefaultHandler()
        {
            return new XdDelegatingHandler(new HttpClientHandler());
        }
    }
}
