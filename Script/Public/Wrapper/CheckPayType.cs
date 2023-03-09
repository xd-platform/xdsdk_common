#if UNITY_EDITOR || UNITY_STANDALONE
namespace XD.SDK.Common.PC.Internal {
    public enum CheckPayType {
        iOS,      //只有iOS需要补款
        Android,  //只有Android需要补款
        iOSAndAndroid,      //iOS Android 都需要补款
        None,     //没有要补款
    }
}
#endif