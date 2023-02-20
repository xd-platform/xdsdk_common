#if UNITY_STANDALONE || UNITY_EDITOR
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TapTap.Common;
using UnityEngine.Scripting;
using XD.SDK.Common.PC.Internal;
using XD.SDK.Payment.Internal;
using XD.SDK.Payment.PC;

namespace XD.SDK.Payment
{
    [Preserve]
    public class XDGPaymentPC : IXDGPayment
    {
        public async void PayWithWeb(
            string orderId,
            string productId,
            string productName,
            double payAmount,
            string roleId,
            string serverId,
            string extras,
            Action<WebPayResultType, string> callback)
        {
            try {
                await PayWithWeb(serverId, roleId, productId, 
                    orderId: orderId,
                    productName: productName,
                    payAmount: payAmount,
                    extras: extras);
                callback?.Invoke(WebPayResultType.OK, "支付完成");
            } catch (TaskCanceledException tce) {
                callback?.Invoke(WebPayResultType.Cancel, tce.Message);
            } catch (Exception e) {
                callback?.Invoke(WebPayResultType.Error, e.Message);
            }
        }

        // 查询需要补款的订单
        public async void CheckRefundStatus(Action<XDGRefundResultWrapper> callback)
        {
            try
            {
                var payInfos = await PayModule.CheckPay();
                var refundList = new List<XDGRefundDetails>();
                foreach (var info in payInfos)
                {
                    refundList.Add(info);
                }
                callback?.Invoke(new XDGRefundResultWrapperClass(refundList, null));
            }
            catch (Exception e)
            {
                var error = new XDException(Result.RESULT_ERROR, e.Message);
                callback?.Invoke(new XDGRefundResultWrapperClass(null, error));
            }
        }
        
        #region XDSDK 迁移
        private static async Task PayWithWeb(string serverId, string roleId, string productId,
            string orderId = null,
            string productName = null,
            double payAmount = 0,
            string extras = null) {
            if (string.IsNullOrEmpty(serverId)) {
                throw new ArgumentNullException(nameof(serverId));
            }
            if (string.IsNullOrEmpty(roleId)) {
                throw new ArgumentNullException(nameof(roleId));
            }
            if (string.IsNullOrEmpty(productId)) {
                throw new ArgumentNullException(nameof(productId));
            }

            var user = UserModule.current;
            if (user == null) {
                XDLogger.Debug("请先登录");
                return;
            }

            AliyunTrack.PaymentStart(productId);
            
            string url = PayModule.GetPayUrl(serverId, roleId, productId, orderId, productName, payAmount, extras);
            url = Uri.EscapeUriString(url);
            XDLogger.Debug("支付 pay URL: " + url);

            if (ConfigModule.IsGlobal) {
                PayListener payListener = new PayListener();
                await payListener.Start(url);
            } else {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

                Dictionary<string, object> data = new Dictionary<string, object> {
                    { "url", url }
                };
                Action<int, object> callback = (code, obj) => {
                    if (code == PayModule.PAY_CANCEL)
                    {
                        AliyunTrack.PaymentFailed("user_cancel");
                        tcs.TrySetCanceled();
                    } else {
                        string result = obj as string;
                        if (result == PayModule.PAY_FRAGMENT_SUCCESS)
                        {
                            AliyunTrack.PaymentDone();
                            tcs.TrySetResult(result);
                        } else if (result == PayModule.PAY_FRAGMENT_CANCEL) {
                            AliyunTrack.PaymentFailed("user_cancel");
                            tcs.TrySetCanceled();
                        } else {
                            AliyunTrack.PaymentFailed($"fail message: {result}");
                            tcs.TrySetException(XDException.MSG(result));
                        }
                    }
                };
                XD.SDK.Common.PC.Internal.UIManager.ShowUI<PaymentAlert>("PaymentAlert", data, callback);
                AliyunTrack.PaymentCallPage();
                await tcs.Task;
            }
        }
        
        public async Task<CheckPayType> CheckPay() {
            List<PayInfo> payInfos = await PayModule.CheckPay();
            if (payInfos != null) {
                var hasIOS = false;
                var hasAndroid = false;
                foreach (var payInfo in payInfos) {
                    if (payInfo.IsIOS) {
                        hasIOS = true;
                    } else if (payInfo.IsAndroid) {
                        hasAndroid = true;
                    }
                }
                if (hasIOS || hasAndroid) {
                    Dictionary<string, object> param = new Dictionary<string, object>() {
                        { "hasIOS", hasIOS },
                        { "hasAndroid", hasAndroid },
                    };
                    XD.SDK.Common.PC.Internal.UIManager.ShowUI<PayHintAlert>(param, null);
                }
                if (hasIOS && hasAndroid) {
                    return CheckPayType.iOSAndAndroid;
                } else if (hasIOS) {
                    return CheckPayType.iOS;
                } else if (hasAndroid) {
                    return CheckPayType.Android;
                }
                return CheckPayType.None;
            }
            return CheckPayType.None;
        }
        #endregion
        
        #region 未实现部分
        public void PayWithProduct(string orderId, string productId, string roleId, string serverId, string ext, Action<XDGOrderInfoWrapper> callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }
        
        public void QueryWithProductIds(string[] productIds, Action<XDGSkuDetailInfo> callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void QueryRestoredPurchase(Action<List<XDGRestoredPurchase>> callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }

        public void RestorePurchase(string purchaseToken, string orderId, string productId, string roleId, string serverId, string ext,
            Action<XDGOrderInfoWrapper> callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }
        
        public void CheckRefundStatusWithUI(Action<XDGRefundResultWrapper> callback)
        {
            UnityEngine.Debug.LogErrorFormat("NotImplementedException");
        }
        #endregion
    }
}
#endif