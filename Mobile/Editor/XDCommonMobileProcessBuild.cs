using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Common.Mobile.Editor
{
    public class XDCommonMobileProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 6;
        
        public override string LinkPath => "XDSDK/Gen/Common/Mobile/link.xml";
        
        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Common.Mobile.Runtime" }
        };
        
        public override Func<BuildReport, bool> IsTargetPlatform => (report) => {
            return BuildTargetUtils.IsSupportMobile(report.summary.platform);
        };
    }
}