using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LC.Newtonsoft.Json;
using XD.SDK.Account;
using XD.SDK.Common;
using XD.SDK.Common.Internal;
using XD.SDK.Report.Internal;
using XDException = XD.SDK.Common.Internal.XDException;

namespace XD.SDK.Report
{
    public class XDReportImpl
    {
        private const string ReportEndpoint = "reporting/v1/player/report";
        private const string GetSignedUrlEndpoint = "reporting/v1/upload/oss_sign_url";
        private const int DefaultTimeoutSec = 5 * 60;

        private static volatile XDReportImpl _instance;
        private static readonly object Locker = new object();

        private XDReportImpl()
        {
        }

        public static XDReportImpl GetInstance()
        {
            if (_instance != null) return _instance;
            lock (Locker)
            {
                if (_instance == null)
                {
                    _instance = new XDReportImpl();
                }
            }

            return _instance;
        }

        public void Report(ReportParams reportParams, Action<ReportResult> resultCallback)
        {
            var currentUserId = UserManager.GetCurrentUser()?.userId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                resultCallback.Invoke(new ReportResult(false,
                    new XDException(ReportResult.CodeUserNotLoggedIn, "user is null, please login first!")));
                return;
            }
            if (reportParams.Reportee == null || reportParams.Reportee.XdId.Length <= 0
                                              || reportParams.Reasons == null || reportParams.Reasons.Count <= 0)
            {
                resultCallback.Invoke(new ReportResult(false,
                    new XDException(ReportResult.CodeParamInvalid, "invalid report params , please check.")));
                return;
            }

            ReportAsync(currentUserId, reportParams, resultCallback).ConfigureAwait(false);
        }

        private static async Task ReportAsync(string currentUserId, ReportParams reportParams, Action<ReportResult> resultCallback)
        {
            var requestParams = new ReportParamsInternal
            {
                Reporter = new ReportParamsInternal.UserInfo(currentUserId,
                    reportParams.Reporter?.Extras == null
                        ? null
                        : JsonConvert.SerializeObject(reportParams.Reporter.Extras)),
                Reportee = new ReportParamsInternal.UserInfo(reportParams.Reportee.XdId,
                    reportParams.Reportee.Extras == null
                        ? null
                        : JsonConvert.SerializeObject(reportParams.Reportee.Extras)),
                ReasonParams = reportParams.Reasons.Select((reasonInfo) => new ReportParamsInternal.ReasonParam
                {
                    ID = reasonInfo.ID,
                    Title = reasonInfo.Title,
                    Extras = reasonInfo.Extras == null ? null : JsonConvert.SerializeObject(reasonInfo.Extras)
                }).ToList(),
                UserDescription = reportParams.UserDescription,
                Extras = reportParams.Extras == null ? null : JsonConvert.SerializeObject(reportParams.Extras)
            };

            var cts = new CancellationTokenSource();
            var token = CancellationTokenSource.CreateLinkedTokenSource(cts.Token).Token;

            if (reportParams.EvidenceList != null && reportParams.EvidenceList.Count > 0)
            {
                try
                {
                    var uploadEvidenceTasks = reportParams.EvidenceList
                        .Select(filePath => UploadEvidence(filePath, cts, token))
                        .ToList();

                    var results = await Task.WhenAll(uploadEvidenceTasks);
                    var evidenceList = results.ToList();

                    requestParams.Evidence = evidenceList;
                }
                catch (XDException ex)
                {
                    XDGLogger.Error("Report failed, upload evidence error:" + ex);
                    cts.Cancel();
                    resultCallback.Invoke(new ReportResult(false, ex));
                    return;
                }
                catch (OperationCanceledException ex)
                {
                    XDGLogger.Warn("upload evidence canceled:" + ex.Message);
                    return;
                }
                catch (Exception e)
                {
                    XDGLogger.Error("Report failed, upload evidence Unexpected exception:" + e.Message);
                    cts.Cancel();
                    resultCallback.Invoke(new ReportResult(false,
                        new XDException(XDException.DEFAULT_CODE, e.Message)));
                    return;
                }
            }

            try
            {
                var reportSubmitResult = await XDHttpClientFactory.GetDefaultXdHttpClient()
                    .Post<ReportSubmitResult>(ReportEndpoint, data: requestParams);
                resultCallback.Invoke(new ReportResult(reportSubmitResult.ReportSubmitData.Success,
                    new XDException(reportSubmitResult.Code, reportSubmitResult.Message)));
            }
            catch (XDException e)
            {
                XDGLogger.Error("Report failed: " + e);
                resultCallback.Invoke(new ReportResult(false, e));
            }
            catch (Exception e)
            {
                XDGLogger.Error("Report failed: Unexpected exception:" + e.Message);
                resultCallback.Invoke(new ReportResult(false, new XDException(XDException.DEFAULT_CODE, e.Message)));
            }
        }

        private static async Task<EvidenceInfo> UploadEvidence(string filePath,
            CancellationTokenSource cancellationTokenSource, CancellationToken token)
        {
            if (!File.Exists(filePath))
            {
                XDGLogger.Error($"report failed, file: {filePath} dose not exist");
                throw new XDException(ReportResult.CodeFileNotExist, $"file: {filePath} does not exist");
            }

            var fileInfo = new FileInfo(filePath);
            var queryParams = new Dictionary<string, object>
            {
                ["file_name"] = fileInfo.Name,
                ["file_size"] = fileInfo.Length
            };

            SignedUrlResponse signedUrlResponse;
            try
            {
                signedUrlResponse = await XDHttpClientFactory.GetDefaultXdHttpClient()
                    .Get<SignedUrlResponse>(GetSignedUrlEndpoint, queryParams: queryParams);
            }
            catch (OperationCanceledException ex)
            {
                XDGLogger.Warn("UploadEvidenceBySignedOssUrl upload evidence canceled:" + ex.Message);
                throw;
            }
            catch (Exception e)
            {
                XDGLogger.Error("Get Signed Url error: " + e);
                cancellationTokenSource.Cancel();
                throw;
            }
            
            var fullUrl = signedUrlResponse?.SignedUrlData?.SignedUrl;
            if (string.IsNullOrEmpty(fullUrl))
            {
                XDGLogger.Error("upload evidence error: couldn't get signed url.");
                cancellationTokenSource.Cancel();
                throw new XDException(XDException.DEFAULT_CODE, "Couldn't get signed url");
            }

            try
            {
                var timeOutSec = signedUrlResponse.SignedUrlData.TimeOutSec;
                if (timeOutSec <= 0)
                {
                    timeOutSec = DefaultTimeoutSec;
                }

                var uploadEvidenceResponse =
                    await UploadEvidenceBySignedOssUrl(signedUrlResponse.SignedUrlData.SignedUrl, timeOutSec,
                        filePath, token);
                if (uploadEvidenceResponse.StatusCode == HttpStatusCode.OK)
                {
                    var uri = new Uri(fullUrl);
                    var evidenceInfo = new EvidenceInfo(fileInfo.Name,
                        new Uri(uri.GetLeftPart(UriPartial.Path)).ToString(), fileInfo.Length);
                    return evidenceInfo;
                }

                var errorContent = await uploadEvidenceResponse.Content.ReadAsStringAsync();
                var ex = new XDException((int)uploadEvidenceResponse.StatusCode, errorContent);
                XDGLogger.Error("Report upload evidences failed: " + ex);
                throw ex;
            }
            catch (OperationCanceledException ex)
            {
                XDGLogger.Warn("UploadEvidence upload evidence canceled:" + ex.Message);
                throw;
            }
            catch (Exception e)
            {
                XDGLogger.Error("Report upload evidences catch an error: " + e);
                cancellationTokenSource.Cancel();
                throw;
            }
        }

        private static async Task<HttpResponseMessage> UploadEvidenceBySignedOssUrl(string signedOssUrl, int timeOutSec,
            string filePath, CancellationToken token)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var content = new StreamContent(fileStream);
                    var ossClient = new HttpClient(new XdDelegatingHandler(new HttpClientHandler()));
                    ossClient.Timeout = TimeSpan.FromSeconds(timeOutSec);
                    var response = await ossClient.PutAsync($"{signedOssUrl}", content, token);
                    return response;
                }
                catch (OperationCanceledException ex)
                {
                    XDGLogger.Warn("UploadEvidenceBySignedOssUrl upload evidence canceled:" + ex);
                    if (!token.IsCancellationRequested)
                    {
                        throw new TimeoutException("UploadEvidenceBySignedOssUrl upload evidence timeout");    
                    }

                    throw;
                }
                catch (Exception e)
                {
                    XDGLogger.Error("signedOssUrl: " + signedOssUrl + ", ------> " + e);
                    throw;
                }
            }
        }
    }
}