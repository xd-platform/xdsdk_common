using System;

namespace XD.SDK.Share {
    public class XDGShareCallback {
        public Action OnSuccess { get; set; }
        public Action<int, string> OnFailed { get; set; }
        public Action OnCancel { get; set; }
    }
}