﻿using System.Collections.Generic;

namespace XD.SDK.Share {
    public class XDGQQShareParam : XDGBaseShareParam {
        public static readonly int Session = 0;
        public static readonly int Timeline = 1;

        public string LinkTitle { get; set; }

        public string LinkUrl { get; set; }

        public string LinkSummary { get; set; }

        public string VideoUrl { get; set; }

        public int SceneType { get; set; }

        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.QQ;

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(LinkTitle)) {
                dict["linkTitle"] = LinkTitle;
            }
            if (!string.IsNullOrWhiteSpace(LinkUrl)) {
                dict["linkUrl"] = LinkUrl;
            }
            if (!string.IsNullOrWhiteSpace(LinkSummary)) {
                dict["linkSummary"] = LinkSummary;
            }
            if (!string.IsNullOrWhiteSpace(VideoUrl)) {
                dict["videoUrl"] = VideoUrl;
            }

            dict["scene"] = SceneType;

            return dict;
        }
    }
}
