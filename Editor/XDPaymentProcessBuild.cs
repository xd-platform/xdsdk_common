using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Payment.Editor
{
    public class XDPaymentProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 5;
        
        public override string LinkPath => "XDSDK/Gen/Payment/link.xml";
        
        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Payment" },
        };
        
        public override Func<BuildReport, bool> IsTargetPlatform => (report) => true;
    }
}