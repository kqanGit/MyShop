using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using System;
using Windows.Storage;

namespace MyShop_Frontend.Services
{
    public sealed class BackendConfig : IBackendConfig
    {
        public string? GetBaseUrl()
        {
            var raw = LocalSettingsHelper.GetValue<string>(AppKeys.BackendBaseUrl);
            if (string.IsNullOrWhiteSpace(raw)) return null;

            // luôn trả về dạng chuẩn (có scheme + có / cuối)
            return NormalizeBaseUrl(raw);
        }

        public Uri GetBaseUri()
        {
            var baseUrl = GetBaseUrl();

#if DEBUG
            // optional: chỉ default khi debug (tuỳ bạn)
            baseUrl ??= "http://localhost:5126/";
#endif

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("BackendBaseUrl is not configured.");

            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                throw new InvalidOperationException($"Invalid BackendBaseUrl: {baseUrl}");

            return uri;
        }

        public void SetBaseUrl(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                LocalSettingsHelper.Remove(AppKeys.BackendBaseUrl);
                return;
            }

            var normalized = NormalizeBaseUrl(baseUrl);

            // Validate ngay lúc lưu (đỡ lưu rác)
            if (!Uri.TryCreate(normalized, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException($"Invalid http/https base url: {baseUrl}", nameof(baseUrl));
            }

            LocalSettingsHelper.SetValue(AppKeys.BackendBaseUrl, normalized);
        }

        private static string NormalizeBaseUrl(string input)
        {
            var raw = input.Trim().TrimEnd('/');

            // Chưa có scheme -> đoán hợp lý
            if (!raw.Contains("://", StringComparison.Ordinal))
            {
                var lower = raw.ToLowerInvariant();

                var isLocal =
                    lower.StartsWith("localhost") ||
                    lower.StartsWith("127.") ||
                    lower.StartsWith("192.168.") ||
                    lower.StartsWith("10.");

                var isCodespaces =
                    lower.Contains(".app.github.dev") || lower.EndsWith(".github.dev");

                var scheme = isCodespaces ? "https://" : (isLocal ? "http://" : "https://");
                raw = scheme + raw;
            }

            // luôn có đúng 1 dấu /
            return raw.TrimEnd('/') + "/";
        }
    }
}
