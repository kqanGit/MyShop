using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.MockData;
using MyShop_Frontend.Models;
using Windows.System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Views.Controls.Order
{
    public sealed partial class CreateOrderDialogControl : ContentDialog, INotifyPropertyChanged
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IVoucherService _voucherService;

        private readonly List<Product> _allProducts = new();
        private readonly List<Category> _allCategories = new();
        private readonly ObservableCollection<CustomerDto> _customerSuggestions = new();

        private bool _isOffline;
        private string _search = string.Empty;
        private Category? _selectedCategory;

        private int _selectedCustomerId = 1;
        private string _selectedCustomerName = "Walk-in customer";
        public string SelectedCustomerName
        {
            get => _selectedCustomerName;
            private set
            {
                if (_selectedCustomerName == value) return;
                _selectedCustomerName = value;
                OnPropertyChanged();
            }
        }

        private string _customerHintText = "Walk-in customer";
        public string CustomerHintText
        {
            get => _customerHintText;
            private set
            {
                if (_customerHintText == value) return;
                _customerHintText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProductCardVm> ProductCards { get; } = new();
        public ObservableCollection<InvoiceLineVm> Lines { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        // Expose created order info to parent
        public int? CreatedOrderId { get; private set; }
        public string? CreatedOrderCode { get; private set; }

        // Parent (ContentDialog/Popup) nghe event này ð? ðóng
        public event EventHandler? RequestClose;

        private VoucherDto? _appliedVoucher;
        private decimal _voucherDiscountAmount;
        private CancellationTokenSource? _voucherCts;

        public CreateOrderDialogControl()
        {
            InitializeComponent();

            _productService = App.Services.GetRequiredService<IProductService>();
            _categoryService = App.Services.GetRequiredService<ICategoryService>();
            _orderService = App.Services.GetRequiredService<IOrderService>();
            _customerService = App.Services.GetRequiredService<ICustomerService>();
            _voucherService = App.Services.GetRequiredService<IVoucherService>();

            CustomerBox.ItemsSource = _customerSuggestions;
            SetSelectedCustomer(1, "Walk-in customer");

            InvoiceList.ItemsSource = Lines;
            ProductsRepeater.ItemsSource = ProductCards;

            // theo d?i ð?i qty ð? update totals + remaining stock
            Lines.CollectionChanged += (_, __) =>
            {
                RaiseTotals();
                SyncRemainingStockToCards();
            };

            Closing += CreateOrderDialogControl_Closing;

            _ = InitializeAsync();
        }

        private void CreateOrderDialogControl_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.None)
            {
                // Close button clicked - allow closing
                return;
            }
        }

        // ===== Totals =====
        private decimal Subtotal => Lines.Sum(l => l.Price * l.Qty);

        private decimal Discount => _voucherDiscountAmount;

        private decimal Total => Math.Max(Subtotal - Discount, 0m);

        public string SubtotalText => FormatCurrency(Subtotal);
        public string DiscountText => Discount <= 0 ? FormatCurrency(0m) : "-" + FormatCurrency(Discount);
        public string TotalText => FormatCurrency(Total);

        private void RaiseTotals()
        {
            RecalculateVoucherDiscount();
            OnPropertyChanged(nameof(SubtotalText));
            OnPropertyChanged(nameof(DiscountText));
            OnPropertyChanged(nameof(TotalText));
        }

        private void RecalculateVoucherDiscount()
        {
            if (_appliedVoucher == null)
            {
                _voucherDiscountAmount = 0;
                return;
            }

            var subtotal = Subtotal;
            if (subtotal <= 0 || subtotal < _appliedVoucher.MinThreshold)
            {
                _voucherDiscountAmount = 0;
                return;
            }

            var calc = _appliedVoucher.Type == 2
                ? _appliedVoucher.Discount
                : subtotal * (_appliedVoucher.Discount / 100m);

            _voucherDiscountAmount = Math.Clamp(calc, 0, subtotal);
        }

        private static string FormatCurrency(decimal value)
        {
            var vi = CultureInfo.GetCultureInfo("vi-VN");
            return value.ToString("N0", vi) + " þ";
        }

        private async Task InitializeAsync()
        {
            await LoadCatalogAsync();
            await LoadCustomerSuggestionsAsync(string.Empty);
            ApplyProductFilter();
            SyncRemainingStockToCards();
        }

        private async Task LoadCatalogAsync()
        {
            _allProducts.Clear();
            _allCategories.Clear();

            Dictionary<int, string> categoryNameById = new();

            try
            {
                // Online
                var catTask = _categoryService.GetCategoriesAsync();
                var prodTask = _productService.GetProductsAsync(pageIndex: 1, pageSize: 500);

                await Task.WhenAll(catTask, prodTask);

                _allCategories.AddRange(catTask.Result);

                categoryNameById = _allCategories
                    .GroupBy(c => c.CategoryId)
                    .ToDictionary(g => g.Key, g => g.First().CategoryName);

                _allProducts.AddRange(
                    (prodTask.Result?.Items ?? new List<Product>())
                        .Where(p => !p.IsRemoved)
                        .Select(p =>
                        {
                            if (string.IsNullOrWhiteSpace(p.CategoryName) &&
                                categoryNameById.TryGetValue(p.CategoryId, out var name))
                            {
                                p.CategoryName = name;
                            }
                            return p;
                        })
                );

                _isOffline = false;
            }
            catch
            {
                // Offline fallback
                _isOffline = true;

                _allProducts.AddRange(Product_MockData.GetProducts().Where(p => !p.IsRemoved));

                categoryNameById = new Dictionary<int, string>
                {
                    [1] = "Th?c ph?m & Ð? u?ng",
                    [2] = "Hóa ph?m & Chãm sóc cá nhân",
                    [3] = "Ð? dùng gia ð?nh"
                };

                foreach (var p in _allProducts)
                {
                    if (string.IsNullOrWhiteSpace(p.CategoryName) &&
                        categoryNameById.TryGetValue(p.CategoryId, out var name))
                        p.CategoryName = name;
                    else if (string.IsNullOrWhiteSpace(p.CategoryName))
                        p.CategoryName = $"Category {p.CategoryId}";
                }

                var derived = _allProducts
                    .GroupBy(p => new { p.CategoryId, p.CategoryName })
                    .Select(g => new Category
                    {
                        CategoryId = g.Key.CategoryId,
                        CategoryName = g.Key.CategoryName ?? $"Category {g.Key.CategoryId}"
                    })
                    .OrderBy(c => c.CategoryName)
                    .ToList();

                _allCategories.AddRange(derived);
            }

            // Build category combobox (include All)
            var comboCats = new List<Category>
            {
                new Category { CategoryId = 0, CategoryName = "All" }
            };
            comboCats.AddRange(_allCategories.OrderBy(c => c.CategoryName));

            CategoryBox.ItemsSource = comboCats;
            CategoryBox.SelectedIndex = 0;
            _selectedCategory = comboCats.FirstOrDefault();

            VoucherHint.Text = _isOffline
                ? "Offline: voucher not validated"
                : "Voucher will be applied on server";
        }

        // ===== Filter/Search =====
        private void ApplyProductFilter()
        {
            IEnumerable<Product> q = _allProducts;

            var cat = _selectedCategory;
            if (cat != null && cat.CategoryId != 0)
                q = q.Where(p => p.CategoryId == cat.CategoryId);

            var s = (_search ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(s))
                q = q.Where(p => (p.ProductName ?? "").Contains(s, StringComparison.OrdinalIgnoreCase));

            ProductCards.Clear();

            foreach (var p in q.OrderBy(p => p.ProductName))
                ProductCards.Add(ProductCardVm.From(p));

            // c?p nh?t stock c?n l?i theo Lines hi?n t?i
            SyncRemainingStockToCards();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _search = SearchBox.Text ?? string.Empty;
            ApplyProductFilter();
        }

        private void CategoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCategory = CategoryBox.SelectedItem as Category;
            ApplyProductFilter();
        }

        // ===== Add product -> invoice =====
        private void ProductCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.DataContext is not ProductCardVm card) return;

            // card.Stock ðang là stock c?n l?i
            if (card.Stock <= 0)
            {
                _ = ShowInlineErrorAsync("This product is out of stock.");
                return;
            }

            var existing = Lines.FirstOrDefault(l => l.ProductId == card.ProductId);
            if (existing != null)
            {
                if (existing.Qty < existing.Stock)
                    existing.Qty += 1;

                // totals + remaining stock s? c?p nh?t qua Line_PropertyChanged
                return;
            }

            // Khi t?o line, Stock = original stock (gi?i h?n qty theo stock g?c)
            var line = new InvoiceLineVm(card.ProductId, card.ProductName, card.OriginalStock, card.Price);
            line.PropertyChanged += Line_PropertyChanged;
            Lines.Add(line);

            RaiseTotals();
            SyncRemainingStockToCards();
        }

        private void Line_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InvoiceLineVm.Qty))
            {
                RaiseTotals();
                SyncRemainingStockToCards();
            }
        }

        private void QtyMinus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is InvoiceLineVm line)
            {
                if (line.Qty > 1) line.Qty--;
            }
        }

        private void QtyPlus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is InvoiceLineVm line)
            {
                if (line.Qty < line.Stock) line.Qty++;
            }
        }

        private void RemoveLine_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is InvoiceLineVm line)
            {
                line.PropertyChanged -= Line_PropertyChanged;
                Lines.Remove(line);
                RaiseTotals();
                SyncRemainingStockToCards();
            }
        }

        // Tính “stock c?n l?i” cho các card: remaining = original - reserved
        private void SyncRemainingStockToCards()
        {
            var reservedByProductId = Lines
                .GroupBy(l => l.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Qty));

            foreach (var card in ProductCards)
            {
                reservedByProductId.TryGetValue(card.ProductId, out var reserved);
                var remaining = Math.Max(card.OriginalStock - reserved, 0);

                // c?p nh?t Stock (bound trên UI) -> hi?n th? stock c?n l?i
                card.Stock = remaining;
            }
        }

        // ===== Voucher =====
        private void VoucherBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var code = VoucherBox.Text?.Trim() ?? "";
            _ = ValidateVoucherAsync(code);
        }

        private void VoucherBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var code = VoucherBox.Text?.Trim() ?? "";
                _ = ValidateVoucherAsync(code);
                e.Handled = true;
            }
        }

        private async Task ValidateVoucherAsync(string code)
        {
            _voucherCts?.Cancel();
            _voucherCts = new CancellationTokenSource();
            var ct = _voucherCts.Token;

            if (string.IsNullOrWhiteSpace(code))
            {
                _appliedVoucher = null;
                _voucherDiscountAmount = 0;
                VoucherHint.Text = _isOffline ? "Offline: voucher not validated" : "";
                RaiseTotals();
                return;
            }

            if (_isOffline)
            {
                VoucherHint.Text = "Offline: voucher not validated";
                _appliedVoucher = null;
                _voucherDiscountAmount = 0;
                RaiseTotals();
                return;
            }

            try
            {
                var resp = await _voucherService.CheckVoucherAsync(code, ct);
                if (resp?.IsValid == true && resp.Voucher != null)
                {
                    _appliedVoucher = resp.Voucher;
                    RecalculateVoucherDiscount();
                    VoucherHint.Text = $"Discount: {FormatCurrency(_voucherDiscountAmount)} (Min {FormatCurrency(_appliedVoucher.MinThreshold)})";
                    OnPropertyChanged(nameof(DiscountText));
                    OnPropertyChanged(nameof(TotalText));
                }
                else
                {
                    _appliedVoucher = null;
                    _voucherDiscountAmount = 0;
                    VoucherHint.Text = resp?.Message ?? "Voucher invalid";
                    OnPropertyChanged(nameof(DiscountText));
                    OnPropertyChanged(nameof(TotalText));
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                _appliedVoucher = null;
                _voucherDiscountAmount = 0;
                VoucherHint.Text = ex.Message;
                OnPropertyChanged(nameof(DiscountText));
                OnPropertyChanged(nameof(TotalText));
            }

            RaiseTotals();
        }

        // ===== Actions =====
        private void Cancel_Click(object sender, RoutedEventArgs e)
            => _ = SubmitAsync(statusCode: 3);

        private void Create_Click(object sender, RoutedEventArgs e)
            => _ = SubmitAsync(statusCode: 1);

        private void Pay_Click(object sender, RoutedEventArgs e)
            => _ = SubmitAsync(statusCode: 2);

        private async Task SubmitAsync(int statusCode, CancellationToken ct = default)
        {
            if (Lines.Count == 0)
            {
                await ShowInlineErrorAsync("Please select at least 1 product.");
                return;
            }

            if (Lines.Any(l => l.Qty < 1))
            {
                await ShowInlineErrorAsync("Quantity must be >= 1.");
                return;
            }

            if (Lines.Any(l => l.Qty > l.Stock))
            {
                await ShowInlineErrorAsync("Quantity exceeds stock.");
                return;
            }

            var customerId = _selectedCustomerId <= 0 ? 1 : _selectedCustomerId;
            var voucher = string.IsNullOrWhiteSpace(VoucherBox.Text) ? null : VoucherBox.Text.Trim();

            var req = new CreateOrderRequest
            {
                CustomerId = customerId,
                VoucherCode = voucher,
                Note = null,
                Status = statusCode,
                Items = Lines.Select(l => new CartItemDto
                {
                    ProductId = l.ProductId,
                    Quantity = l.Qty
                }).ToList()
            };

            if (_isOffline)
            {
                await ShowInlineInfoAsync(
                    statusCode == 2
                        ? "Offline mode: order created locally. Payment is not available offline."
                        : "Offline mode: order saved locally (not synced).",
                    closeAfter: true);
                return;
            }

            try
            {
                var result = await _orderService.CreateOrderAsync(req, ct);

                CreatedOrderId = result?.OrderId;
                CreatedOrderCode = result?.OrderCode;


                string msg = statusCode switch
                {
                    2 => $"Order {result?.OrderCode ?? ""} created and marked as Paid.",
                    3 => $"Order {result?.OrderCode ?? ""} created and marked as Canceled.",
                    _ => $"Order {result?.OrderCode ?? ""} created."
                };

                await ShowInlineInfoAsync(msg, closeAfter: true);
            }
            catch (Exception ex)
            {
                _isOffline = true;
                await ShowInlineInfoAsync(
                    $"Network/server error. Saved locally (not synced).\n{ex.Message}",
                    closeAfter: true);
            }
        }

        private async Task ShowInlineErrorAsync(string message)
        {
            var dlg = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Title = "Create order",
                Content = message,
                CloseButtonText = "OK"
            };
            await dlg.ShowAsync();
        }

        private async Task ShowInlineInfoAsync(string message, bool closeAfter)
        {
            if (closeAfter)
            {
                RequestClose?.Invoke(this, EventArgs.Empty);
                try { Hide(); } catch { }
                return;
            }

            var dlg = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Title = "Create order",
                Content = message,
                CloseButtonText = "OK"
            };
            await dlg.ShowAsync();
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // ===== Customers (searchable) =====
        private async Task LoadCustomerSuggestionsAsync(string term, CancellationToken ct = default)
        {
            _customerSuggestions.Clear();

            // Always include walk-in
            _customerSuggestions.Add(new CustomerDto { CustomerId = 1, FullName = "Walk-in customer", Phone = "", Address = "" });

            if (_isOffline) return;

            try
            {
                PagedResult<CustomerDto>? resp;
                if (string.IsNullOrWhiteSpace(term))
                {
                    // load first page of customers
                    resp = await _customerService.GetCustomersAsync(pageIndex: 1, pageSize: 50, ct: ct);
                }
                else
                {
                    resp = await _customerService.SearchCustomersAsync(pageIndex: 1, pageSize: 50, phone: term, name: term, ct: ct);
                }

                if (resp?.Items != null)
                {
                    foreach (var c in resp.Items)
                    {
                        if (c.CustomerId == 1) continue;
                        _customerSuggestions.Add(c);
                    }
                }
            }
            catch
            {
                // fallback: keep only walk-in
            }
        }

        private void SetSelectedCustomer(int id, string name)
        {
            _selectedCustomerId = id;
            SelectedCustomerName = string.IsNullOrWhiteSpace(name) ? "Walk-in customer" : name;
            CustomerHintText = _selectedCustomerId == 1 ? "Walk-in customer" : $"Selected: {SelectedCustomerName} (ID {_selectedCustomerId})";
        }

        private async void CustomerBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            if (e.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;
            await LoadCustomerSuggestionsAsync(CustomerBox.Text?.Trim() ?? string.Empty);
        }

        private void CustomerBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is CustomerDto c)
            {
                SetSelectedCustomer(c.CustomerId, c.FullName);
            }
        }

        private async void CustomerBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is CustomerDto chosen)
            {
                SetSelectedCustomer(chosen.CustomerId, chosen.FullName);
                return;
            }

            // If user just types and presses enter, fetch and pick first match (or default walk-in)
            await LoadCustomerSuggestionsAsync(args.QueryText ?? string.Empty);
            var first = _customerSuggestions.FirstOrDefault();
            if (first != null)
                SetSelectedCustomer(first.CustomerId, first.FullName);
        }
    }

    // ===== View-models for dialog (UI-only) =====

    // Card VM: implement INPC ð? update Stock c?n l?i ngay trên UI
    public sealed class ProductCardVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string PriceText => Price.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " þ";

        // Stock hi?n th? trên UI = stock c?n l?i
        private int _stock;
        public int Stock
        {
            get => _stock;
            set
            {
                if (_stock == value) return;
                _stock = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stock)));
            }
        }

        // Stock g?c ð? gi?i h?n qty + tính remaining
        public int OriginalStock { get; set; }

        public string? Image { get; set; }

        public static ProductCardVm From(Product p) => new()
        {
            ProductId = p.ProductId,
            CategoryId = p.CategoryId,
            CategoryName = string.IsNullOrWhiteSpace(p.CategoryName) ? $"Category {p.CategoryId}" : p.CategoryName,
            ProductName = p.ProductName ?? string.Empty,
            Price = p.Price,
            OriginalStock = p.Stock,
            Stock = p.Stock,
            Image = p.Image
        };
    }

    public sealed class InvoiceLineVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int ProductId { get; }
        public string ProductName { get; }
        public int Stock { get; }              // stock g?c (gi?i h?n qty)
        public decimal Price { get; }

        public string PriceText => Price.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " þ";

        public decimal LineTotal => Price * Qty;
        public string LineTotalText => LineTotal.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " þ";

        private int _qty = 1;
        public int Qty
        {
            get => _qty;
            set
            {
                var next = value;
                if (next < 1) next = 1;
                if (next > Stock) next = Stock;
                if (Stock <= 0) next = 0;

                if (_qty == next) return;
                _qty = next;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Qty)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineTotal)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineTotalText)));
            }
        }

        public InvoiceLineVm(int productId, string productName, int stock, decimal price)
            => (ProductId, ProductName, Stock, Price) = (productId, productName, stock, price);
    }
}
