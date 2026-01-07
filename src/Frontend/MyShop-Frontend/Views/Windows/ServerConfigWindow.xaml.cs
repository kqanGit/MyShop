using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyShop_Frontend.Views.Windows
{
    public sealed partial class ServerConfigWindow : Window
    {
        private readonly IBackendConfig _backendConfig;

        private sealed record DefaultServerConfig(string Host, string Port, string Dns);

        public ServerConfigWindow()
        {
            InitializeComponent();
            Title = "MyShop - Server Config";

            _backendConfig = App.Services.GetRequiredService<IBackendConfig>();

            ApplyDefaultToUI(loadFallbackIfMissing: true);
            StatusText.Text = "";
        }

        private DefaultServerConfig ReadUI()
        {
            return new DefaultServerConfig(
                (ServerAddressBox.Text ?? "").Trim(),
                (PortBox.Text ?? "").Trim(),
                (DnsBox.Text ?? "").Trim()
            );
        }

        private void ApplyToUI(DefaultServerConfig cfg)
        {
            ServerAddressBox.Text = cfg.Host;
            PortBox.Text = cfg.Port;
            DnsBox.Text = cfg.Dns;
        }

        private DefaultServerConfig? LoadSavedDefault()
        {
            var raw = LocalSettingsHelper.GetValue<string>(AppKeys.DefaultBackendConfig);
            if (string.IsNullOrWhiteSpace(raw)) return null;

            try
            {
                return JsonSerializer.Deserialize<DefaultServerConfig>(raw);
            }
            catch
            {
                return null;
            }
        }

        private void SaveAsDefault(DefaultServerConfig cfg)
        {
            LocalSettingsHelper.SetValue(AppKeys.DefaultBackendConfig, JsonSerializer.Serialize(cfg));
        }

        private void ApplyDefaultToUI(bool loadFallbackIfMissing)
        {
            var saved = LoadSavedDefault();
            if (saved is not null)
            {
                ApplyToUI(saved);
                return;
            }

            if (!loadFallbackIfMissing) return;

            ApplyToUI(new DefaultServerConfig(
                Host: "localhost",
                Port: "5126",
                Dns: "8.8.8.8"
            ));
        }

        private string BuildBaseUrlFromUI()
        {
            var hostOrUrl = (ServerAddressBox.Text ?? "").Trim();
            var portText = (PortBox.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(hostOrUrl))
                throw new InvalidOperationException("Server address is required.");

            if (hostOrUrl.Contains("://", StringComparison.Ordinal))
                return hostOrUrl.Trim().TrimEnd('/') + "/";

            var lower = hostOrUrl.ToLowerInvariant();
            var isCodespaces = lower.Contains(".app.github.dev") || lower.EndsWith(".github.dev");
            var isLocal = lower.StartsWith("localhost") || lower.StartsWith("127.") || lower.StartsWith("192.168.") || lower.StartsWith("10.");

            if (isCodespaces)
                return $"https://{hostOrUrl.TrimEnd('/')}/";

            if (!int.TryParse(portText, out var port) || port <= 0 || port > 65535)
                throw new InvalidOperationException("Port is invalid.");

            var scheme = isLocal ? "http" : "https";
            return $"{scheme}://{hostOrUrl}:{port}/";
        }

        private async void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StatusText.Text = "Testing...";

                var baseUrl = BuildBaseUrlFromUI();

                using var http = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = TimeSpan.FromSeconds(5)
                };

                var probe = await ProbeBackendAsync(http);

                StatusText.Text = probe is not null
                    ? $"Connection OK ({baseUrl}) ✓ {probe}"
                    : $"Failed: cannot detect backend endpoints ({baseUrl})";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Failed: " + ex.Message;
            }
        }

        private static async Task<string?> ProbeBackendAsync(HttpClient http)
        {
            var candidates = new[]
            {
                "swagger/index.html",
                "swagger/v1/swagger.json"
            };

            foreach (var path in candidates)
            {
                try
                {
                    using var res = await http.GetAsync(path);
                    if (res.IsSuccessStatusCode || res.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                        return "/" + path + $" ({(int)res.StatusCode})";
                }
                catch
                {
                }
            }

            return null;
        }

        private void ResetDefault_Click(object sender, RoutedEventArgs e)
        {
            ApplyDefaultToUI(loadFallbackIfMissing: true);
            SaveAsDefaultCheckBox.IsChecked = false;

            StatusText.Text = LoadSavedDefault() is null
                ? "Reset to fallback default"
                : "Reset to saved default";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var baseUrl = BuildBaseUrlFromUI();
                _backendConfig.SetBaseUrl(baseUrl);

                if (SaveAsDefaultCheckBox.IsChecked == true)
                {
                    SaveAsDefault(ReadUI());
                    StatusText.Text = $"Saved + set as default ({_backendConfig.GetBaseUrl()})";
                }
                else
                {
                    StatusText.Text = $"Saved ({_backendConfig.GetBaseUrl()})";
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = "Save failed: " + ex.Message;
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            App.Windows.ShowAuthWindow();
            Close();
        }
    }
}
