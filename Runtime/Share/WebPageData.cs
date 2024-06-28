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

        //缩略图数据, 当缩略图地址同时存在时优先使用缩略图数据
        public byte[] thumbData;

        //点击后跳转链接(必选)
        public string url;

        public static string toJsonString(WebPageData data){
            if(data == null) return "";
            Dictionary<string,string> dictionary = new Dictionary<string, string>();
            if(data.title != null){
                dictionary.Add("title", data.title);
            }
            if(data.summary != null){
                dictionary.Add("summary", data.summary);
            }
            if(data.thumbUri != null){
                dictionary.Add("thumbUri", data.thumbUri);
            }
            if(data.thumbData != null && data.thumbData.Length > 0){
                dictionary.Add("thumbData", Convert.ToBase64String(data.thumbData));
            }
            if(data.url != null){
                dictionary.Add("url",data.url);
            }
            return Json.Serialize(dictionary);
        }
    }
}