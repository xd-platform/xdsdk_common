using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Share.Mobile.Editor {
    public class XDShareMobileProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 8;

        public override string LinkPath => "XDSDK/Gen/Share/Mobile/link.xml";

        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Share.Mobile" },
        };

        public override Func<BuildReport, bool> IsTargetPlatform => (report) => true;
    }
}
