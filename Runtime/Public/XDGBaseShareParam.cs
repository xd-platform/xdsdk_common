using System;
using System.Collections.Generic;

namespace XD.SDK.Share {
    public abstract class XDGBaseShareParam {
        public string ContentText { get; set; }

        public string ImageUrl { get; set; }

        public byte[] ImageData { get; set; }

        public abstract XDGSharePlatformType SharePlatformType { get; }

        public virtual Dictionary<string, object>  ToDictionary() {
            Dictionary<string, object> dict = new Dictionary<string, object> {
                { "shareWithType", (int) SharePlatformType }
            };
            if (!string.IsNullOrWhiteSpace(ContentText)) {
                dict["contentText"] = ContentText;
            }
            if (!string.IsNullOrWhiteSpace(ImageUrl)) {
                dict["imageUrl"] = ImageUrl;
            }
            if (ImageData != null) {
                dict["imageData"] = BytesToBase64(ImageData);
            }
            return dict;
        }

        protected static string BytesToBase64(byte[] bytes) {
            if (bytes == null) {
                return null;
            }

            string base64String = Convert.ToBase64String(bytes);
            return base64String;
        }
    }
}
