using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.ADSubPackage.Editor
{
    public class XDADSubPackageMobileProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 6;
        
        public override string LinkPath => "XDSDK/Gen/ADSub/link.xml";
        
        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.ADSubPackage" }
        };
        
        public override Func<BuildReport, bool> IsTargetPlatform => (report) => {
            return BuildTargetUtils.IsSupportMobile(report.summary.platform);
        };
    }
}