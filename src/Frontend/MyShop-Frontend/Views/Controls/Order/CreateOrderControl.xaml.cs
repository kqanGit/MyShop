using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyShop_Frontend.Views.Controls.Order
{
    public sealed partial class CreateOrderControl : UserControl
    {
        public ObservableCollection<OrderItemRow> Items { get; } = new();

        // demo: giảm giá cố định (bạn thay bằng coupon/level khách sau)
        private decimal _discount = 3.02m;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CreateOrderControl()
        {
            InitializeComponent();

            // Demo data giống ảnh
            Items.Add(new OrderItemRow("Organic Whole Milk (1 Gallon)", 50, 2, 9.98m));
            Items.Add(new OrderItemRow("Fresh Bananas (per lb)", 25, 5, 2.95m));
            Items.Add(new OrderItemRow("Premium Ground Beef (1 lb)", 30, 3, 20.97m));
            Items.Add(new OrderItemRow("Fresh Strawberries (16 oz)", 45, 4, 15.96m));
            Items.Add(new OrderItemRow("Coca-Cola 12-Pack Cans", 80, 2, 10.98m));

            foreach (var it in Items)
                it.PropertyChanged += Item_PropertyChanged;

            RecalcTotals();
        }

        // ====== Totals (bind lên UI) ======
        private decimal Subtotal => Items.Sum(i => i.Qty * i.Price);
        private decimal Total => Subtotal - _discount;

        public string SubtotalText => $"${Subtotal:0.00}";
        public string DiscountText => $"-${_discount:0.00}";
        public string TotalText => $"${Total:0.00}";

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderItemRow.Qty))
                RecalcTotals();
        }

        private void RecalcTotals()
        {
            OnPropertyChanged(nameof(SubtotalText));
            OnPropertyChanged(nameof(DiscountText));
            OnPropertyChanged(nameof(TotalText));
        }

        private void QtyMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is OrderItemRow row)
            {
                if (row.Qty > 0) row.Qty--;
            }
        }

        private void QtyPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is OrderItemRow row)
            {
                // optional: chặn vượt stock
                if (row.Qty < row.Stock) row.Qty++;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // ====== Helper để lấy data từ các textbox (khi bấm Create) ======
        public CreateOrderFormData GetFormData()
        {
            return new CreateOrderFormData(
                fullName: FullNameBox.Text?.Trim() ?? "",
                phone: PhoneBox.Text?.Trim() ?? "",
                payment: (PaymentCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                notes: NotesBox.Text ?? "",
                coupon: CouponBox.Text?.Trim() ?? "",
                items: Items.ToList(),
                subtotal: Subtotal,
                discount: _discount,
                total: Total
            );
        }
    }

    public sealed class OrderItemRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; }
        public int Stock { get; }
        public decimal Price { get; }
        public string PriceText => $"${Price:0.00}";

        private int _qty;
        public int Qty
        {
            get => _qty;
            set
            {
                if (_qty == value) return;
                _qty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Qty)));
            }
        }

        public OrderItemRow(string name, int stock, int qty, decimal price)
            => (Name, Stock, _qty, Price) = (name, stock, qty, price);
    }

    public sealed record CreateOrderFormData(
        string fullName,
        string phone,
        string payment,
        string notes,
        string coupon,
        System.Collections.Generic.List<OrderItemRow> items,
        decimal subtotal,
        decimal discount,
        decimal total
    );
}
