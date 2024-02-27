using System.Collections.Generic;

namespace XD.SDK.Share {
    public class XDGXHSShareParam : XDGBaseShareParam {
        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.XHS;

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(Title)) {
                dict["Title"] = Title;
            }
            if (!string.IsNullOrWhiteSpace(VideoUrl)) {
                dict["videoUrl"] = VideoUrl;
            }

            return dict;
        }
    }
}
