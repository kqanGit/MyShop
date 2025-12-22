using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;

namespace MyShop_Frontend.Views.Pages
{
    /// <summary>
    /// Login page for user authentication.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
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
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            // Fix for CS1501: No overload for method 'Activate' takes 1 arguments
            // To open MainWindow, create a new instance and call Activate()
            var mainWindow = new MainWindow();
            mainWindow.Activate();

            var authWindow = App.Windows.AuthWindow;
            authWindow?.Close();
        }
    }
}
