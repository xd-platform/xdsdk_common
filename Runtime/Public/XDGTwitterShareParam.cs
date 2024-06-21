using System.Collections.Generic;

namespace XD.SDK.Share {
    public class XDGTwitterShareParam : XDGBaseShareParam {
        public string LinkUrl { get; set; }

        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.Twitter;

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(LinkUrl)) {
                dict["linkUrl"] = LinkUrl;
            }

            return dict;
        }
    }
}
