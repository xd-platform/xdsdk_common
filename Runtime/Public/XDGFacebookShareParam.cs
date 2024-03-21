using System.Collections.Generic;

namespace XD.SDK.Share {
    public class XDGFacebookShareParam : XDGBaseShareParam {
        public string LinkUrl { get; set; }

        public string LinkSummary { get; set; }

        public string VideoUrl { get; set; }

        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.Facebook;

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(LinkUrl)) {
                dict["linkUrl"] = LinkUrl;
            }
            if (!string.IsNullOrWhiteSpace(LinkSummary)) {
                dict["linkSummary"] = LinkSummary;
            }
            if (!string.IsNullOrWhiteSpace(VideoUrl)) {
                dict["videoUrl"] = VideoUrl;
            }

            return dict;
        }
    }
}
