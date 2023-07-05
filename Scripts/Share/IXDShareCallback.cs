namespace XD.SDK.Share
{
    public interface IXDShareCallback
    {
        void OnSuccess();

        void OnFail(int code, string msg);

        void OnCancel();
    }
}