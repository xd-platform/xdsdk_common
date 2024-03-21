#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class TrackHttpClient {
        private class RetryHandler : DelegatingHandler
        {
            private const int MAX_RETRY_COUNT = 3;
            public RetryHandler() : base(new HttpClientHandler()) {}

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                HttpResponseMessage response = null;
                for (int i = 0; i < MAX_RETRY_COUNT; i++)
                {
                    response = await base.SendAsync(request, cancellationToken);
                    if (response.IsSuccessStatusCode) {
                        return response;
                    }
                }

                return response;
            }
        }
        private static HttpClient client;

        internal static HttpClient Client {
            get {
                if (client == null) {
                    client = new HttpClient(new RetryHandler());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(10);
                }
                return client;
            }
        }

        public static async Task<HttpResponseMessage> Request(string path,
            HttpMethod method,
            Dictionary<string, object> headers = null,
            object data = null) 
        {
            string url = path;
            HttpRequestMessage request = new HttpRequestMessage {
                RequestUri = new Uri(url),
                Method = method,
            };
            FillHeaders(request.Headers, headers);

            string content = null;
            if (data != null) {
                content = JsonConvert.SerializeObject(data);
                StringContent requestContent = new StringContent(content);
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = requestContent;
            }

            XDHttpUtils.PrintRequest(Client, request, content);
            HttpResponseMessage response = null;
            try
            {
                response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                request.Dispose();
                return response;
            }
            catch (Exception e)
            {
                request.Dispose();
                throw e;
            }
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
#endif