using System.Collections.Generic;
using XD.SDK.Common;

namespace XD.SDK.Payment
{
    public interface XDGRefundResultWrapper
    {
        XDGError xdgError { get; }
        List<XDGRefundDetails> refundList { get; }
    }
}