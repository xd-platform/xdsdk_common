#if UNITY_STANDALONE || UNITY_EDITOR
using UnityEngine.Scripting;
using XD.SDK.Account.Internal;
using XD.SDK.Common.PC.Internal;

namespace XD.SDK.Account
{
    [Preserve]
    public class UserManagerPC: IUserManagerPlatformWrapper
    {
        public  XDGUser GetCurrentUser()
        {
            return UserModule.GetCurrentUser();
        }
    }
}
#endif