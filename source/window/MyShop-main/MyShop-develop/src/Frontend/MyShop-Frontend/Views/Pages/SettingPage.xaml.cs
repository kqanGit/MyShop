using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.ViewModels.Settings;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class SettingPage : Page
    {
        public SettingViewModel ViewModel { get; }

        public SettingPage()
        {
            this.InitializeComponent();

            // Initialize ViewModel
            var backendConfig = App.Services.GetService(typeof(Contracts.Services.IBackendConfig)) as Contracts.Services.IBackendConfig;
            ViewModel = new SettingViewModel(backendConfig);

            DataContext = ViewModel;
        }
    }
}
