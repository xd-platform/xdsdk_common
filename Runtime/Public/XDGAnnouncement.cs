using System.Collections.Generic;
using LC.Newtonsoft.Json;

public class XDGAnnouncement {
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("shortTitle")]
    public string ShortTitle { get; set; }

    [JsonProperty("longTitle")]
    public string LongTitle { get; set; }

    [JsonProperty("publishTime")]
    public long PublishTime { get; set; }

    [JsonProperty("expireTime")]
    public long ExpireTime { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("dimensions")]
    public List<Dictionary<string, List<string>>> Dimensions { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}
