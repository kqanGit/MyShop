using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using MyShop_Frontend.Views.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyShop_Frontend.Views.Controls
{
    public sealed partial class AuthLayout : UserControl
    {
        public AuthLayout()
        {
            InitializeComponent();
        }

        public UIElement FormContent
        {
            get => (UIElement)GetValue(FormContentProperty);
            set => SetValue(FormContentProperty, value);
        }

        public static readonly DependencyProperty FormContentProperty =
            DependencyProperty.Register(nameof(FormContent), typeof(UIElement),
                typeof(AuthLayout), new PropertyMetadata(null, OnFormChanged));

        private static void OnFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AuthLayout)d;
            control.Presenter.Content = e.NewValue as UIElement;
        }


        public void GoToLogin() =>
    AuthFrame.Navigate(typeof(LoginPage), null,
        new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });

        public void GoToRegister() =>
            AuthFrame.Navigate(typeof(RegisterPage), null,
                new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
    }
}
