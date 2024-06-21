using System;
using System.Collections.Generic;
using TapTap.Common;

namespace XD.SDK.Payment
{
    public interface XDGRefundDetails
    {
        string tradeNo { get; }
        string productId { get; }
        string currency { get; }
        string outTradeNo { get; }
        double refundAmount { get; }
        int supplyStatus { get; }
        /// <summary>
        /// 1- iOS;2-Android;3-Web;4-macOS;5-Windows;6-unknown
        /// </summary>
        int platform { get; }
        int channelType { get; }
    }
}