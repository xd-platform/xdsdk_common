#if XD_STEAM_SUPPORT

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Steamworks;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    public class SteamWrapper : ISteamSDKWrapper {
        private readonly Callback<GetAuthSessionTicketResponse_t> getAuthSessionTicketResponseCallback;
        private readonly ConcurrentDictionary<HAuthTicket, Tuple<TaskCompletionSource<string>, string>> tickTasks;

        private readonly Callback<MicroTxnAuthorizationResponse_t> mircoTxnAuthorizationCallback;
        private readonly ConcurrentDictionary<ulong, TaskCompletionSource<string>> microTxnTasks;

        public SteamWrapper() {
            tickTasks = new ConcurrentDictionary<HAuthTicket, Tuple<TaskCompletionSource<string>, string>>();

            getAuthSessionTicketResponseCallback = Callback<GetAuthSessionTicketResponse_t>.Create((GetAuthSessionTicketResponse_t pCallback) => {
                HAuthTicket authTicket = pCallback.m_hAuthTicket;
                if (tickTasks.TryRemove(authTicket, out Tuple<TaskCompletionSource<string>, string> callback)) {
                    callback.Item1.TrySetResult(callback.Item2);
                }
            });

            microTxnTasks = new ConcurrentDictionary<ulong, TaskCompletionSource<string>>();

            mircoTxnAuthorizationCallback = Callback<MicroTxnAuthorizationResponse_t>.Create((MicroTxnAuthorizationResponse_t pCallback) => {
                // 根据 pCallback.m_ulOrderID 查找对应的回调
                ulong orderId = pCallback.m_ulOrderID;
                UnityEngine.Debug.Log($"recv order id: {orderId}");
                if (microTxnTasks.TryGetValue(orderId, out TaskCompletionSource<string> tcs)) {
                    Dictionary<string, object> data = new Dictionary<string, object> {
                        { "appId", pCallback.m_unAppID },
                        { "orderId", pCallback.m_ulOrderID },
                        { "authorized", pCallback.m_bAuthorized }
                    };
                    tcs.TrySetResult(JsonConvert.SerializeObject(data));
                }
            });
        }

        public Task<string> GetAuthTicket() {
            if (!SteamManager.Initialized) {
                throw new Exception("Steam did not initialize.");
            }

            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            byte[] data = new byte[1024];
            HAuthTicket hTicket = SteamUser.GetAuthSessionTicket(data, 1024, out uint ticketLength);
            string ticket = BitConverter.ToString(data, 0, (int)ticketLength)
                    .Replace("-", string.Empty);
            tickTasks.TryAdd(hTicket, new Tuple<TaskCompletionSource<string>, string>(tcs, ticket));

            return tcs.Task;
        }

        /// <summary>
        /// 获取支付授权
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Task<string> GetMicroTxn(ulong orderId) {
            if (!SteamManager.Initialized) {
                throw new Exception("Steam did not initialize.");
            }

            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            microTxnTasks.TryAdd(orderId, tcs);
            return tcs.Task;
        }

        public string GetSteamId() {
            if (!SteamManager.Initialized) {
                throw new Exception("Steam did not initialize.");
            }
            
            CSteamID steamID = SteamUser.GetSteamID();
            return steamID.ToString();
        }

        public string GetSteamLanguage() {
            if (!SteamManager.Initialized) {
                throw new Exception("Steam did not initialize.");
            }
            
            return SteamApps.GetCurrentGameLanguage();
        }
    }
}

#endif