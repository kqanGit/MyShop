using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.ViewModels.Dashboard;
using MyShop_Frontend.Views.Pages;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<DashboardViewModel>();
            DataContext = ViewModel;
            Loaded += DashboardPage_Loaded;
        }

        private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
            => await ViewModel.RefreshAsync();

        private async void Refresh_Click(object sender, RoutedEventArgs e)
            => await ViewModel.RefreshAsync();

        private async void Export_Click(object sender, RoutedEventArgs e)
            => await ViewModel.ExportExcelAsync();

        private void ViewAllOrders_Click(object sender, RoutedEventArgs e)
        {
            if (App.MainWindow is MainWindow main)
            {
                main.NavigateByTag("Order");
            }
            else
            {
                Frame?.Navigate(typeof(OrderPage));
            }
        }
    }
}