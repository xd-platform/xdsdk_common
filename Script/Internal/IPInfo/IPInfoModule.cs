#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using LC.Newtonsoft.Json;
using LeanCloud.Common;

namespace XD.SDK.Common.PC.Internal {
    internal class IPInfoModule {
        //IP信息
        private readonly static string IP_INFO = "https://ip.xindong.com/myloc2";

        private static readonly Persistence persistence = new Persistence(Path.Combine(Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "ip_info"));

        private static IPInfo local;

        private static HttpClient client;

        internal static HttpClient Client {
            get {
                if (client == null) {
                    client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(10);
                }
                return client;
            }
        }

        internal static async Task SaveToLocal(IPInfo ipInfo) {
            await persistence.Save(ipInfo);
            local = ipInfo;
        }

        internal static async Task<IPInfo> GetLocalIPInfo() {
            if (local == null) {
                local = await persistence.Load<IPInfo>();
            }
            return local;
        }

        internal static async Task<IPInfo> RequestIpInfo() {
            try {
                HttpResponseMessage response = await Client.GetAsync(IP_INFO);
                string resultString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode) {
                    IPInfo ret = JsonConvert.DeserializeObject<IPInfo>(resultString,
                        LCJsonConverter.Default);
                    return ret;
                }
                throw new XDException((int)response.StatusCode, resultString);
            } catch (Exception e) {
                XDGLogger.Warn($"Request IP info error: {e}");
                throw e;
            }
        }

        internal static async Task<IPInfo> GetIpInfo() {
            try {
                IPInfo ipInfo = await RequestIpInfo();
                return ipInfo;
            } catch (Exception e) {
                XDGLogger.Warn($"Get IP error: {e}");
                IPInfo ipInfo = await IPInfoModule.GetLocalIPInfo();
                return ipInfo;
            }
        }
    }
}
#endif
