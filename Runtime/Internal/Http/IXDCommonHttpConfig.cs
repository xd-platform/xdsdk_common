using System.Collections.Generic;

namespace XD.SDK.Common.Internal {
    public interface IXDCommonHttpConfig {
        Dictionary<string, string> GetCommonQueryParams(string url, long timestamp);
    }
}
