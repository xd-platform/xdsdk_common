using System.Collections.Generic;

namespace XD.SDK.Share {
    public class XDGLineShareParam : XDGBaseShareParam {
        public string LinkUrl { get; set; }

        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.Line;

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(LinkUrl)) {
                dict["linkUrl"] = LinkUrl;
            }

            return dict;
        }
    }
}
