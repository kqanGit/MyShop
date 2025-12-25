using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MyShop_Frontend.Views.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyShop_Frontend
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public ViewModels.MainViewModel ViewModel { get; } = new();
        public MainWindow()
        {
            this.InitializeComponent();
            (this.Content as FrameworkElement).DataContext = ViewModel;
        }

        private void RootNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var invokedItem = args.InvokedItemContainer as NavigationViewItem;

            if (invokedItem != null)
            {
                string tag = invokedItem.Tag?.ToString();

                // Chỉ xử lý nếu Tag là "SignOut"
                if (tag == "SignOut")
                {
                    ViewModel.SignOutCommand.Execute(null);
                    this.Close(); // Đóng cửa sổ hiện tại sau khi đăng xuất
                }
            }
        }

        

    }
}
