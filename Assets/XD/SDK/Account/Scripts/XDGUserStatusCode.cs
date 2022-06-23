namespace XD.SDK.Account
{
    public enum XDGUserStatusCodeType : int{
        // 登出
         LOGOUT = 0x9001,
        // 绑定
         BIND = 0x1001,
        // 解绑
         UNBIND = 0x1002,
        //出错了，未知code
         ERROR = -100
    }
}