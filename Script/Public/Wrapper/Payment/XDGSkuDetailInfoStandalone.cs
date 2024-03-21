using System.Collections.Generic;
using XD.SDK.Common;

namespace XD.SDK.Payment.PC {
    public class XDGSkuDetailInfoStandalone : XDGSkuDetailInfo {
        public XDGError xdgError => Error;

        public List<SkuDetailBean> skuDetailList => SkuDetailList;

        public XDGError Error { get; set; }

        public List<SkuDetailBean> SkuDetailList { get; set; }
    }
}
