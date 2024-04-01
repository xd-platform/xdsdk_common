using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using LC.Newtonsoft.Json;
using XD.SDK.Common.Internal;
using XD.SDK.Common;

namespace XD.SDK.Announcement.Internal {
    public class HttpCacheHandler : DelegatingHandler {
        private static readonly HashSet<string> IgnoreHeaderKeys = new HashSet<string> { "time", "res" };

        private readonly string cacheDirectory;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> cacheLocks;

        public HttpCacheHandler(string cacheDirectory) : base(new HttpClientHandler()) {
            this.cacheDirectory = cacheDirectory;
            if (!Directory.Exists(cacheDirectory)) {
                Directory.CreateDirectory(cacheDirectory);
            }
            cacheLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            string cacheKey = GenerateCacheKey(request.RequestUri);
            string cacheFilePath = Path.Combine(cacheDirectory, cacheKey);

            CacheEntry cacheEntry = null;

            if (File.Exists(cacheFilePath)) {
                cacheEntry = await ReadCacheEntry(cacheFilePath);

                if (cacheEntry != null) {
                    if (cacheEntry.IsValid) {
                        XDGLogger.Debug("Hit the strong cache.");
                        HttpResponseMessage cachedResponse = new HttpResponseMessage {
                            RequestMessage = request,
                            Content = new StringContent(cacheEntry.Content)
                        };
                        cachedResponse.Headers.Date = cacheEntry.Date;
                        return cachedResponse;
                    }

                    if (!string.IsNullOrEmpty(cacheEntry.ETag)) {
                        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(cacheEntry.ETag));
                    }
                    if (cacheEntry.LastModified.HasValue) {
                        request.Headers.IfModifiedSince = cacheEntry.LastModified;
                    }
                }
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotModified) {
                if (cacheEntry == null) {
                    return response;
                }

                XDGLogger.Debug("Conditional cache hit.");
                HttpResponseMessage cachedResponse = new HttpResponseMessage {
                    RequestMessage = request,
                    Content = new StringContent(cacheEntry.Content)
                };
                cachedResponse.Headers.Date = response.Headers.Date;
                return cachedResponse;
            }

            if (response.IsSuccessStatusCode && CanCache(response.Headers)) {
                CacheEntry newCacheEntry = new CacheEntry {
                    Date = response.Headers.Date,
                    ETag = response.Headers.ETag?.Tag,
                    LastModified = response.Content.Headers.LastModified,
                };
                newCacheEntry.SetExpires(response.Headers);
                newCacheEntry.Content = await response.Content.ReadAsStringAsync();

                await WriteCacheEntry(cacheFilePath, newCacheEntry);
            }

            return response;
        }

        private bool CanCache(HttpResponseHeaders headers) {
            return headers?.CacheControl?.NoStore != true;
        }

        private async Task<CacheEntry> ReadCacheEntry(string cacheFilePath) {
            SemaphoreSlim cacheLock = GetCacheLock(cacheFilePath);
            await cacheLock.WaitAsync();

            try {
                string text;
                using (FileStream fs = File.OpenRead(cacheFilePath)) {
                    byte[] buffer = new byte[fs.Length];
                    await fs.ReadAsync(buffer, 0, (int)fs.Length);
                    text = EncryptionUtils.Decrypt(Encoding.UTF8.GetString(buffer), SystemInfo.deviceUniqueIdentifier);
                }
                return JsonConvert.DeserializeObject<CacheEntry>(text);
            } catch (Exception e) {
                XDGLogger.Warn(e.Message);
                try {
                    File.Delete(cacheFilePath);
                } catch (Exception ex) {
                    XDGLogger.Warn(ex.Message);
                }
                return null;
            } finally {
                cacheLock.Release();
            }
        }

        private async Task WriteCacheEntry(string cacheFilePath, CacheEntry cacheEntry) {
            SemaphoreSlim cacheLock = GetCacheLock(cacheFilePath);
            await cacheLock.WaitAsync();

            string text;
            try {
                text = EncryptionUtils.Encrypt(JsonConvert.SerializeObject(cacheEntry), SystemInfo.deviceUniqueIdentifier);
                using (FileStream fs = File.Create(cacheFilePath)) {
                    byte[] buffer = Encoding.UTF8.GetBytes(text);
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }
            } catch (Exception e) {
                XDGLogger.Warn(e.Message);
                return;
            } finally {
                cacheLock.Release();
            }
        }

        private SemaphoreSlim GetCacheLock(string cacheFilePath) {
            return cacheLocks.GetOrAdd(cacheFilePath, _ = new SemaphoreSlim(1, 1));
        }

        private static string GenerateCacheKey(Uri requestUri) {
            NameValueCollection queries = XDUrlUtils.ParseQueryString(requestUri.Query);
            foreach (string key in IgnoreHeaderKeys) {
                queries.Remove(key);
            }
            Dictionary<string, string> sortedQueries = queries.AllKeys
                .OrderBy(key => key)
                .ToDictionary(key => key, key => queries[key]);

            StringBuilder normalizedQuery = new StringBuilder();
            foreach (var kvp in sortedQueries) {
                if (normalizedQuery.Length > 0) {
                    normalizedQuery.Append("&");
                }
                normalizedQuery.Append($"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}");
            }
            XDGLogger.Debug($"Cache Key Source: {normalizedQuery}");

            UriBuilder ub = new UriBuilder(requestUri) {
                Query = normalizedQuery.ToString()
            };

            using (MD5 md5 = MD5.Create()) {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(ub.Uri.AbsoluteUri));
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}