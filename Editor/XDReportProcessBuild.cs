using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Report.Editor
{
    public class XDReportProcessBuild : SDKLinkProcessBuild
    {
        public override int callbackOrder => 9;
        public override string LinkPath => "XDSDK/Gen/Report/link.xml";

        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[]
        {
            new LinkedAssembly { Fullname = "XD.SDK.Report" }
        };

        public override Func<BuildReport, bool> IsTargetPlatform => (report) => true;
    }
}