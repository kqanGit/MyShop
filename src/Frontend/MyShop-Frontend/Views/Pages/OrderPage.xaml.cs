using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Printing;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.ViewModels.Orders;
using MyShop_Frontend.Views.Controls.Order;
using System;
using System.Collections.Generic;
using Microsoft.UI.Text;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using System.Threading.Tasks;
using System.Text;

namespace MyShop_Frontend.Views.Pages
{
    public sealed partial class OrderPage : Page
    {
        private readonly IApiClient _api;
        private readonly IOrderService _orders;

        private PrintDocument? _printDocument;
        private IPrintDocumentSource? _printDocSource;
        private UIElement? _printContent;
        private bool _printRegistered;

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
                        PrimaryButtonText = "Export CSV",
                        CloseButtonText = "Close",
                        DefaultButton = ContentDialogButton.Primary
                    };

                    var result = await dlg.ShowAsync();

                    if (result == ContentDialogResult.Primary)
                    {
                        await ExportCsvAsync(detail);
                    }
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

        private async void PrintOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuFlyoutItem item || item.CommandParameter is not OrderSummaryDto order)
                return;

            try
            {
                var detail = await _orders.GetOrderByIdAsync(order.OrderId);
                await ExportCsvAsync(detail);
            }
            catch (Exception ex)
            {
                var dlg = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Export failed",
                    Content = ex.Message,
                    CloseButtonText = "OK"
                };
                await dlg.ShowAsync();
            }
        }

        private async Task ExportCsvAsync(OrderResponseDto detail)
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = $"Order_{detail.OrderCode}"
            };
            picker.FileTypeChoices.Add("CSV", new List<string> { ".csv" });

            if (App.MainWindow is null) return;
            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSaveFileAsync();
            if (file == null) return;

            var sb = new StringBuilder();
            string Quote(string input) => $"\"{(input ?? string.Empty).Replace("\"", "\"\"")}\"";

            sb.AppendLine("OrderCode,Date,Status,Voucher,Total,Discount,Final");
            sb.AppendLine($"{Quote(detail.OrderCode)}," +
                           $"{Quote(detail.OrderDate.ToString("O"))}," +
                           $"{Quote(detail.Status)}," +
                           $"{Quote(detail.VoucherCode ?? "")}," +
                           $"{detail.TotalPrice}," +
                           $"{detail.DiscountAmount}," +
                           $"{detail.FinalPrice}");
            sb.AppendLine();
            sb.AppendLine("Product,Quantity,Price,TotalLine");
            foreach (var line in detail.OrderDetails ?? new List<OrderDetailResponseDto>())
            {
                sb.AppendLine($"{Quote(line.ProductName)}," +
                              $"{line.Quantity}," +
                              $"{line.PriceAtPurchase}," +
                              $"{line.TotalLine}");
            }

            var csv = sb.ToString();
            var utf8 = Encoding.UTF8;
            var preamble = utf8.GetPreamble();
            var body = utf8.GetBytes(csv);
            var bytes = new byte[preamble.Length + body.Length];
            Buffer.BlockCopy(preamble, 0, bytes, 0, preamble.Length);
            Buffer.BlockCopy(body, 0, bytes, preamble.Length, body.Length);

            await FileIO.WriteBytesAsync(file, bytes);
        }

        private void RegisterPrint()
        {
            if (_printRegistered) return;
            var manager = PrintManager.GetForCurrentView();
            manager.PrintTaskRequested += OnPrintTaskRequested;
            _printRegistered = true;
        }

        private void OnPrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            args.Request.CreatePrintTask("Invoice", printTaskArgs =>
            {
                printTaskArgs.SetSource(_printDocSource);
            });
        }

        private void PreparePrintContent(OrderResponseDto detail)
        {
            _printDocument ??= new PrintDocument();
            _printDocument.Paginate += PrintDocument_Paginate;
            _printDocument.GetPreviewPage += PrintDocument_GetPreviewPage;
            _printDocument.AddPages += PrintDocument_AddPages;

            _printDocSource = _printDocument.DocumentSource;

            var panel = new StackPanel { Spacing = 8, Padding = new Thickness(12) };

            panel.Children.Add(new TextBlock
            {
                Text = $"Order: {detail.OrderCode}",
                FontSize = 18,
                FontWeight = FontWeights.SemiBold
            });
            panel.Children.Add(new TextBlock { Text = $"Date: {detail.OrderDate:dd/MM/yyyy HH:mm}" });
            panel.Children.Add(new TextBlock { Text = $"Status: {detail.Status}" });
            panel.Children.Add(new TextBlock { Text = $"Voucher: {detail.VoucherCode ?? "None"}" });
            panel.Children.Add(new TextBlock { Text = $"Total: {detail.TotalPrice:N0}  Discount: {detail.DiscountAmount:N0}  Final: {detail.FinalPrice:N0}" });

            panel.Children.Add(new TextBlock
            {
                Text = "Items",
                Margin = new Thickness(0, 8, 0, 4),
                FontWeight = FontWeights.SemiBold
            });

            foreach (var line in detail.OrderDetails ?? new List<OrderDetailResponseDto>())
            {
                panel.Children.Add(new TextBlock
                {
                    Text = $"- {line.ProductName} x{line.Quantity} @ {line.PriceAtPurchase:N0} = {line.TotalLine:N0}",
                    TextWrapping = TextWrapping.WrapWholeWords
                });
            }

            _printContent = panel;

            // Invalidate preview
            _printDocument.InvalidatePreview();
        }

        private void PrintDocument_Paginate(object sender, PaginateEventArgs e)
        {
            if (_printDocument == null || _printContent == null) return;
            _printDocument.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void PrintDocument_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            if (_printDocument == null || _printContent == null) return;
            _printDocument.SetPreviewPage(e.PageNumber, _printContent);
        }

        private void PrintDocument_AddPages(object sender, AddPagesEventArgs e)
        {
            if (_printDocument == null || _printContent == null) return;
            _printDocument.AddPage(_printContent);
            _printDocument.AddPagesComplete();
        }
    }
}
