using System;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine;
using XD.SDK.Common;

namespace XD.SDK.Share{
    public class XDShareMobileImpl{

        //Share
        private static string SHARE_SERVICE_NAME = "com.xd.share.cn.unityBridge.XDShareService";
        private static string SHARE_SERVICE_IMPL = "com.xd.share.cn.unityBridge.XDShareServiceImpl";

        private readonly string XD_SHARE_SERVICE = "XDShareService"; //注意要和iOS本地桥接类名一样

        private XDShareMobileImpl(){
            EngineBridge.GetInstance()
                .Register(SHARE_SERVICE_NAME, SHARE_SERVICE_IMPL);
        }

        private static volatile XDShareMobileImpl _instance;
        private static readonly object Locker = new object();

        public static XDShareMobileImpl GetInstance(){
            lock (Locker){
                if (_instance == null){
                    _instance = new XDShareMobileImpl();
                }
            }

            return _instance;
        }


        public void ShareText(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string text, IXDShareCallback callback){
            text = (text == null) ? "" : text;
            var dic = new Dictionary<string, object>{
                {"shareTextWithTarget", (int)target},
                {"scene",(int) scene},
                {"text",text}
            };
            var command = new Command.Builder()
                .Service(XD_SHARE_SERVICE)
                .Method("shareTextWithTarget")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGLogger.Debug("shareText 方法结果: " + result.ToJSON());
                convertShareCallback(result, callback);
            });
        }

        public void ShareImage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, string imageUri, IXDShareCallback callback){
            imageUri = (imageUri == null) ? "" : imageUri;
            var dic = new Dictionary<string, object>{
                {"shareImageWithTarget", (int)target},
                {"scene",(int) scene},
                {"image",imageUri}
            };
            var command = new Command.Builder()
                .Service(XD_SHARE_SERVICE)
                .Method("shareImageWithTarget")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGLogger.Debug("shareImage 方法结果: " + result.ToJSON());
                convertShareCallback(result, callback);
            });
        }

        public void ShareImage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, byte[] imageData, IXDShareCallback callback){
            if(imageData == null){
                XDGLogger.Debug("shareImage data is null");
            }else{
                XDGLogger.Debug("shareImage data length = " + imageData.Length);
            }
            var dic = new Dictionary<string, object>{
                {"shareImageDataWithTarget", (int)target},
                {"scene",(int) scene},
                {"imageData",(imageData == null ? "" : Convert.ToBase64String(imageData))}
            };
            var command = new Command.Builder()
                .Service(XD_SHARE_SERVICE)
                .Method("shareImageDataWithTarget")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGLogger.Debug("shareImage 方法结果: " + result.ToJSON());
                convertShareCallback(result, callback);
            });
        }

        public void ShareWebPage(ShareConstants.ShareTarget target, ShareConstants.ShareScene scene, WebPageData data, IXDShareCallback callback){
            var dic = new Dictionary<string, object>{
                {"shareWebPageWithTarget", (int)target},
                {"scene",(int) scene},
                {"webPageConfig",WebPageData.toJsonString(data)}
            };
            var command = new Command.Builder()
                .Service(XD_SHARE_SERVICE)
                .Method("shareWebPageWithTarget")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGLogger.Debug("shareWebPage 方法结果: " + result.ToJSON());
                convertShareCallback(result, callback);
            });
        }

        public void IsTargetInstalled(ShareConstants.ShareTarget target, Action<bool> callback){
             var dic = new Dictionary<string, object>{
                {"isTargetInstalled", (int)target}
            };
             var command = new Command.Builder()
                .Service(XD_SHARE_SERVICE)
                .Method("isTargetInstalled")
                .Args(dic)
                .Callback(true)
                .OnceTime(true)
                .CommandBuilder();

            EngineBridge.GetInstance().CallHandler(command, result => {
                XDGLogger.Debug("isTargetInstalled 方法结果: " + result.ToJSON());
                if (XDGTool.checkResultSuccess(result)){
                    var contentDic = Json.Deserialize(result.content) as Dictionary<string, object>;
                    var success = SafeDictionary.GetValue<bool>(contentDic, "success");
                    callback(success);
                }else{
                    callback(false);
                }
            });
        }

        private void convertShareCallback(Result result, IXDShareCallback callback){
            if(callback == null) return;
            if (XDGTool.checkResultSuccess(result)){
                ShareCallbackWrapper wrapper = new ShareCallbackWrapper(result.content);
                if(wrapper.isError){
                    callback.OnFail(wrapper.errorCode, wrapper.errorMsg);
                }else{
                    if(wrapper.isSuccess){
                        callback.OnSuccess();
                    }else{
                        callback.OnCancel();
                    }
                }
            }else{
                callback.OnFail((int)ShareConstants.ShareErrorCode.OTHER, "parse result failed");
            }

        }
      
    }
}