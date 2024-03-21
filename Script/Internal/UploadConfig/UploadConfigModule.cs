using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XD.SDK.Common.PC.Internal {
    public class UploadConfigModule {
        private static bool needUploadConfig = false;

        public static void TriggerUploadConfig() {
            needUploadConfig = true;
            UIManager.ShowToast("已开启参数上报");
        }

        public static async Task TryUploadConfig() {
            if (!needUploadConfig) {
                return;
            }

            try {
                string path = "api/v1/config/upload";
                Dictionary<string, object> xdconfigData = new Dictionary<string, object> {
                    { "app_id", ConfigModule.AppId },
                    { "client_id", ConfigModule.ClientId },
                    { "report_url", ConfigModule.ReportUrl },
                    { "logout_url", ConfigModule.CancelUrl },
                    { "region_type", ConfigModule.RegionType },
                    { "game_name", ConfigModule.GameName },
                    { "tapsdk", ConfigModule.TapConfig },
                    { "apple", ConfigModule.Apple },
                    { "google", ConfigModule.Google },
                    { "facebook", ConfigModule.Facebook },
                };
                Dictionary<string, object> configData = new Dictionary<string, object> {
                    { "xdConfig", xdconfigData }
                };
                await XDHttpClient.Post<BaseResponse>(path, data: configData);
                UIManager.ShowToast("参数上报成功");
            } catch (Exception) {
                UIManager.ShowToast("参数上报失败");
            }
        }
    }
}