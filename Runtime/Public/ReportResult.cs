using XD.SDK.Common.Internal;

namespace XD.SDK.Report
{
    public class ReportResult
    {
        public const int CodeParamInvalid = -1; // 无效参数
        public const int CodeUserNotLoggedIn = -2; // 用户未登录
        public const int CodeFileNotExist = -3; // 文件不存在
        public bool Success { get; }
        public XDException XdException { get; }

        public ReportResult(bool success, XDException exception)
        {
            Success = success;
            XdException = exception;
        }

        public override string ToString()
        {
            return $"ReportResult: Success={Success} XdException={XdException}";
        }
    }
}