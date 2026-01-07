using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.UI.Xaml.Controls;

namespace MyShop_Frontend.ViewModels
{
    public class SearchResult
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Customer", "Order", "Product"
        public object Data { get; set; } // The actual object
        public string IconGlyph { get; set; } = "\uE721"; // Default search icon
    }

    public class MainViewModel : ViewModelBase
    {
        private readonly WindowsService _windowsService;
        private readonly ITokenStore _tokenStore;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        // Command xử lý đăng xuất
        public RelayCommand SignOutCommand { get; }

        // === Thông tin người dùng ===
        private string _username = "User";
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        private string _userRole = "Staff";
        public string UserRole
        {
            get => _userRole;
            set { _userRole = value; OnPropertyChanged(nameof(UserRole)); }
        }

        public string RoleDisplayName => UserRole switch
        {
            "1" => "Quản trị viên",
            "2" => "Quản lý",
            "3" => "Nhân viên",
            _ => "Người dùng"
        };

        // === SEARCH ===
        private string _searchText = "";
     
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    // Pass the value directly to avoid race conditions capturing the property later
                    _ = SearchAsync(_searchText); 
                }
            }
        }

        public ObservableCollection<SearchResult> SearchResults { get; } = new();

        public MainViewModel()
        {
            _windowsService = App.Windows;
            // Resolve services from App.Services since MainViewModel is often created in XAML or manually
            _tokenStore = App.Services.GetService<ITokenStore>();
            _customerService = App.Services.GetService<ICustomerService>();
            _productService = App.Services.GetService<IProductService>();
            _orderService = App.Services.GetService<IOrderService>();

            SignOutCommand = new RelayCommand(_ => ExecuteSignOut());

            // Load thông tin user từ TokenStore
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            if (_tokenStore == null) return;
            var username = _tokenStore.GetUsername();
            var role = _tokenStore.GetRole();

            Username = string.IsNullOrEmpty(username) ? "User" : username;
            UserRole = string.IsNullOrEmpty(role) ? "Staff" : role;
            
            OnPropertyChanged(nameof(RoleDisplayName));
        }

        private void ExecuteSignOut()
        {
            _tokenStore?.Clear();
            _windowsService.ShowAuthWindow();
        }

        private System.Threading.CancellationTokenSource? _searchCts;

        private async Task SearchAsync(string query)
        {
            // Cancel previous search
            _searchCts?.Cancel();
            _searchCts = new System.Threading.CancellationTokenSource();
            var token = _searchCts.Token;

            if (string.IsNullOrWhiteSpace(query) || query.Length < 2) 
            {
                SearchResults.Clear();
                return;
            }

            try
            {
                // Debounce
                await Task.Delay(300, token);

                // Fetch data using the captured 'query' and passing 'token'
                var customerTask = _customerService?.SearchCustomersAsync(pageIndex: 1, pageSize: 5, name: query, ct: token);
                // Fetch more products to filter client-side since API filter is unreliable
                var productTask = _productService?.GetProductsAsync(keyword: query, pageIndex: 1, pageSize: 1000, ct: token);
                var orderTask = _orderService?.GetOrdersAsync(new GetOrdersRequest { SearchTerm = query, PageIndex = 1, PageSize = 5 }, ct: token);

                // We await tasks but careful about exceptions in them
                // Also, we can run them in parallel
                
                // Clear existing only when we are about to show new results or incrementally? 
                // Better to clear before adding new ones, but maybe after we get at least one response?
                // For simplicity, let's clear at start of result processing
                
                // Wait for all to complete or process as they come?
                // Let's safe wait
                // Note: If any task throws, others might still be running.
                
                // Simple sequential for safety against concurrency on SearchResults collection
                // (Or use dispatcher, but we are in ViewModel)

                var customers = customerTask != null ? await customerTask : null;
                if (token.IsCancellationRequested) return;

                var products = productTask != null ? await productTask : null;
                if (token.IsCancellationRequested) return;

                var orders = orderTask != null ? await orderTask : null;
                if (token.IsCancellationRequested) return;

                // Update UI
                SearchResults.Clear();

                if (customers != null)
                {
                    foreach (var c in customers.Items)
                    {
                        SearchResults.Add(new SearchResult 
                        { 
                            Title = c.FullName, 
                            Description = $"Customer - {c.Phone}", 
                            Type = "Customer", 
                            Data = c,
                            IconGlyph = "\uE716"
                        });
                    }
                }

                if (products != null)
                {
                    // Client-side filter for Products (API might be unreliable/ignoring keyword)
                    var filteredProducts = products.Items
                        .Where(p => (p.ProductName ?? "").Contains(query, StringComparison.OrdinalIgnoreCase))
                        .Take(5);

                    foreach (var p in filteredProducts)
                    {
                        SearchResults.Add(new SearchResult 
                        { 
                            Title = p.ProductName, 
                            Description = $"Product - {p.Price:C}", 
                            Type = "Product", 
                            Data = p,
                            IconGlyph = "\uE821" 
                        });
                    }
                }

                if (orders != null)
                {
                    foreach (var o in orders.Items)
                    {
                        SearchResults.Add(new SearchResult 
                        { 
                            Title = o.OrderCode, 
                            Description = $"Order - {o.FinalPrice:C}", 
                            Type = "Order", 
                            Data = o,
                            IconGlyph = "\uE7BF"
                        });
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // Ignore
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Search Error: {ex.Message}");
            }
        }
    }
}