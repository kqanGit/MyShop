using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.ViewModels.Reports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class ReportPage : Page
    {
        private CancellationTokenSource? _loadCts;

        public ReportViewModel ViewModel { get; }

        public ReportPage()
        {
            InitializeComponent();

            ViewModel = App.Services.GetRequiredService<ReportViewModel>();

            Loaded += ReportPage_Loaded;
            Unloaded += ReportPage_Unloaded;
        }

        private async void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }

        private void ReportPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _loadCts?.Cancel();
            _loadCts?.Dispose();
            _loadCts = null;
        }

        private async Task RefreshAsync()
        {
            _loadCts?.Cancel();
            _loadCts?.Dispose();
            _loadCts = new CancellationTokenSource();

            await ViewModel.LoadReportAsync(_loadCts.Token);
        }

        private async void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }

        private async void ExportReport_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ExportReportAsync();
        }
    }
}