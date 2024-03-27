using System;
using System.Text;

namespace XD.SDK.Common {
    public class XDGLogger {
        /// <summary>
        /// Configures the logger.
        /// </summary>
        /// <value>The log delegate.</value>
        public static Action<XDGLogLevel, string> LogDelegate {
            get; set;
        }

        public static void Debug(string log) {
            LogDelegate?.Invoke(XDGLogLevel.Debug, log);
        }

        public static void Debug(string format, params object[] args) {
            LogDelegate?.Invoke(XDGLogLevel.Debug, string.Format(format, args));
        }

        public static void Warn(string log) {
            LogDelegate?.Invoke(XDGLogLevel.Warn, log);
        }

        public static void Warn(string format, params object[] args) {
            LogDelegate?.Invoke(XDGLogLevel.Warn, string.Format(format, args));
        }

        public static void Error(string log) {
            LogDelegate?.Invoke(XDGLogLevel.Error, log);
        }

        public static void Error(string format, params object[] args) {
            LogDelegate?.Invoke(XDGLogLevel.Error, string.Format(format, args));
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