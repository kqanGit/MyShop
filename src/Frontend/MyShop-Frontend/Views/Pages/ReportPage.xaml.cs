using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.ViewModels.Reports;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class ReportPage : Page
    {
        public ReportViewModel ViewModel { get; }

        public ReportPage()
        {
            InitializeComponent();

            ViewModel = App.Services.GetRequiredService<ReportViewModel>();
            DataContext = ViewModel;

            Loaded += ReportPage_Loaded;
        }

        private async void ReportPage_Loaded(object sender, RoutedEventArgs e)
        {
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
