using System.Collections.Generic;

namespace XD.SDK.Common
{
    public interface XDGError
    {
        int code { get; }
        string error_msg { get; }
        //参考： https://stg-xdsdk.ap-sg.tdsapps.com/docs/account/account-server#taptap-%E7%BB%91%E5%AE%9A%E9%82%AE%E7%AE%B1%E6%9C%AA%E9%AA%8C%E8%AF%81
        Dictionary<string, object> extra_data { get; }

        string ToJSON();
    }
}