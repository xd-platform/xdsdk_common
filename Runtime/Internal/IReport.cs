using System;

namespace XD.SDK.Report.Internal
{
    public interface IReport
    {
        void Report(Action<bool> resultAction);
    }
}