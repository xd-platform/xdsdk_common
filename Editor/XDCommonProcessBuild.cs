using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Common.Editor
{
    public class XDCommonProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 5;
        
        public override string LinkPath => "XDSDK/Gen/Common/link.xml";
        
        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Common" },
        };
        
        public override Func<BuildReport, bool> IsTargetPlatform => (report) => true;
    }
}