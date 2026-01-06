using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.ViewModels.Base;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.ViewModels.Dashboard
{
    public sealed class DashboardViewModel : ViewModelBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("vi-VN");

        public DashboardViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

            FromDate = new DateTimeOffset(new DateTime(2020, 1, 1));
            ToDate = new DateTimeOffset(DateTime.Today);
            SelectedGroupBy = 3; // ComboBox index: 0=Day, 1=Week, 2=Month, 3=Year

            // Initialize empty chart
            InitializeCharts();
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

        // ===== Change Indicators =====
        private string _revenueChangePercent = "0%";
        public string RevenueChangePercent
        {
            get => _revenueChangePercent;
            set => SetProperty(ref _revenueChangePercent, value);
        }

        private SolidColorBrush _revenueChangeColor = new(Colors.Green);
        public SolidColorBrush RevenueChangeColor
        {
            get => _revenueChangeColor;
            set => SetProperty(ref _revenueChangeColor, value);
        }

        private string _profitChangePercent = "0%";
        public string ProfitChangePercent
        {
            get => _profitChangePercent;
            set => SetProperty(ref _profitChangePercent, value);
        }

        private SolidColorBrush _profitChangeColor = new(Colors.Green);
        public SolidColorBrush ProfitChangeColor
        {
            get => _profitChangeColor;
            set => SetProperty(ref _profitChangeColor, value);
        }

        private string _totalProductsSold = "0";
        public string TotalProductsSold
        {
            get => _totalProductsSold;
            set => SetProperty(ref _totalProductsSold, value);
        }

        private string _averageOrderValue = "0 ₫";
        public string AverageOrderValue
        {
            get => _averageOrderValue;
            set => SetProperty(ref _averageOrderValue, value);
        }

        private int _lowStockCount;
        public int LowStockCount
        {
            get => _lowStockCount;
            set => SetProperty(ref _lowStockCount, value);
        }

        // ===== Collections (using DTOs directly) =====
        public ObservableCollection<RevenueChartDto> RevenueChartData { get; } = new();
        public ObservableCollection<TopProductDto> TopProducts { get; } = new();
        public ObservableCollection<ProductLowStockDto> LowStockProducts { get; } = new();

        // ===== LiveCharts Properties =====
        private ISeries[] _revenueProfitSeries = [];
        public ISeries[] RevenueProfitSeries
        {
            get => _revenueProfitSeries;
            set => SetProperty(ref _revenueProfitSeries, value);
        }

        private IEnumerable<ICartesianAxis> _xAxes = [];
        public IEnumerable<ICartesianAxis> XAxes
        {
            get => _xAxes;
            set => SetProperty(ref _xAxes, value);
        }

        private IEnumerable<ICartesianAxis> _yAxes = [];
        public IEnumerable<ICartesianAxis> YAxes
        {
            get => _yAxes;
            set => SetProperty(ref _yAxes, value);
        }

        private void InitializeCharts()
        {
            RevenueProfitSeries =
            [
                new ColumnSeries<double>
                {
                    Name = "Revenue",
                    Values = Array.Empty<double>(),
                    Fill = new SolidColorPaint(SKColor.Parse("#5D5FEF")),
                    MaxBarWidth = 20,
                    Rx = 4,
                    Ry = 4
                },
                new ColumnSeries<double>
                {
                    Name = "Profit",
                    Values = Array.Empty<double>(),
                    Fill = new SolidColorPaint(SKColor.Parse("#22C55E")),
                    MaxBarWidth = 20,
                    Rx = 4,
                    Ry = 4
                }
            ];

            XAxes = new List<ICartesianAxis>
            {
                new Axis
                {
                    Labels = Array.Empty<string>(),
                    LabelsRotation = 0,
                    TextSize = 12,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#64748B"))
                }
            };

            YAxes = new List<ICartesianAxis>
            {
                new Axis
                {
                    Labeler = value => FormatCurrency(value),
                    TextSize = 12,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#64748B"))
                }
            };
        }

        private static string FormatCurrency(double value)
        {
            if (value >= 1_000_000_000)
                return $"{value / 1_000_000_000:N1}B";
            if (value >= 1_000_000)
                return $"{value / 1_000_000:N1}M";
            if (value >= 1_000)
                return $"{value / 1_000:N0}K";
            return $"{value:N0}";
        }

        // ===== Actions =====
        public async Task RefreshAsync(CancellationToken ct = default)
        {
            try
            {
                IsLoading = true;
                Error = null;

                var dto = await _dashboardService.GetDashboardStatsAsync(
                    FromDate.DateTime.Date,
                    ToDate.DateTime.Date,
                    (StatsGroupBy)SelectedGroupBy,
                    ct
                );

                // Format KPI values
                TotalRevenue = string.Format(_culture, "{0:N0} ₫", dto.TotalRevenue);
                TotalProfit = string.Format(_culture, "{0:N0} ₫", dto.TotalProfit);
                TotalOrders = dto.TotalOrders.ToString("N0", _culture);
                NewCustomers = dto.NewCustomersCount.ToString("N0", _culture);

                // Calculate total products sold and average order value
                int totalProductsSold = dto.RevenueChart?.Sum(c => c.TotalQuantity) ?? 0;
                TotalProductsSold = totalProductsSold.ToString("N0", _culture);

                decimal avgOrderValue = dto.TotalOrders > 0 ? dto.TotalRevenue / dto.TotalOrders : 0;
                AverageOrderValue = string.Format(_culture, "{0:N0} ₫", avgOrderValue);

                // Calculate change percentages from chart data
                CalculateChangePercents(dto.RevenueChart);

                // Revenue Chart Data
                RevenueChartData.Clear();
                if (dto.RevenueChart != null)
                {
                    foreach (var p in dto.RevenueChart)
                        RevenueChartData.Add(p);
                }

                // Update LiveCharts
                UpdateCharts(dto);

                // Top Products with rank and formatted values
                TopProducts.Clear();
                int rank = 1;
                if (dto.TopSellingProducts != null)
                {
                    foreach (var p in dto.TopSellingProducts)
                    {
                        TopProducts.Add(p);
                        rank++;
                    }
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

        private void CalculateChangePercents(List<RevenueChartDto>? chartData)
        {
            decimal revenueChange = 0;
            decimal profitChange = 0;

            if (chartData != null && chartData.Count >= 2)
            {
                var first = chartData.First();
                var last = chartData.Last();

                if (first.Revenue > 0)
                    revenueChange = (last.Revenue - first.Revenue) / first.Revenue * 100;
                if (first.Profit > 0)
                    profitChange = (last.Profit - first.Profit) / first.Profit * 100;
            }

            // Update Revenue change
            RevenueChangePercent = revenueChange >= 0 ? $"+{revenueChange:F1}%" : $"{revenueChange:F1}%";
            RevenueChangeColor = new SolidColorBrush(revenueChange >= 0 ? Colors.Green : Colors.Red);

            // Update Profit change
            ProfitChangePercent = profitChange >= 0 ? $"+{profitChange:F1}%" : $"{profitChange:F1}%";
            ProfitChangeColor = new SolidColorBrush(profitChange >= 0 ? Colors.Green : Colors.Red);
        }

        private void UpdateCharts(DashboardStatsDto dto)
        {
            if (dto.RevenueChart == null || dto.RevenueChart.Count == 0)
                return;

            // Update Revenue/Profit Chart
            var revenueValues = dto.RevenueChart.Select(x => (double)x.Revenue).ToArray();
            var profitValues = dto.RevenueChart.Select(x => (double)x.Profit).ToArray();
            var labels = dto.RevenueChart.Select(x => x.DateLabel).ToArray();

            RevenueProfitSeries =
            [
                new ColumnSeries<double>
                {
                    Name = "Revenue",
                    Values = revenueValues,
                    Fill = new SolidColorPaint(SKColor.Parse("#5D5FEF")),
                    MaxBarWidth = 20,
                    Rx = 4,
                    Ry = 4
                },
                new ColumnSeries<double>
                {
                    Name = "Profit",
                    Values = profitValues,
                    Fill = new SolidColorPaint(SKColor.Parse("#22C55E")),
                    MaxBarWidth = 20,
                    Rx = 4,
                    Ry = 4
                }
            ];

            XAxes = new List<ICartesianAxis>
            {
                new Axis
                {
                    Labels = labels,
                    LabelsRotation = labels.Length > 10 ? 45 : 0,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#64748B"))
                }
            };
        }

        public async Task ExportExcelAsync()
        {
            try
            {
                IsLoading = true;
                Error = null;

                var picker = new Windows.Storage.Pickers.FileSavePicker();
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                picker.FileTypeChoices.Add("Excel File", new System.Collections.Generic.List<string> { ".xlsx" });
                picker.SuggestedFileName = $"Dashboard_Report_{FromDate.DateTime:yyyyMMdd}_{ToDate.DateTime:yyyyMMdd}";

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    var reportService = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<IReportService>(App.Services);
                    
                    var bytes = await reportService.ExportToExcelAsync(
                        FromDate.DateTime.Date,
                        ToDate.DateTime.Date,
                        (StatsGroupBy)SelectedGroupBy
                    );

                    await Windows.Storage.FileIO.WriteBytesAsync(file, bytes);
                }
            }
            catch (Exception ex)
            {
                Error = $"Export failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}