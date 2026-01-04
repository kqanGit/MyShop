using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyShop_Frontend.Views.Controls.Order;
using System;

namespace MyShop_Frontend.Views.Dialogs
{
    public static class CreateOrderDialog
    {
        public static ContentDialog Create(XamlRoot xamlRoot, EventHandler? onClosed = null)
        {
            var view = new CreateOrderDialogControl();

            // Responsive dialog size
            var bounds = xamlRoot?.Size ?? new global::Windows.Foundation.Size(1200, 800);
            var targetWidth = Math.Clamp(bounds.Width * 0.92, 980, 1600);
            var targetHeight = Math.Clamp(bounds.Height * 0.90, 640, 900);

            var host = new Grid
            {
                Width = targetWidth,
                Height = targetHeight
            };
            host.Children.Add(view);

            var dlg = new ContentDialog
            {
                XamlRoot = xamlRoot,
                Title = "Create Order",
                Content = host,
                CloseButtonText = "Close",
                DefaultButton = ContentDialogButton.Close
            };

            view.RequestClose += (_, __) => dlg.Hide();
            if (onClosed is not null)
            {
                dlg.Closed += (_, __) => onClosed(dlg, EventArgs.Empty);
            }

            return dlg;
        }
    }
}
