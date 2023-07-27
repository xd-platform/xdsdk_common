namespace XD.SDK.Share
{
    public interface IXDShareCallback
    {
        void OnSuccess();

        void OnFail(int code, string msg);

        void OnCancel();
    }
}

// File google-services.json is missing. The Google Services Plugin cannot function without it.  See the Console for details.