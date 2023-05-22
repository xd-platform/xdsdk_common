#if XD_STEAM_SUPPORT

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Steamworks;

namespace XD.SDK.Common.PC.Internal {
    public class SteamWrapper : ISteamSDKWrapper {
        private readonly Callback<GetAuthSessionTicketResponse_t> getAuthSessionTicketResponseCallback;
        private readonly ConcurrentDictionary<HAuthTicket, Tuple<TaskCompletionSource<string>, string>> tickTasks;

        public SteamWrapper() {
            tickTasks = new ConcurrentDictionary<HAuthTicket, Tuple<TaskCompletionSource<string>, string>>();

            getAuthSessionTicketResponseCallback = Callback<GetAuthSessionTicketResponse_t>.Create((GetAuthSessionTicketResponse_t pCallback) => {
                HAuthTicket authTicket = pCallback.m_hAuthTicket;
                if (tickTasks.TryRemove(authTicket, out Tuple<TaskCompletionSource<string>, string> callback)) {
                    callback.Item1.TrySetResult(callback.Item2);
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

        public string GetSteamId() {
            if (!SteamManager.Initialized) {
                throw new Exception("Steam did not initialize.");
            }
            
            CSteamID steamID = SteamUser.GetSteamID();
            return steamID.ToString();
        }
    }
}

#endif