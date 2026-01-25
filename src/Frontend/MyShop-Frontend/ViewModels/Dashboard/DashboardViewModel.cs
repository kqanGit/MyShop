using MyShop_Frontend.Contracts.Dtos.Stats;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.ViewModels.Dashboard
{
    public sealed class DashboardViewModel : ViewModelBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

            FromDate = new DateTimeOffset(new DateTime(2020, 1, 1));
            ToDate = new DateTimeOffset(DateTime.Today);
            SelectedGroupBy = 4; // 1=ngày,2=tuần,3=tháng,4=năm
        }

        // ===== Helpers =====
        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName ?? string.Empty);
            return true;
        }

        // ===== Filter properties =====
        private DateTimeOffset _fromDate;
        public DateTimeOffset FromDate
        {
            get => _fromDate;
            set => SetProperty(ref _fromDate, value);
        }

        private DateTimeOffset _toDate;
        public DateTimeOffset ToDate
        {
            get => _toDate;
            set => SetProperty(ref _toDate, value);
        }

        private int _selectedGroupBy;
        public int SelectedGroupBy
        {
            get => _selectedGroupBy;
            set => SetProperty(ref _selectedGroupBy, value);
        }

        // ===== UI states =====
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string? _error;
        public string? Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        // ===== KPI Card Properties (formatted strings for display) =====
        private string _totalRevenue = "0 ₫";
        public string TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        private string _totalProfit = "0 ₫";
        public string TotalProfit
        {
            get => _totalProfit;
            set => SetProperty(ref _totalProfit, value);
        }

        private string _totalOrders = "0";
        public string TotalOrders
        {
            get => _totalOrders;
            set => SetProperty(ref _totalOrders, value);
        }

        private string _newCustomers = "0";
        public string NewCustomers
        {
            get => _newCustomers;
            set => SetProperty(ref _newCustomers, value);
        }

        private int _lowStockCount;
        public int LowStockCount
        {
            get => _lowStockCount;
            set => SetProperty(ref _lowStockCount, value);
        }

        // ===== Collections (using DTOs directly) =====
        public ObservableCollection<RevenueChartPointDto> RevenueChart { get; } = new();
        public ObservableCollection<TopSellingProductDto> TopProducts { get; } = new();
        public ObservableCollection<LowStockProductDto> LowStockProducts { get; } = new();

        // ===== Actions =====
        public async Task RefreshAsync(CancellationToken ct = default)
        {
            try
            {
                IsLoading = true;
                Error = null;

                var dto = await _dashboardService.GetDashboardAsync(
                    FromDate.DateTime.Date,
                    ToDate.DateTime.Date,
                    SelectedGroupBy,
                    ct
                );

                var culture = CultureInfo.GetCultureInfo("vi-VN");

                // Format KPI values
                TotalRevenue = string.Format(culture, "{0:N0} ₫", dto.TotalRevenue);
                TotalProfit = string.Format(culture, "{0:N0} ₫", dto.TotalProfit);
                TotalOrders = dto.TotalOrders.ToString("N0", culture);
                NewCustomers = dto.NewCustomersCount.ToString("N0", culture);

                // Revenue Chart
                RevenueChart.Clear();
                foreach (var p in dto.RevenueChart)
                    RevenueChart.Add(p);

                // Top Products with rank and formatted values
                TopProducts.Clear();
                int rank = 1;
                foreach (var p in dto.TopSellingProducts)
                {
                    p.Rank = rank++;
                    p.RevenueText = string.Format(culture, "{0:N0}đ", p.Revenue);
                    TopProducts.Add(p);
                }

                // Low Stock Products
                LowStockProducts.Clear();
                if (dto.LowStockProducts != null)
                {
                    foreach (var p in dto.LowStockProducts)
                        LowStockProducts.Add(p);
                    LowStockCount = dto.LowStockProducts.Count;
                }
                else
                {
                    LowStockCount = 0;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
