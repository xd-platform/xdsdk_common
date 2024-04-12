using XD.SDK.Common;

namespace XD.SDK.Payment.PC {
    public class XDGOrderInfoWrapperStandalone : XDGOrderInfoWrapper {
        public XDGError xdgError => Error;

        public XDGOrderInfo orderInfo => OrderInfo;

        public string debugMsg => DebugMsg;

        public XDGError Error { get; set; }

        public XDGOrderInfo OrderInfo { get; set; }

        public string DebugMsg { get; set; }
    }
}
