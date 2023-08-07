namespace XD.SDK.Common
{
    public interface XDGShareCallback
    {
        void ShareSuccess();
        void ShareCancel();
        void ShareFailed(string error);
    }
}