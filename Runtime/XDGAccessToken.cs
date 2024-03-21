namespace XD.SDK.Account
{
    public interface XDGAccessToken
    {
        // 唯一标志
        string kid { get; }

        // 认证码类型
        string tokenType { get; }

        // mac密钥
        string macKey { get; }

        // mac密钥计算方式
        string macAlgorithm { get; }
        
    }
}