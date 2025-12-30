using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Dashboard;
using MyShop_Frontend.ViewModels.Reports;
using MyShop_Frontend.Views.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyShop_Frontend
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private static readonly Lazy<IServiceProvider> _services = new Lazy<IServiceProvider>(ConfigureServices);

        public static IServiceProvider Services => _services.Value;

        public static WindowsService Windows => Services.GetRequiredService<WindowsService>();

        public static Window? MainWindow { get; set; }

        public App()
        {
            InitializeComponent();

            // (Optional) ép khởi tạo sớm để bắt lỗi cấu hình ngay khi start
            _ = Services;
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Windows.ShowAuthWindow();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // === Singleton Services ===
            services.AddSingleton<WindowsService>();
            services.AddSingleton<IBackendConfig, BackendConfig>();
            services.AddSingleton<ITokenStore, TokenStore>();

            // === HttpClient Services ===
            services.AddHttpClient<IApiClient, ApiClient>((sp, http) =>
            {
                var cfg = sp.GetRequiredService<IBackendConfig>();
                http.BaseAddress = cfg.GetBaseUri();
                http.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<IAuthenticationService, AuthenticationService>((sp, http) =>
            {
                var cfg = sp.GetRequiredService<IBackendConfig>();
                http.BaseAddress = cfg.GetBaseUri();
                http.Timeout = TimeSpan.FromSeconds(30);
            });

            // === Transient Services ===
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IReportService, ReportService>();

            // === ViewModels ===
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<ReportViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
