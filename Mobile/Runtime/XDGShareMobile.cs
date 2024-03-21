using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using LC.Newtonsoft.Json;
using XD.SDK.Common;
using XD.SDK.Share.Internal;
using TapTap.Common;

namespace XD.SDK.Share.Mobile {
    public class XDGShareMobile : IXDSharePlatform {
        private static readonly string SHARE_SERVICE_NAME = "com.xd.sharing.mix.unitybridge.XDGSharingService";
        private static readonly string SHARE_SERVICE_IMPL = "com.xd.sharing.mix.unitybridge.XDGSharingServiceImpl";

        private static readonly string MOBILE_SHARE_SERVICE = "XDGSharingService";

        public XDGShareMobile() {
            EngineBridge.GetInstance()
                .Register(SHARE_SERVICE_NAME, SHARE_SERVICE_IMPL);
        }

        public void Share(XDGBaseShareParam platformParam, XDGShareCallback callback) {
            Dictionary<string, object> dict = platformParam.ToDictionary();
            // 保证参数完整有序
            Dictionary<string, object> args = new Dictionary<string, object> {
                { "shareWithType", dict.TryGetValue("shareWithType", out object shareWithType) ? shareWithType : 0 },
                { "scene", dict.TryGetValue("scene", out object scene) ? scene : 0 },
                { "contentText", dict.TryGetValue("contentText", out object contentText) ? contentText : string.Empty },
                { "videoUrl", dict.TryGetValue("videoUrl", out object videoUrl) ? videoUrl : string.Empty },
                { "imageUrl", dict.TryGetValue("imageUrl", out object imageUrl) ? imageUrl : string.Empty },
                { "imageData", dict.TryGetValue("imageData", out object imageData) ? imageData : string.Empty },
                { "title", dict.TryGetValue("title", out object title) ? title : string.Empty },
                { "linkUrl", dict.TryGetValue("linkUrl", out object linkUrl) ? linkUrl : string.Empty },
                { "linkTitle", dict.TryGetValue("linkTitle", out object linkTitle) ? linkTitle : string.Empty },
                { "linkSummary", dict.TryGetValue("linkSummary", out object linkSummary) ? linkSummary : string.Empty },
            };
            Command command = new Command.Builder()
                .Service(MOBILE_SHARE_SERVICE)
                .Method("shareWithType")
                .Args(args)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGLogger.Debug("Share 方法结果: " + result.ToJSON());
                if (result.code == Result.RESULT_SUCCESS) {
                    ShareBridgeResponse response = JsonConvert.DeserializeObject<ShareBridgeResponse>(result.content);
                    if (response.IsSuccess) {
                        callback?.OnSuccess?.Invoke();
                    } else if (response.IsCancelled) {
                        callback?.OnCancel?.Invoke();
                    } else {
                        callback?.OnFailed?.Invoke(response.Code, response.Message);
                    }
                } else {
                    callback?.OnFailed?.Invoke(result.code, result.message);
                }
            });
        }

        public bool IsAppInstalled(XDGSharePlatformType platformType) {
#if UNITY_ANDROID
            using (AndroidJavaClass ShareBridgeClass = new AndroidJavaClass("com.xd.share.bridge.XDGShareBridge")) {
                return ShareBridgeClass.CallStatic<bool>("isAppInstalled", (int)platformType);
            }
#elif UNITY_IOS
            return XDShareBridgeIsAppInstalled((int)platformType);
#else
            return false;
#endif
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern bool XDShareBridgeIsAppInstalled(int platformType);
#endif
    }
}
