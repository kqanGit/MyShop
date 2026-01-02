using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Dashboard;
using MyShop_Frontend.ViewModels.Reports;
using System;

namespace MyShop_Frontend
{
    public partial class App : Application
    {
        private static readonly Lazy<IServiceProvider> _services = new Lazy<IServiceProvider>(ConfigureServices);

        public static IServiceProvider Services => _services.Value;

        public static WindowsService Windows => Services.GetRequiredService<WindowsService>();

        public static Window? MainWindow { get; set; }

        public App()
        {
            InitializeComponent();
            _ = Services;
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Windows.ShowAuthWindow();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<WindowsService>();
            services.AddSingleton<IBackendConfig, BackendConfig>();
            services.AddSingleton<ITokenStore, TokenStore>();
            services.AddSingleton<Contracts.Services.IUserSettingsStore, Services.UserSettingsStore>();

            services.AddTransient<BackendBaseUrlHandler>();

            services.AddHttpClient<IApiClient, ApiClient>(ConfigureHttpClient)
                .AddHttpMessageHandler<BackendBaseUrlHandler>();

            services.AddHttpClient<IAuthenticationService, AuthenticationService>(ConfigureHttpClient)
                .AddHttpMessageHandler<BackendBaseUrlHandler>();

            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IReportService, ReportService>();

            services.AddTransient<DashboardViewModel>();
            services.AddTransient<ReportViewModel>();

            return services.BuildServiceProvider();
        }

        private static void ConfigureHttpClient(IServiceProvider sp, System.Net.Http.HttpClient http)
        {
            http.Timeout = TimeSpan.FromSeconds(30);

            var cfg = sp.GetRequiredService<IBackendConfig>();
            try
            {
                http.BaseAddress = cfg.GetBaseUri();
            }
            catch
            {
                http.BaseAddress = new Uri("http://localhost:5126/");
            }
        }
    }
}