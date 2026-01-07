using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Services;
using Microsoft.UI;
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
        private readonly IUserSettingsStore _userSettings;
        private readonly ITokenStore _tokenStore;

        public ViewModels.MainViewModel ViewModel { get; } = new();
        public MainWindow()
        {
            this.InitializeComponent();

            _userSettings = App.Services.GetRequiredService<IUserSettingsStore>();
            _tokenStore = App.Services.GetRequiredService<ITokenStore>();

            HideReportForStaff();

            var startTag = "Dashboard";
            if (_userSettings.GetRememberLastModule(defaultValue: true))
            {
                var last = _userSettings.GetLastModule();
                if (!string.IsNullOrWhiteSpace(last))
                    startTag = last;
            }

            NavigateByTag(startTag);

            (this.Content as FrameworkElement).DataContext = ViewModel;
        }

        private bool IsStaffRole()
        {
            var role = _tokenStore.GetRole()?.ToLowerInvariant();
            return role == "3" || role == "staff";
        }

        private void HideReportForStaff()
        {
            if (!IsStaffRole()) return;

            var reportItem = FlattenNavItems(NavView).FirstOrDefault(i => i.Tag?.ToString() == "Report");
            if (reportItem != null)
            {
                // Remove from MenuItems
                NavView.MenuItems.Remove(reportItem);
            }
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
                    this.Close();
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
                if (!tag.Equals("SignOut", StringComparison.OrdinalIgnoreCase))
                {
                    _userSettings.SetLastModule(tag);
                }
            }

            // Tô đậm item đang chọn
            UpdateNavFontWeights(sender);
        }

        private void SelectNavigationItemByTag(string tag)
        {
            var item = FlattenNavItems(NavView)
                .FirstOrDefault(nvi => nvi.Tag?.ToString() == tag);

            if (item != null)
            {
                NavView.SelectedItem = item;
                UpdateNavFontWeights(NavView);
            }
        }

        private void NavigateByTag(string tag)
        {
            if (tag == "Report" && IsStaffRole())
            {
                tag = "Dashboard";
            }

            SelectNavigationItemByTag(tag);

            Type? pageType = Type.GetType($"MyShop_Frontend.Views.Pages.{tag}Page") ?? tag switch
            {
                "Dashboard" => typeof(DashboardPage),
                "Login" => typeof(LoginPage),
                "Order" => typeof(OrderPage),
                "Products" => typeof(ProductPage),
                "Customers" => typeof(CustomerPage),
                "Report" => typeof(ReportPage),
                "Help" => typeof(HelpPage),
                "Settings" => typeof(SettingPage),
                _ => typeof(DashboardPage)
            };

            ContentFrame.Navigate(pageType);
        }

        private static void UpdateNavFontWeights(NavigationView nav)
        {
            var selected = nav.SelectedItem as NavigationViewItem;

            foreach (var nvi in FlattenNavItems(nav))
            {
                bool isSelected = ReferenceEquals(nvi, selected);

                // Cập nhật FontWeight cho text
                nvi.FontWeight = isSelected ? FontWeights.Bold : FontWeights.Normal;

                // Cập nhật icon
                if (nvi.Icon is FontIcon fontIcon)
                {
                    fontIcon.FontWeight = isSelected ? FontWeights.Bold : FontWeights.Normal;

                    var scale = isSelected ? 1.25 : 1.0;
                    fontIcon.RenderTransform = new ScaleTransform { ScaleX = scale, ScaleY = scale };
                    fontIcon.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
                }
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


        private void GlobalSearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is MyShop_Frontend.ViewModels.SearchResult result)
            {
                // Navigate based on type
                switch (result.Type)
                {
                    case "Customer":
                        NavigateByTag("Customers"); 
                        // Optionally pass data: ContentFrame.Navigate(typeof(CustomerPage), result.Data);
                        break;
                    case "Product":
                        NavigateByTag("Products");
                        break;
                    case "Order":
                        NavigateByTag("Order");
                        break;
                }
            }
        }
    }


}