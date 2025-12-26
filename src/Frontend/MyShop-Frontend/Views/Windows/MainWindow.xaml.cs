using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyShop_Frontend.Views.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

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
            ContentFrame.Navigate(typeof(Views.Pages.DashboardPage)); // mặc định
            
            SelectNavigationItemByTag("Dashboard");
            
            (this.Content as FrameworkElement).DataContext = ViewModel;
        }

        private void RootNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var invokedItem = args.InvokedItemContainer as NavigationViewItem;

            if (invokedItem != null)
            {
                string tag = invokedItem.Tag?.ToString();

                if (tag == "SignOut")
                {
                    ViewModel.SignOutCommand.Execute(null);
                    this.Close(); // Đóng cửa sổ hiện tại sau khi đăng xuất
                }
            }
        }
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is not NavigationViewItem item)
                return;

            var tag = item.Tag?.ToString();
            if (string.IsNullOrEmpty(tag))
                return;

            if (tag.Equals("SignOut", StringComparison.OrdinalIgnoreCase))
            {
                // TODO: implement sign-out logic
                return;
            }

            // Try resolve page type by convention: MyShop_Frontend.Views.Pages.<Tag>Page
            Type? pageType = Type.GetType($"MyShop_Frontend.Views.Pages.{tag}Page");

            if (pageType is null)
            {
                pageType = tag switch
                {
                    "Dashboard" => typeof(DashboardPage),
                    "Login" => typeof(LoginPage),
                    "Order" => typeof(OrderPage),
                    "Products" => typeof(ProductPage),
                    "Customers" => typeof(CustomerPage),
                    "Report" => typeof(ReportPage),
                    "Help" => typeof(HelpPage),
                    "Settings" => typeof(SettingPage),
                    _ => null
                };
            }

            if (pageType != null)
            {
                ContentFrame.Navigate(pageType);
            }

            // ✅ tô đậm item đang chọn
            UpdateNavFontWeights(sender);
        }

        private void SelectNavigationItemByTag(string tag)
        {
            // Tìm NavigationViewItem có Tag khớp
            var item = FlattenNavItems(NavView)
                .FirstOrDefault(nvi => nvi.Tag?.ToString() == tag);

            if (item != null)
            {
                NavView.SelectedItem = item;
                UpdateNavFontWeights(NavView);
            }
        }

        private static void UpdateNavFontWeights(NavigationView nav)
        {
            // Lưu ý: SelectedItem của NavigationView thường chính là NavigationViewItem
            var selected = nav.SelectedItem as NavigationViewItem;

            foreach (var nvi in FlattenNavItems(nav))
            {
                nvi.FontWeight = ReferenceEquals(nvi, selected)
                    ? FontWeights.Bold
                    : FontWeights.Normal;
            }
        }

        private static IEnumerable<NavigationViewItem> FlattenNavItems(NavigationView nav)
        {
            foreach (var obj in nav.MenuItems)
                foreach (var nvi in Flatten(obj))
                    yield return nvi;

            foreach (var obj in nav.FooterMenuItems)
                foreach (var nvi in Flatten(obj))
                    yield return nvi;

            static IEnumerable<NavigationViewItem> Flatten(object? obj)
            {
                if (obj is NavigationViewItem nvi)
                {
                    yield return nvi;

                    foreach (var child in nvi.MenuItems)
                        foreach (var childNvi in Flatten(child))
                            yield return childNvi;
                }
            }
        }
    }


}
