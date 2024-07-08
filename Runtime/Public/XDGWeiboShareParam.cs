using System.Collections.Generic;

namespace XD.SDK.Share {
    public class XDGWeiboShareParam : XDGBaseShareParam {
        public string VideoUrl { get; set; }
        
        public string SuperGroupName { get; set; }
        
        public string SuperGroupSection { get; set; }
        
        public string SuperGroupExtras { get; set; }

        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.Weibo;

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(VideoUrl)) {
                dict["videoUrl"] = VideoUrl;
            }

            if (!string.IsNullOrWhiteSpace(SuperGroupName))
            {
                dict["superGroupName"] = SuperGroupName;
            }
            
            if (!string.IsNullOrWhiteSpace(SuperGroupSection))
            {
                dict["superGroupSection"] = SuperGroupSection;
            }

            if (!string.IsNullOrWhiteSpace(SuperGroupExtras))
            {
                dict["superGroupExtras"] = SuperGroupExtras;
            }

            return dict;
        }
    }
}
