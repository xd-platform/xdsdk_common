using XD.SDK.Common.Internal;
using XD.SDK.Common.PC.Internal;

namespace XD.SDK.Common {
    public class XDGConfigsPC : IXDConfigs {
        public bool IsCN => !ConfigModule.IsGlobal;
    }
}