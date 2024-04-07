using System;
using System.Net;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace XD.SDK.Common.Internal {
    public static class XDUrlUtils {
        public static NameValueCollection ParseQueryString(string query) {
            NameValueCollection nvc = new NameValueCollection();

            if (query.StartsWith("?")) {
                query = query.Substring(1);
            }

            foreach (var param in query.Split('&')) {
                string[] pair = param.Split('=');
                if (pair.Length == 2) {
                    string key = WebUtility.UrlDecode(pair[0]);
                    string value = WebUtility.UrlDecode(pair[1]);
                    nvc[key] = value;
                }
            }

            return nvc;
        }

        public static string ToQueryString(Dictionary<string, object> queryParams) {
            return string.Join("&",
                queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value.ToString())}")
            );
        }
    }
}
