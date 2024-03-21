namespace XD.SDK.Account
{
    public enum LoginType
    {
        Guest = 0,    // 游客登录
        WeChat = 1,    // PC 未实现
        Apple = 2,    // 苹果登录
        Google = 3,    // Google 登录
        Facebook = 4,    // Facebook 登录
        TapTap = 5,    // Tap 登录
        LINE = 6,    // PC 未实现
        Twitter = 7,    // PC 未实现
        // QQ       = 8,
        // Twitch   = 9,
        Steam = 10,   // Steam 登录
        Phone = 11,   // 手机号 登陆
        Default = -1,   // 自动登录，以上次登录成功的信息登录
    }
    // ios的，要与后台一致！
    // XDGLoginInfoTypeGuest = 0,
    // XDGLoginInfoTypeWeChat = 1,
    // XDGLoginInfoTypeApple = 2,
    // XDGLoginInfoTypeGoogle = 3,
    // XDGLoginInfoTypeFacebook = 4,
    // XDGLoginInfoTypeTapTap = 5,
    // XDGLoginInfoTypeLine = 6,
    // XDGLoginInfoTypeTwitter = 7
}