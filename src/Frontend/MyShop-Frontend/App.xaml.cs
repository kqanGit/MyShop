using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Dashboard;
using MyShop_Frontend.ViewModels.Reports;
using System;
using System.Reflection;
using Windows.ApplicationModel;

namespace MyShop_Frontend
{
    public partial class App : Application
    {
        private static readonly Lazy<IServiceProvider> _services = new Lazy<IServiceProvider>(ConfigureServices);

        public static IServiceProvider Services => _services.Value;

        public static WindowsService Windows => Services.GetRequiredService<WindowsService>();

        public static Window? MainWindow { get; set; }

        public static T GetService<T>()
            where T : class
        {
            if (App.Services is IServiceProvider serviceProvider)
            {
                var service = serviceProvider.GetService<T>();
                return service ?? throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }
            throw new InvalidOperationException("The ServiceProvider is not initialized.");
        }

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
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IVoucherService, VoucherService>();
            services.AddTransient<IUserConfigService, UserConfigService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<ViewModels.Customers.CustomerViewModel>();
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

        private static string GetAppVersion()
        {
            try
            {
                var v = Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
            catch
            {
                var asm = Assembly.GetExecutingAssembly();
                var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
                if (!string.IsNullOrWhiteSpace(info)) return info;

                var v2 = asm.GetName().Version;
                return v2?.ToString() ?? "1.0.0";
            }
        }

        public static string AppVersion { get; } = GetAppVersion();
    }
}