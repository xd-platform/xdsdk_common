using System;
using System.Collections.Generic;
using TapTap.Common;

namespace XD.SDK.Share
{
    public class WebPageData
    {
        //标题(必选)
        public string title;

        //摘要
        public string summary;

        //缩略图地址
        public string thumbUri;

        //点击后跳转链接(必选)
        public string url;

        public static string toJsonString(WebPageData data){
            if(data == null) return null;
            Dictionary<string,string> dictionary = new Dictionary<string, string>();
            dictionary.Add("title", data.title);
            dictionary.Add("summary", data.summary);
            dictionary.Add("thumbUri", data.thumbUri);
            dictionary.Add("url",data.url);
            return Json.Serialize(dictionary);
        }
    }
}