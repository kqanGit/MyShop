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
        private readonly IOrderService _orderService;

        private bool _isBusy;
        private string _searchText = string.Empty;
        private DateTimeOffset? _fromDate;
        private DateTimeOffset? _toDate;
        private string? _errorMessage;

        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();

        public RelayCommand ClearFilterCommand { get; }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (!SetProperty(ref _pageIndex, value)) return;
                _ = LoadOrdersAsync();
            }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            private set
            {
                if (!SetProperty(ref _pageSize, value)) return;
                PageIndex = 1;
                _ = LoadOrdersAsync();
            }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get => _totalRecords;
            set => SetProperty(ref _totalRecords, value);
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }

        public string PagingStatus => $"Page {PageIndex}/{Math.Max(TotalPages, 1)} - {Orders.Count} of {TotalRecords}";

        public RelayCommand NextPageCommand { get; }
        public RelayCommand PrevPageCommand { get; }

        public OrderViewModel()
        {
            _orderService = App.Services.GetRequiredService<IOrderService>();

            var settings = App.Services.GetRequiredService<IUserSettingsStore>();
            PageSize = settings.GetProductsPageSize(defaultValue: 10);

            _fromDate = new DateTimeOffset(new DateTime(2025, 1, 1));
            _toDate = DateTimeOffset.Now;

            ClearFilterCommand = new RelayCommand(_ => ClearFilters());
            NextPageCommand = new RelayCommand(async _ => { PageIndex++; await LoadOrdersAsync(); }, _ => PageIndex < TotalPages);
            PrevPageCommand = new RelayCommand(async _ => { PageIndex--; await LoadOrdersAsync(); }, _ => PageIndex > 1);

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
            SearchText = string.Empty;
            FromDate = new DateTimeOffset(new DateTime(2025, 1, 1));
            ToDate = DateTimeOffset.Now;
            PageIndex = 1;
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
                // Server side filter by date (orders endpoint does not accept search in backend currently)
                var response = await _orderService.GetOrdersAsync(
                    fromDate: FromDate?.DateTime,
                    toDate: ToDate?.DateTime,
                    pageIndex: PageIndex,
                    pageSize: PageSize);

                Orders.Clear();

                if (response?.Items == null)
                    return;

                TotalRecords = response.TotalRecords;
                TotalPages = response.TotalPages;
                OnPropertyChanged(nameof(PagingStatus));

                (PrevPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();

                var items = response.Items.AsEnumerable();

                // Keep local SearchText filtering for now (API list doesn't include search)
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    items = items.Where(o => o.OrderCode != null && o.OrderCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                foreach (var order in items)
                {
                    // Normalize status names for UI (English only)
                    order.StatusName = order.StatusName switch
                    {
                        "Completed" => "Paid",
                        "Canceled" => "Canceled",
                        "Delivering" => "Paid", // backend legacy
                        "New" => "New",
                        _ => order.StatusName
                    };

                    Orders.Add(order);
                }

                OnPropertyChanged(nameof(PagingStatus));
            }
            catch (Exception ex)
            {
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
