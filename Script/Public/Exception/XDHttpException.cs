using System.Collections.Generic;

namespace XD.SDK.Common.PC.Internal {
    public class XDHttpException : XDException {
        public int HttpStatusCode { get; set; }

        public XDHttpException(int httpStatusCode, int code, string message) :
            base(code, message) {
            HttpStatusCode = httpStatusCode;
        }

        public XDHttpException(int httpStatusCode, int code, string message, Dictionary<string, object> extraData) :
            base(code, message, extraData) {
            HttpStatusCode = httpStatusCode;
        }

        public override string ToString() {
            return $"{HttpStatusCode} - {code} - {base.Message}";
        }
    }
}