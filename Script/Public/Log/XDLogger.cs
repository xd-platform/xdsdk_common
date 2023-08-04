#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Text;

namespace XD.SDK.Common.PC.Internal {
    public class XDLogger {
        /// <summary>
        /// Configures the logger.
        /// </summary>
        /// <value>The log delegate.</value>
        public static Action<XDLogLevel, string> LogDelegate {
            get; set;
        }

        public static void Debug(string log) {
            LogDelegate?.Invoke(XDLogLevel.Debug, log);
        }

        public static void Debug(string format, params object[] args) {
            LogDelegate?.Invoke(XDLogLevel.Debug, string.Format(format, args));
        }

        public static void Warn(string log) {
            LogDelegate?.Invoke(XDLogLevel.Warn, log);
        }

        public static void Warn(string format, params object[] args) {
            LogDelegate?.Invoke(XDLogLevel.Warn, string.Format(format, args));
        }

        public static void Error(string log) {
            LogDelegate?.Invoke(XDLogLevel.Error, log);
        }

        public static void Error(string format, params object[] args) {
            LogDelegate?.Invoke(XDLogLevel.Error, string.Format(format, args));
        }

        public static void Error(Exception e) {
            StringBuilder sb = new StringBuilder();
            sb.Append(e.GetType());
            sb.Append("\n");
            sb.Append(e.Message);
            sb.Append("\n");
            sb.Append(e.StackTrace);
            Error(sb.ToString());
        }
    }
}
#endif
