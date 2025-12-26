using MyShop_Frontend.Models;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Services;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyShop_Frontend.ViewModels.Orders
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private bool _isBusy;
        private string _searchText;
        private DateTimeOffset? _fromDate;
        private DateTimeOffset? _toDate;

        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    LoadOrdersAsync();
                }
            }
        }

        public DateTimeOffset? FromDate
        {
            get => _fromDate;
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    OnPropertyChanged(nameof(FromDate));
                    LoadOrdersAsync();
                }
            }
        }

        public DateTimeOffset? ToDate
        {
            get => _toDate;
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged(nameof(ToDate));
                    LoadOrdersAsync();
                }
            }
        }

        public RelayCommand ClearFilterCommand { get; }

        public OrderViewModel()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5126/")
            };

            _fromDate = new DateTimeOffset(new DateTime(2025, 1, 1));
            _toDate = DateTimeOffset.Now;

            ClearFilterCommand = new RelayCommand(_ => ClearFilters());
            LoadOrdersAsync();
        }

        private void ClearFilters()
        {
            _searchText = string.Empty;
            _fromDate = new DateTimeOffset(new DateTime(2025, 1, 1));
            _toDate = DateTimeOffset.Now;
            OnPropertyChanged(nameof(SearchText));
            OnPropertyChanged(nameof(FromDate));
            OnPropertyChanged(nameof(ToDate));
            LoadOrdersAsync();
        }

        public async Task LoadOrdersAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Ensure token is available
                var token = AuthenticationService.CurrentToken;
                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("No access token available.");
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (FromDate.HasValue && ToDate.HasValue && ToDate < FromDate)
                {
                    // Invalid date range: ToDate is before FromDate
                    // For now, we can just return or clear data, or maybe swap them?
                    // User said "invalid". Let's explicitly not load and maybe show empty or keep previous.
                    // Better interaction: Just return.
                    Debug.WriteLine("Invalid Date Range: ToDate < FromDate");
                    return;
                }

                var query = "api/Orders?pageIndex=1&pageSize=10";
                if (!string.IsNullOrEmpty(SearchText))
                {
                    query += $"&search={Uri.EscapeDataString(SearchText)}";
                }
                if (FromDate.HasValue)
                {
                    query += $"&from={FromDate.Value:yyyy-MM-dd}";
                }
                if (ToDate.HasValue)
                {
                    query += $"&to={ToDate.Value:yyyy-MM-dd}";
                }

                var response = await _httpClient.GetFromJsonAsync<OrderResponse>(query);

                if (response != null && response.Items != null)
                {
                    Orders.Clear();
                    
                    // Client-side filtering as a fallback/enforcement
                    var items = response.Items;

                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        items = items.Where(o => o.OrderCode != null && 
                                                 o.OrderCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    if (FromDate.HasValue)
                    {
                        items = items.Where(o => o.OrderDate.Date >= FromDate.Value.Date).ToList();
                    }
                    if (ToDate.HasValue)
                    {
                        items = items.Where(o => o.OrderDate.Date <= ToDate.Value.Date).ToList();
                    }

                    foreach (var order in items)
                    {
                        Orders.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading orders: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
