using System.Collections.Generic;

namespace XD.SDK.Common.Editor{
    public class XDGConfigModel{
    public string region_type{ get; set; }
    public string client_id{ get; set; }
    public string app_id{ get; set; }
    public string bundle_id{ get; set; }
    public bool idfa_enabled{ get; set; }
    public string game_name{ get; set; }
    public string report_url{ get; set; }
    public string logout_url{ get; set; }
    public string webpay_url{ get; set; }
    public Tapsdk tapsdk{ get; set; }
    public List<string> logos{ get; set; }
    public Facebook facebook{ get; set; }
    public Line line{ get; set; }
    public Twitter twitter{ get; set; }
    public Google google{ get; set; }
    public Adjust adjust{ get; set; }
    public Appsflyer appsflyer{ get; set; }
}

public class Adjust{
    public string app_token{ get; set; }
    public List<Event> events{ get; set; }
}

public class Appsflyer{
    public string app_id{ get; set; }
    public string dev_key{ get; set; }
}

public class DbConfig{
    public bool enable{ get; set; }
    public string channel{ get; set; }
}

public class Event{
    public string event_name{ get; set; }
    public string token{ get; set; }
}

public class Facebook{
    public string app_id{ get; set; }
    public string client_token{ get; set; }
    public List<string> permissions{ get; set; }
}

public class Google{
    public string CLIENT_ID{ get; set; }
    public string REVERSED_CLIENT_ID{ get; set; }
    public string API_KEY{ get; set; }
    public string GCM_SENDER_ID{ get; set; }
    public int PLIST_VERSION{ get; set; }
    public string BUNDLE_ID{ get; set; }
    public string PROJECT_ID{ get; set; }
    public string STORAGE_BUCKET{ get; set; }
    public bool IS_ADS_ENABLED{ get; set; }
    public bool IS_ANALYTICS_ENABLED{ get; set; }
    public bool IS_APPINVITE_ENABLED{ get; set; }
    public bool IS_GCM_ENABLED{ get; set; }
    public bool IS_SIGNIN_ENABLED{ get; set; }
    public string GOOGLE_APP_ID{ get; set; }
    public string DATABASE_URL{ get; set; }
}

public class Line{
    public string channel_id{ get; set; }
}

public class Tapsdk{
    public string client_id{ get; set; }
    public string client_token{ get; set; }
    public string server_url{ get; set; }
    public DbConfig db_config{ get; set; }
    public List<string> permissions{ get; set; }
}

public class Twitter{
    public string consumer_key{ get; set; }
    public string consumer_secret{ get; set; }
}
}