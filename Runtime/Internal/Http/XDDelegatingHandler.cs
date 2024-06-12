using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XD.SDK.Common.Internal
{
    public class XdDelegatingHandler : DelegatingHandler
    {
        public XdDelegatingHandler(HttpMessageHandler innerHandler) :
            base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (XDUtility.UseProxy && XDUtility.Proxy != null)
            {
                if (GetInnerMostHandler(this) is HttpClientHandler httpClientHandler)
                {
                    if (httpClientHandler.Proxy == null)
                    {
                        httpClientHandler.Proxy = XDUtility.Proxy;
                    }
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private HttpMessageHandler GetInnerMostHandler(HttpMessageHandler handler)
        {
            if (handler is DelegatingHandler delegatingHandler)
            {
                return GetInnerMostHandler(delegatingHandler.InnerHandler);
            }

            return handler;
        }
    }
}