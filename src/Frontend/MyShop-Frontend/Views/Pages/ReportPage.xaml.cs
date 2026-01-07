using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.ViewModels.Reports;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class ReportPage : Page
    {
        public ReportViewModel ViewModel { get; }
        private readonly ITokenStore _tokenStore;

        public ReportPage()
        {
            InitializeComponent();

            _tokenStore = App.Services.GetRequiredService<ITokenStore>();
            ViewModel = App.Services.GetRequiredService<ReportViewModel>();
            DataContext = ViewModel;

            Loaded += ReportPage_Loaded;
        }

        private async void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            var role = _tokenStore.GetRole()?.ToLowerInvariant();
            if (role == "3" || role == "staff")
            {
                var dlg = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Permission denied",
                    Content = "You do not have permission to view reports.",
                    CloseButtonText = "OK"
                };
                await dlg.ShowAsync();
                return;
            }

            await ViewModel.LoadReportAsync();
        }

        private async void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadReportAsync();
        }

        private async void ExportReport_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ExportReportAsync();
        }
    }
}
