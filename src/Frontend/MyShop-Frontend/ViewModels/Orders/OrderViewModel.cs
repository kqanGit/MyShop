using Microsoft.Extensions.DependencyInjection;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Helpers.Command;
using MyShop_Frontend.Models;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MyShop_Frontend.ViewModels.Orders
{
    public class OrderViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;

        private bool _isBusy;
        private string _searchText = string.Empty;
        private DateTimeOffset? _fromDate;
        private DateTimeOffset? _toDate;
        private string? _errorMessage;

        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();

        public RelayCommand ClearFilterCommand { get; }

        public OrderViewModel()
        {
            _apiClient = App.Services.GetRequiredService<IApiClient>();

            _fromDate = new DateTimeOffset(new DateTime(2025, 1, 1));
            _toDate = DateTimeOffset.Now;

            ClearFilterCommand = new RelayCommand(_ => ClearFilters());

            _ = LoadOrdersAsync();
        }

        // ===== Helpers =====
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name ?? string.Empty);
            return true;
        }

        // ===== Properties =====
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    _ = LoadOrdersAsync();
                }
            }
        }

        public DateTimeOffset? FromDate
        {
            get => _fromDate;
            set
            {
                if (SetProperty(ref _fromDate, value))
                {
                    _ = LoadOrdersAsync();
                }
            }
        }

        public DateTimeOffset? ToDate
        {
            get => _toDate;
            set
            {
                if (SetProperty(ref _toDate, value))
                {
                    _ = LoadOrdersAsync();
                }
            }
        }

        private void ClearFilters()
        {
            SearchText = string.Empty; // sẽ auto trigger LoadOrdersAsync
            FromDate = new DateTimeOffset(new DateTime(2025, 1, 1));
            ToDate = DateTimeOffset.Now;
        }

        public async Task LoadOrdersAsync()
        {
            if (IsBusy) return;

            // validate date range
            if (FromDate.HasValue && ToDate.HasValue && ToDate.Value.Date < FromDate.Value.Date)
            {
                ErrorMessage = "Khoảng ngày không hợp lệ: ToDate nhỏ hơn FromDate.";
                return;
            }

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                var query = "api/Orders?pageIndex=1&pageSize=10";

                if (!string.IsNullOrWhiteSpace(SearchText))
                    query += $"&search={Uri.EscapeDataString(SearchText)}";

                if (FromDate.HasValue)
                    query += $"&from={FromDate.Value:yyyy-MM-dd}";

                if (ToDate.HasValue)
                    query += $"&to={ToDate.Value:yyyy-MM-dd}";

                // ApiClient tự gắn Bearer token từ TokenStore
                var response = await _apiClient.GetAsync<OrderResponse>(query);

                Orders.Clear();

                if (response?.Items == null)
                    return;

                // Fallback filter client-side (giữ giống logic cũ của bạn)
                var items = response.Items.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    items = items.Where(o =>
                        o.OrderCode != null &&
                        o.OrderCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                if (FromDate.HasValue)
                    items = items.Where(o => o.OrderDate.Date >= FromDate.Value.Date);

                if (ToDate.HasValue)
                    items = items.Where(o => o.OrderDate.Date <= ToDate.Value.Date);

                foreach (var order in items)
                    Orders.Add(order);
            }
            catch (Exception ex)
            {
                // Nếu token thiếu/expired => backend thường trả 401 => ApiClient throw HttpRequestException
                ErrorMessage = ex.Message;
                Debug.WriteLine($"Error loading orders: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
