using System;
using TapTap.Common.Editor;
using UnityEditor.Build.Reporting;

namespace XD.SDK.Announcement.Editor {
    public class XDAnnouncementProcessBuild : SDKLinkProcessBuild {
        public override int callbackOrder => 9;

        public override string LinkPath => "XDSDK/Gen/Announcement/link.xml";

        public override LinkedAssembly[] LinkedAssemblies => new LinkedAssembly[] {
            new LinkedAssembly { Fullname = "XD.SDK.Announcement" }
        };

        public override Func<BuildReport, bool> IsTargetPlatform => (report) => true;
    }
}