#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Text;
using XD.SDK.Common;

namespace XD.SDK.Common.PC.Internal {
    public class XDLogger {
        /// <summary>
        /// Configures the logger.
        /// </summary>
        /// <value>The log delegate.</value>
        public static Action<XDLogLevel, string> LogDelegate {
            set {
                XDGLogger.LogDelegate = (level, message) => {
                    switch (level) {
                        case XDGLogLevel.Debug:
                            value?.Invoke(XDLogLevel.Debug, message);
                            break;
                        case XDGLogLevel.Warn:
                            value?.Invoke(XDLogLevel.Warn, message);
                            break;
                        case XDGLogLevel.Error:
                            value?.Invoke(XDLogLevel.Error, message);
                            break;
                        default:
                            break;
                    }
                };
            }
        }

        public static void Debug(string log) {
            XDGLogger.Debug(log);
        }

        public static void Debug(string format, params object[] args) {
            XDGLogger.Debug(format, args);
        }

        public static void Warn(string log) {
            XDGLogger.Warn(log);
        }

        public static void Warn(string format, params object[] args) {
            XDGLogger.Warn(format, args);
        }

        public static void Error(string log) {
            XDGLogger.Error(log);
        }

        public static void Error(string format, params object[] args) {
            XDGLogger.Error(format, args);
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
