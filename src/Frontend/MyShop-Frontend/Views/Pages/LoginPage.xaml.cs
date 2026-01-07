using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class LoginPage : Page
    {
        public ViewModels.Authentication.LoginViewModel ViewModel { get; } = new();

        public LoginPage()
        {
            InitializeComponent();
            DataContext = ViewModel;

            // Focus vào username khi trang load
            Loaded += (s, e) =>
            {
                if (string.IsNullOrEmpty(ViewModel.Username))
                    UsernameInput.Focus(FocusState.Programmatic);
                else
                    PasswordInput.Focus(FocusState.Programmatic);
            };

            // Cho phép nhấn Enter để đăng nhập
            UsernameInput.KeyDown += OnInputKeyDown;
            PasswordInput.KeyDown += OnInputKeyDown;
        }

        private void OnInputKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == global::Windows.System.VirtualKey.Enter)
            {
                SignInButton_Click(sender, e);
            }
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Sync password với ViewModel
            ViewModel.Password = PasswordInput.Password;
        }

        private void ConfigServerButton_Click(object sender, RoutedEventArgs e)
        {
            App.Windows.ShowServerConfigWindow();
        }

        private void CreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Frame)?.Navigate(typeof(RegisterPage), null,
                new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoginAsync();
        }
    }
}
