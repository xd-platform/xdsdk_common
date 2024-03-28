using System.Collections.Generic;

namespace XD.SDK.Common.Editor {
    public class XDConfigModel {
        public string region_type { get; set; }
        public string bundle_id { get; set; }
        public string client_id { get; set; }
        public string app_id { get; set; }
        public bool idfa_enabled { get; set; }
        public string game_name { get; set; }
        public string report_url { get; set; }
        public string logout_url { get; set; }
        public string web_pay_url { get; set; }
        public Tapsdk tapsdk { get; set; }
        public List<string> logos { get; set; }
        public Firebase firebase { get; set; }
        public Facebook facebook { get; set; }
        public Line line { get; set; }
        public Twitter twitter { get; set; }
        public Google google { get; set; }
        public Adjust adjust { get; set; }
        public Appsflyer appsflyer { get; set; }
        public ADConfig ad_config { get; set; }
        public QQConfig qq { get; set; }
        public WeChatConfig wechat { get; set; }
        public WeiBoConfig weibo { get; set; }
        public XHSConfig xhs { get; set; }
        public DouYinConfig douyin { get; set; }
    }

    public class Adjust {
        public string app_token { get; set; }
        public List<Event> events { get; set; }
    }

    public class Appsflyer {
        public string app_id { get; set; }
        public string dev_key_android { get; set; }
        public string dev_key { get; set; }
        public string dev_key_ios { get; set; }
    }

    public class DbConfig {
        public bool enable { get; set; }
        public string channel { get; set; }
        public string game_version { get; set; }
    }

    public class Event {
        public string event_name { get; set; }
        public string token { get; set; }
    }

    public class Firebase {
        public bool enableTrack { get; set; }
    }

    public class Facebook {
        public string app_id { get; set; }
        public string client_token { get; set; }
        public List<string> permissions { get; set; }
    }

    public class Google {
        public string CLIENT_ID { get; set; }
        public string CLIENT_ID_FOR_ANDROID { get; set; }
    }

    public class Line {
        public string channel_id { get; set; }
    }

    public class Tapsdk {
        public string client_id { get; set; }
        public string client_token { get; set; }
        public string server_url { get; set; }
        public DbConfig db_config { get; set; }
        public List<string> permissions { get; set; }
    }

    public class Twitter {
        public string consumer_key { get; set; }
        public string consumer_secret { get; set; }
    }
    public class TTConfig {
        public string app_id { get; set; }
        public string app_name { get; set; }
    }

    public class GDTConfig {
        public string user_action_set_id { get; set; }
        public string app_secret_key { get; set; }
    }

    public class ADConfig {
        public TTConfig tt_config { get; set; }
        public GDTConfig gdt_config { get; set; }
    }

    public class QQConfig {
        public string app_id { get; set; }
    }

    public class WeChatConfig {
        public string app_id { get; set; }
    }

    public class WeiBoConfig {
        public string app_id { get; set; }
    }

    public class XHSConfig {
        public string app_id_ios { get; set; }

        public string app_id_android { get; set; }

        public string universal_link { get; set; }
    }

    public class DouYinConfig {
        public string app_id { get; set; }
    }
}