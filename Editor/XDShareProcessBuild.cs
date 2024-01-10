using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Share.Editor
{
    public class XDShareProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 5;
        
        public override string LinkPath => "XDSDK/Gen/Mainland/link.xml";
        
        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Share" },
        };
        
        public override Func<BuildReport, bool> IsTargetPlatform => (report) => true;
    }
}