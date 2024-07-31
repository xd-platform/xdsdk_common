using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Account.Editor
{
    public class XDAccountMobileProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 6;
        
        public override string LinkPath => "XDSDK/Gen/Account/Mobile/link.xml";
        
        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Account.Mobile" }
        };
        
        public override Func<BuildReport, bool> IsTargetPlatform => (report) => {
            return BuildTargetUtils.IsSupportMobile(report.summary.platform);
        };
    }
}