using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XD.SDK.Common;
using XD.SDK.Common.Internal;
using XD.SDK.Announcement.Internal;

namespace XD.SDK.Announcement {
    public class XDGAnnouncementManager {
        private static readonly string CNHost = "https://xdsdk6-page-static.xdcdn.cn";
        private static readonly string GlobalHost = "https://xdsdk6-page-static.xdcdn.com";

        private static readonly string DetailAllEndpoint = "api/v1.0/gm/external/convenient/announcement/client/{client_id}/lang/{sdk_lang}";

        private static XDHttpClient client;

        private static XDHttpClient Client {
            get {
                if (client == null) {
                    string cachePath = Path.Combine(Application.persistentDataPath, "HttpCaches");
                    string host = XDConfigs.IsCN ? CNHost : GlobalHost;
                    client = XDHttpClientFactory.CreateClient(host, new HttpCacheHandler(cachePath));
                }
                return client;
            }
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        /// <returns></returns>
        public static async Task<List<XDGAnnouncement>> GetAnnouncements()
        {
            var queryParam = XDCommonHttpConfig.GetCommonQueryParams(GlobalHost, 0);
            var clientId = queryParam["clientId"];
            var sdkLang = queryParam["sdkLang"];
            var realPath = DetailAllEndpoint.Replace("{client_id}", clientId).Replace("{sdk_lang}", sdkLang);
            DetailAllResponse response = await Client.Get<DetailAllResponse>(realPath);
            DateTimeOffset? date = response.Headers.Date;
            if (date == null) {
                date = DateTimeOffset.Now;
            }
            return response.Announcements.Where(announcement =>
                DateTimeOffset.FromUnixTimeSeconds(announcement.PublishTime) <= date.Value &&
                date.Value <= DateTimeOffset.FromUnixTimeSeconds(announcement.ExpireTime))
                .ToList();
        }

        /// <summary>
        /// 获取公告列表（旧接口方式）
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="errorCallback"></param>
        public static async void GetAnnouncements(Action<List<XDGAnnouncement>> callback, Action<XDGError> errorCallback) {
            try {
                List<XDGAnnouncement> announcements = await GetAnnouncements();
                callback?.Invoke(announcements);
            } catch (XDException e) {
                errorCallback?.Invoke(e);
            } catch (Exception e) {
                errorCallback?.Invoke(XDException.MSG(e.Message));
            }
        }
    }
}