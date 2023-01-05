using System.Collections.Generic;

namespace XD.SDK.Common.Internal
{
    public interface IXDGError
    {
        int code { get; }
        string error_msg { get; }
        Dictionary<string, object> extra_data { get; }
    }
}