using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;
using XD.SDK.Account.Internal;
using XD.SDK.Common.PC.Internal;

namespace XD.SDK.Account {
    public class XDGAccountHttpConfigPC : IXDAccountHttpConfig {
        private static readonly string LetterStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly string GetMethod = "GET";
        private static readonly string PostMethod = "POST";

        public async Task<Dictionary<string, string>> GetAuthorizationHeaders(HttpRequestMessage request, long timestamp) {
            string auth = await GetMacToken(request.RequestUri, request.Method, timestamp);
            if (string.IsNullOrEmpty(auth)) {
                return new Dictionary<string, string>();
            }

            return new Dictionary<string, string> {
                { "Authorization", auth }
            };
        }

        private static Task<string> GetMacToken(Uri uri, HttpMethod method, long timestamp) {
            if (method == HttpMethod.Get) {
                return GetMacToken(uri, GetMethod, timestamp);
            } else if (method == HttpMethod.Post) {
                return GetMacToken(uri, PostMethod, timestamp);
            }
            return null;
        }

        private static async Task<string> GetMacToken(Uri uri, string method, long timestamp) {
            var accessToken = await AccessTokenModule.GetLocalAccessToken();
            string authToken = null;
            if (accessToken != null) {
                string nonce = GetRandomStr(5);
                string md = method.ToUpper();

                string pathAndQuery = uri.PathAndQuery;
                string domain = uri.Host.ToLower();
                string port = $"{uri.Port}";

                var dataStr = $"{timestamp}\n{nonce}\n{md}\n{pathAndQuery}\n{domain}\n{port}\n";
                var mac = Base64WithSecret(accessToken.macKey, dataStr);
                authToken = $"MAC id=\"{accessToken.kid}\",ts=\"{timestamp}\",nonce=\"{nonce}\",mac=\"{mac}\"";
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
    }
}