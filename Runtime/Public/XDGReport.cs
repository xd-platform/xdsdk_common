using System;

namespace XD.SDK.Report
{
    public class XDGReport
    {
        public static void Report(ReportParams reportParams, Action<ReportResult> resultCallback)
        {
            XDReportImpl.GetInstance().Report(reportParams, resultCallback);
        }
    }
}