using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyShop_Frontend.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyShop_Frontend.Views.Pages
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class LoginPage : Window
    {
        public LoginViewModel ViewModel { get; } = new LoginViewModel();
        public LoginPage()
        {
            InitializeComponent();
        }
        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordInput.Password;
        }
    }


    public class HandButton : Button
    {
        public HandButton()
        {
            // dùng style mặc định của Button
            this.DefaultStyleKey = typeof(Button);

            // set cursor hand khi pointer over
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }
    }

}
