#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using LC.Newtonsoft.Json;
using XD.SDK.Account.PC.Internal;

namespace XD.SDK.Common.PC.Internal {
    public class AccessTokenModule {
        // 登录
        private readonly static string XDG_COMMON_LOGIN = "api/login/v1/union";
        // 登录不注册，主机登录时用到
        private readonly static string XDG_COMMON_SIGNIN = "api/login/v1/signin"; 
        // 刷新 token
        private readonly static string XDG_COMMON_REFRESH = "api/login/v1/token/refresh";

        private static readonly Persistence persistence = new Persistence(Path.Combine(Application.persistentDataPath,
            XDGCommonPC.XD_PERSISTENCE_NAME,
            "access_token"));

        internal static AccessToken local;

        public static async Task<AccessToken> GetLocalAccessToken() {
            if (local == null){
                local = await persistence.Load<AccessToken>();
            }
            return local;
        }

        public static void ClearToken() {
            persistence.Delete();
            local = null;
        }

        /// <summary>
        /// 登录且注册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal static Task<AccessToken> Login(Dictionary<string, object> param, Dictionary<string, object> cacheData)
        {
            return LoginInternal(XDG_COMMON_LOGIN, param, cacheData);
        }

        /// <summary>
        /// 登录不注册
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        internal static Task<AccessToken> SignIn(Dictionary<string, object> param, Dictionary<string, object> cacheData) {
            return LoginInternal(XDG_COMMON_SIGNIN, param, cacheData);
        }

        static async Task<AccessToken> LoginInternal(string path,
            Dictionary<string, object> param, Dictionary<string, object> cacheData) {
            try
            {
                AccessTokenResponse response = await XDHttpClient.Post<AccessTokenResponse>(path, data: param);

                if (cacheData == null) {
                    local = new AccessToken();
                } else {
                    local = JsonConvert.DeserializeObject<AccessToken>(JsonConvert.SerializeObject(cacheData));
                }

                return await UpdateLocalToken(response.AccessToken);
            }
            catch (XDException xdException)
            {
                var errorMsg = xdException.Message;
                // 所有 40801 错误代表 401 授权失败,需要明示原因为 XD_TOKEN_EXPIRED
                if (xdException.code == ResponseCode.TOKEN_EXPIRED || xdException.code == 401)
                    errorMsg = "XD_TOKEN_EXPIRED";
                // 触发风控以及登陆频繁不需要发送 PreFail
                if (xdException.code != PhoneModule.INVALID_VERIFY_CODE && xdException.code != PhoneModule.LOGIN_REQUEST_FREQUENTLY)
                {
                    AliyunTrack.LoginPreFail(errorMsg);
                }
                throw xdException;
            }catch (Exception e) {
                AliyunTrack.LoginPreFail(e.Message);
                throw e;
            }
        }
        
        internal static async Task<AccessToken> Refresh() {
            RefreshTokenResponse response = await XDHttpClient.Post<RefreshTokenResponse>(XDG_COMMON_REFRESH);
            if (response.Data.IsUpdated) {
                await UpdateLocalToken(response.Data.AccessToken);
            }
            return local;
        }

        /// <summary>
        /// 更新本地 AccessToken，因为 AccessToken 中包含 SDK 的缓存字段，所以只做部分更新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        static async Task<AccessToken> UpdateLocalToken(AccessToken token) {
            if (local == null) {
                local = new AccessToken();
            }

            local.kid = token.kid;
            local.macKey = token.macKey;
            local.tokenType = token.tokenType;
            local.ExpireIn = token.ExpireIn;

            await persistence.Save(local);

            return local;
        }

    }
}
#endif