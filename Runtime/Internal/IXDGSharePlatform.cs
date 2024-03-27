
namespace XD.SDK.Share.Internal {
    public interface IXDSharePlatform {
        void Share(XDGBaseShareParam shareParam, XDGShareCallback callback);
        bool IsAppInstalled(XDGSharePlatformType platformType);
    }
}
