using System.Collections.Generic;

namespace XD.SDK.Share
{
    public class XDGTapTapShareParam : XDGBaseShareParam
    {
        public string AppId { get; set; }
        
        public string GroupLabelId { get; set; }
        
        public string HashtagIds { get; set; }
        
        public string Title { get; set; }
        
        public string FailUrl { get; set; }
        public override XDGSharePlatformType SharePlatformType => XDGSharePlatformType.TapTap;
        
        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> dict = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(AppId)) {
                dict["tapAppId"] = AppId;
            }

            if (!string.IsNullOrWhiteSpace(GroupLabelId))
            {
                dict["tapGroupLabelId"] = GroupLabelId;
            }
            
            if (!string.IsNullOrWhiteSpace(HashtagIds))
            {
                dict["tapHashtagIds"] = HashtagIds;
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                dict["title"] = Title;
            }
            
            if (!string.IsNullOrWhiteSpace(FailUrl))
            {
                dict["failUrl"] = FailUrl;
            }

            return dict;
        }
    }
}