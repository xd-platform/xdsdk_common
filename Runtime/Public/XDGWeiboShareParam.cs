using System.Collections.Generic;

namespace XD.SDK.Share {
    public class XDGWeiboShareParam : XDGBaseShareParam {
        public string VideoUrl { get; set; }

        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.Weibo;

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(VideoUrl)) {
                dict["videoUrl"] = VideoUrl;
            }

            return dict;
        }
    }
}
