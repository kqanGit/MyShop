using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System.Diagnostics;

namespace MyShop_Frontend.Views.Pages
{
    /// <summary>
    /// Login page for user authentication.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public ViewModels.Authentication.LoginViewModel ViewModel { get; } = new();
        public LoginPage()
        {
            this.InitializeComponent();
            // Gán DataContext để Binding trong XAML hoạt động
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
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
            if (string.IsNullOrEmpty(ViewModel.Username))
            {
                Debug.WriteLine("Lỗi: Username đang trống tại View");
                return;
            }

            await ViewModel._login();

            if (ViewModel.IsAuthenticated)
            {
                var mainWindow = new MainWindow();
                mainWindow.Activate();

                var authWindow = App.Windows.AuthWindow;
                authWindow?.Close();
            }
        }
    }
}
