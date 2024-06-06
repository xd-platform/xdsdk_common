#if UNITY_EDITOR || UNITY_STANDALONE
namespace XD.SDK.Common.PC.Internal {
    public enum BindType {
        None = -1,
        Bind = 0,   //绑定
        UnBind = 1  //未绑定
    }
}
#endif