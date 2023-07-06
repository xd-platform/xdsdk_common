#if UNITY_EDITOR || UNITY_STANDALONE
using System.Collections.Generic;
using XD.SDK.Common;
using XD.SDK.Common.PC;
using XD.SDK.Payment;

namespace XD.SDK.Payment.PC
{
    public class XDGRefundResultWrapperClass : XDGRefundResultWrapper
    {
        public XDGError xdgError { get; private set; }
        public List<XDGRefundDetails> refundList { get; private set; }

        public XDGRefundResultWrapperClass(List<XDGRefundDetails> refundList, XDGError xdgError)
        {
            this.refundList = refundList;
            this.xdgError = xdgError;
        }
    }
}
#endif