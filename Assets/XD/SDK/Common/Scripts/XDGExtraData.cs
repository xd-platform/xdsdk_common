using System;
using System.Collections.Generic;

namespace XD.SDK.Common{
    [Serializable]
    public class XDGExtraData{
        public string loginType { get; set; }
        public string email { get; set; }
        public List<Conflict> conflicts { get; set; }
        
        [Serializable]
        public class Conflict{
            public string loginType{ get; set; }
            public string userId{ get; set; }
        }
    }
}