using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.ViewModels.Orders;
using MyShop_Frontend.Views.Controls.Order;
using System;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class OrderPage : Page
    {
        private readonly IApiClient _api;
        private readonly IOrderService _orders;

        public OrderPage()
        {
            InitializeComponent();
            _api = App.Services.GetRequiredService<IApiClient>();
            _orders = App.Services.GetRequiredService<IOrderService>();
        }

        private async void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CreateOrderDialogControl
            {
                XamlRoot = this.XamlRoot
            };

            await dlg.ShowAsync();

            if (DataContext is OrderViewModel vm)
                await vm.LoadOrdersAsync();
        }

        private async void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && item.CommandParameter is OrderSummaryDto order)
            {
                try
                {
                   var detail = await _orders.GetOrderByIdAsync(order.OrderId);

                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine($"Order: {detail.OrderCode}");
                    sb.AppendLine($"Date: {detail.OrderDate:dd/MM/yyyy HH:mm}");
                    var normalizedStatus = detail.Status.ToLower() switch
                    {
                        "completed" or "paid" or "hoàn thành" => "Paid",
                        "canceled" or "cancelled" or "đã hủy" => "Canceled",
                        "delivering" => "Paid",
                        _ => "New"
                    };

                    sb.AppendLine($"Status: {normalizedStatus}");
                    sb.AppendLine($"Voucher: {detail.VoucherCode ?? "None"}");
                    sb.AppendLine($"Total: {detail.TotalPrice:N0}  Discount: {detail.DiscountAmount:N0}  Final: {detail.FinalPrice:N0}");
                    sb.AppendLine();
                    sb.AppendLine("Items:");
                    foreach (var line in detail.OrderDetails)
                    {
                        sb.AppendLine($"- {line.ProductName} x{line.Quantity}  @ {line.PriceAtPurchase:N0} = {line.TotalLine:N0}");
                    }

                    var contentBlock = new TextBlock 
                    { 
                        Text = sb.ToString(), 
                        TextWrapping = TextWrapping.Wrap 
                    };

                    var scrollViewer = new ScrollViewer 
                    { 
                        MaxHeight = 500,
                        MaxWidth = 600,
                        Content = contentBlock 
                    };

                    var dlg = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Order Details",
                        Content = scrollViewer,
                        CloseButtonText = "Close"
                    };

                    await dlg.ShowAsync();
                }
                catch (Exception ex)
                {
                    var dlg = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Load details failed",
                        Content = ex.Message,
                        CloseButtonText = "OK"
                    };
                    await dlg.ShowAsync();
                }
            }
        }

        private async void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && item.CommandParameter is OrderSummaryDto order)
            {
                var confirm = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Delete Order",
                    Content = $"Delete order {order.OrderCode}?",
                    PrimaryButtonText = "Delete",
                    SecondaryButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Secondary
                };

                var res = await confirm.ShowAsync();
                if (res != ContentDialogResult.Primary) return;

                try
                {
                    await _orders.DeleteOrderAsync(order.OrderId);

                    if (DataContext is OrderViewModel vm)
                        await vm.LoadOrdersAsync();
                }
                catch (Exception ex)
                {
                    var dlg = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Delete failed",
                        Content = ex.Message,
                        CloseButtonText = "OK"
                    };
                    await dlg.ShowAsync();
                }
            }
        }

        private async void PayOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && item.CommandParameter is OrderSummaryDto order)
            {
                try
                {
                    await _orders.PayOrderAsync(order.OrderId);
                     if (DataContext is OrderViewModel vm)
                        await vm.LoadOrdersAsync();
                 }
                catch (Exception ex)
                {
                    var dlg = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Pay failed",
                        Content = ex.Message,
                        CloseButtonText = "OK"
                    };
                    await dlg.ShowAsync();
                }
            }
        }

        private async void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && item.CommandParameter is OrderSummaryDto order)
            {
                try
                {
                    await _orders.CancelOrderAsync(order.OrderId);
                     if (DataContext is OrderViewModel vm)
                        await vm.LoadOrdersAsync();
                 }
                catch (Exception ex)
                {
                    var dlg = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Cancel failed",
                        Content = ex.Message,
                        CloseButtonText = "OK"
                    };
                    await dlg.ShowAsync();
                }
            }
        }
    }
}
