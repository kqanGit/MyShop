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

            services.AddSingleton<MyShop_Frontend.Services.WindowsService>();

            services.AddSingleton<MyShop_Frontend.Contracts.Services.IBackendConfig, MyShop_Frontend.Services.BackendConfig>();
            services.AddSingleton<MyShop_Frontend.Contracts.Services.ITokenStore, MyShop_Frontend.Services.TokenStore>();

            services.AddHttpClient<MyShop_Frontend.Contracts.Services.IApiClient, MyShop_Frontend.Services.ApiClient>((sp, http) =>
            {
                var cfg = sp.GetRequiredService<MyShop_Frontend.Contracts.Services.IBackendConfig>();
                http.BaseAddress = cfg.GetBaseUri();
                http.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<MyShop_Frontend.Contracts.IAuthenticationService, MyShop_Frontend.Services.AuthenticationService>((sp, http) =>
            {
                var cfg = sp.GetRequiredService<MyShop_Frontend.Contracts.Services.IBackendConfig>();
                http.BaseAddress = cfg.GetBaseUri();
                http.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddTransient<MyShop_Frontend.Contracts.Services.IDashboardService, MyShop_Frontend.Services.DashboardService>();
            services.AddTransient<MyShop_Frontend.ViewModels.Dashboard.DashboardViewModel>();

            return services.BuildServiceProvider();
        }


    }
}
