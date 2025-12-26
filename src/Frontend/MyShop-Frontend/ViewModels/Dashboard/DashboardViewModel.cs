using MyShop_Frontend.Contracts.Dtos.Stats;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        // ===== Summary texts =====
        private string _totalRevenueText = "0";
        public string TotalRevenueText
        {
            get => _totalRevenueText;
            set => SetProperty(ref _totalRevenueText, value);
        }

        private string _totalProfitText = "0";
        public string TotalProfitText
        {
            get => _totalProfitText;
            set => SetProperty(ref _totalProfitText, value);
        }

        private string _totalOrdersText = "0";
        public string TotalOrdersText
        {
            get => _totalOrdersText;
            set => SetProperty(ref _totalOrdersText, value);
        }

        // ===== Collections =====
        public ObservableCollection<RevenueChartPointDto> RevenueChart { get; } = new();
        public ObservableCollection<TopSellingProductDto> TopSellingProducts { get; } = new();

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

                TotalRevenueText = string.Format(culture, "{0:N0} ₫", dto.TotalRevenue);
                TotalProfitText = string.Format(culture, "{0:N0} ₫", dto.TotalProfit);
                TotalOrdersText = dto.TotalOrders.ToString(culture);

                RevenueChart.Clear();
                foreach (var p in dto.RevenueChart)
                    RevenueChart.Add(p);

                TopSellingProducts.Clear();
                foreach (var p in dto.TopSellingProducts)
                    TopSellingProducts.Add(p);
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
