using MyShop_Frontend.Contracts.Services;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public sealed class BackendBaseUrlHandler : DelegatingHandler
    {
        private readonly IBackendConfig _cfg;

        public BackendBaseUrlHandler(IBackendConfig cfg)
        {
            _cfg = cfg;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var baseUri = _cfg.GetBaseUri();
            var current = request.RequestUri;

            if (current is null)
            {
                request.RequestUri = baseUri;
                return base.SendAsync(request, cancellationToken);
            }

            if (!current.IsAbsoluteUri)
            {
                var rel = (current.OriginalString ?? string.Empty).Trim().TrimStart('/');
                request.RequestUri = new Uri(baseUri, rel);
                return base.SendAsync(request, cancellationToken);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
