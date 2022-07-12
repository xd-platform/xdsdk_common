using System;
using System.Collections.Generic;
using TapTap.Bootstrap;
using XD.SDK.Common;

namespace XD.SDK.Account{
    public class XDGTokenManager{
        private static long DaySeconds = 24 * 60 * 60; // 24 小时有多少秒

        public static void updateFacebookToken(XDGUser xdgUser){
            var preTime = getFacebookRefreshTime(); //上一次刷新时间
            if (preTime == 0 || (getCurrentSecond() - preTime) < DaySeconds){ //第一次 或 不到 24小时
                return;
            }

            try{
                XDGAccountImpl.GetInstance().updateThirdPlatformTokenWithCallback(async (success) => {
                    if (success){
                        foreach (var bound in xdgUser.boundAccounts){
                            if (bound.ToLower().Equals("facebook")){
                                var tdsUser = await TDSUser.GetCurrent().Result.Fetch();
                                if (tdsUser != null && tdsUser.AuthData != null &&
                                    tdsUser.AuthData["facebook"] != null){
                                    //获取之前的信息
                                    var fbDic = tdsUser.AuthData["facebook"] as Dictionary<string, object>;
                                    var platToken = fbDic["access_token"] as string;
                                    var platUserId = fbDic["uid"] as string;
                                    XDGTool.Log($"FB token刷新前是 uid: {platUserId}, token:{platToken}");

                                    if (string.IsNullOrEmpty(platToken) || string.IsNullOrEmpty(platUserId)){
                                        XDGTool.LogError($"获取FB token失败, 有空: uid:{platUserId},  token: {platToken}");
                                        return;
                                    }
                                    
                                    //获取最新信息
                                    XDGAccountImpl.GetInstance().GetFacebookToken(async (newUid, newToken) => {
                                            if (newUid.Equals(platUserId) && !newToken.Equals(platToken)){
                                                //uid相同，token不同
                                                fbDic["access_token"] = newToken;
                                                await tdsUser.AssociateAuthData(fbDic, "facebook");
                                                updateFacebookRefreshTime();
                                                XDGTool.Log(
                                                    $"刷新FB成功后 uid: {newUid}, token:{newToken}。\n 刷新前是 uid: {platUserId}, token:{platToken}");
                                            } else{
                                                XDGTool.Log($"FB token相同 不刷新: uid:{newUid}, token:{newToken}");
                                            }
                                        },
                                        (e) => {
                                            XDGTool.LogError(
                                                $"获取FB token 失败: code:{e.code}, msg:{e.error_msg}, uid:{platUserId}, token:{platToken}");
                                        });
                                } else{
                                    XDGTool.LogError("刷新FB token 失败 tdsUser/AuthData/AuthData[facebook] 是空");
                                }

                                break;
                            }
                        }
                    } else{
                        XDGTool.Log($"刷新FB updateThirdPlatformTokenWithCallback: false");
                    }
                });
            } catch (Exception e){
                XDGTool.LogError("刷新FB token失败：" + e.Message);
            }
        }

        public static long getFacebookRefreshTime(){
            var timeStr = DataStorage.LoadString(DataStorage.RefreshFacebookDateKey);
            if (string.IsNullOrEmpty(timeStr)){
                return 0;
            }

            return long.Parse(timeStr);
        }

        public static void updateFacebookRefreshTime(){
            var time = getCurrentSecond() + "";
            DataStorage.SaveString(DataStorage.RefreshFacebookDateKey, time);
        }

        public static long getCurrentSecond(){ //获取当前时间  秒
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}