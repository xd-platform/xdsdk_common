using System.Collections.Generic;
using LC.Newtonsoft.Json;
using XD.SDK.Common.Internal;

namespace XD.SDK.Announcement.Internal {
    public class DetailAllResponse : BaseResponse {
        [JsonProperty("data")]
        public List<XDGAnnouncement> Announcements { get; set; }
    }
}