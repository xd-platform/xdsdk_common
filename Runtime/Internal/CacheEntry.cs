using System;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using LC.Newtonsoft.Json;

namespace XD.SDK.Announcement.Internal {
    public class CacheEntry {
        public DateTimeOffset? Date { get; set; }

        public DateTimeOffset Expires { get; set; }

        public string ETag { get; set; }

        public DateTimeOffset? LastModified { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [JsonIgnore]
        public bool IsValid => DateTimeOffset.UtcNow <= Expires;

        public void SetExpires(HttpResponseHeaders headers) {
            if (headers.CacheControl?.MaxAge.HasValue == true) {
                Expires = DateTimeOffset.UtcNow.Add(headers.CacheControl.MaxAge.Value);
            } else {
                if (headers.TryGetValues("Expires", out IEnumerable<string> values)) {
                    string expiresHeader = values.FirstOrDefault();
                    if (DateTimeOffset.TryParse(expiresHeader, out DateTimeOffset expiresValue)) {
                        Expires = expiresValue;
                    }
                }
            }
        }
    }
}