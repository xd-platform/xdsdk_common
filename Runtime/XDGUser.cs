using System.Collections.Generic;

namespace XD.SDK.Account
{
    public interface XDGUser
    {
        // The _userStandalone's _userStandalone ID.
        string userId { get; }

        string sub { get; }
        // The _userStandalone's _userStandalone name.
        string name { get; }

        string loginType //App传来的是字符串，如 TapTap。 通过 GetLoginType() 方法获取枚举
        {
            get;
        }

        string avatar { get; }

        string nickName { get; }

        List<string> boundAccounts { get; }

        // The _userStandalone's token.
        XDGAccessToken token { get; }

        XD.SDK.Account.LoginType getLoginType();
    }
}