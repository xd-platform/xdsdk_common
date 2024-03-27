using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Account.Editor
{
    public class XDAccountProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 5;
        
        public override string LinkPath => "XDSDK/Gen/Account/link.xml";
        
        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Account" },
        };
        
        public override Func<BuildReport, bool> IsTargetPlatform => (report) => true;
    }
}