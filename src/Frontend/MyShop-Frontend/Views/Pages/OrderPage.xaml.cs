using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.ViewModels.Orders;
using MyShop_Frontend.Views.Controls.Order;
using System;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class OrderPage : Page
    {
        public OrderPage()
        {
            this.InitializeComponent();
        }
        private async void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var view = new CreateOrderControl();

            var dlg = new ContentDialog
            {
                XamlRoot = this.XamlRoot, // bắt buộc WinUI3
                Title = "Create Order",
                Content = view,
                PrimaryButtonText = "Create order",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            var result = await dlg.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var data = view.GetFormData();
                // TODO: submit order (call API)
            }
        }
    }
}
