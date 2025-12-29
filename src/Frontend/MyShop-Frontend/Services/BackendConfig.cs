using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyShop_Frontend.Services
{
    public sealed class BackendConfig : IBackendConfig
    {

        private readonly ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;

        public Uri GetBaseUri()
        {
            var raw = _settings.Values[AppKeys.BackendBaseUrl] as string;

            // raw ??= "https://stunning-goggles-x5rv46v6674p26q4j-5126.app.github.dev/";
            raw ??= "http://localhost:5126";

            raw = raw.Trim();

            if (!raw.EndsWith("/")) raw += "/";

            if (!Uri.TryCreate(raw, UriKind.Absolute, out var uri))
                throw new InvalidOperationException($"Invalid BackendBaseUrl: {raw}");

            return uri;
        }

        public void SetBaseUrl(string baseUrl)
        {
            _settings.Values[AppKeys.BackendBaseUrl] = baseUrl?.Trim();
        }
    }
}