using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class LoginPage : Page
    {
        public ViewModels.Authentication.LoginViewModel ViewModel { get; } = new();

        public LoginPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void ConfigServerButton_Click(object sender, RoutedEventArgs e)
        {
            App.Windows.ShowServerConfigWindow();
        }

        private void CreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Frame)?.Navigate(typeof(RegisterPage), null,
                new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordInput.Password;

            if (string.IsNullOrWhiteSpace(ViewModel.Username))
            {
                Debug.WriteLine("Lỗi: Username đang trống tại View");
                return;
            }

            await ViewModel._login();
        }
    }
}
