using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using MyShop_Frontend.Contracts.Dtos.Reports;
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
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MyShop_Frontend.ViewModels.Reports
{
    public sealed class ReportViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;
        private readonly CultureInfo _culture = CultureInfo.GetCultureInfo("vi-VN");

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;

            FromDate = new DateTimeOffset(DateTime.Today.AddMonths(-12));
            ToDate = new DateTimeOffset(DateTime.Today);

            SelectedGroupBy = 2;

            InitializeCharts();
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName ?? string.Empty);
            return true;
        }

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

        private string _uniqueProductsCount = "0";
        public string UniqueProductsCount
        {
            get => _uniqueProductsCount;
            set => SetProperty(ref _uniqueProductsCount, value);
        }

        private string _revenueChangePercent = "+0%";
        public string RevenueChangePercent
        {
            get => _revenueChangePercent;
            set => SetProperty(ref _revenueChangePercent, value);
        }

        private string _revenueChangeIcon = "\uE70E";
        public string RevenueChangeIcon
        {
            get => _revenueChangeIcon;
            set => SetProperty(ref _revenueChangeIcon, value);
        }

        private SolidColorBrush _revenueChangeColor = new(Colors.Green);
        public SolidColorBrush RevenueChangeColor
        {
            get => _revenueChangeColor;
            set => SetProperty(ref _revenueChangeColor, value);
        }

        private string _profitChangePercent = "+0%";
        public string ProfitChangePercent
        {
            get => _profitChangePercent;
            set => SetProperty(ref _profitChangePercent, value);
        }

        private string _profitChangeIcon = "\uE70E";
        public string ProfitChangeIcon
        {
            get => _profitChangeIcon;
            set => SetProperty(ref _profitChangeIcon, value);
        }

        private SolidColorBrush _profitChangeColor = new(Colors.Green);
        public SolidColorBrush ProfitChangeColor
        {
            get => _profitChangeColor;
            set => SetProperty(ref _profitChangeColor, value);
        }

        public ObservableCollection<TopProductItem> TopProducts { get; } = new();
        public ObservableCollection<PeriodComparisonItem> PeriodComparisons { get; } = new();

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

        private IEnumerable<ISeries> _categorySeries = [];
        public IEnumerable<ISeries> CategorySeries
        {
            get => _categorySeries;
            set => SetProperty(ref _categorySeries, value);
        }

        private void InitializeCharts()
        {
            RevenueProfitSeries =
            [
                new LineSeries<double>
                {
                    Name = "Revenue",
                    Values = Array.Empty<double>(),
                    Stroke = new SolidColorPaint(SKColor.Parse("#5D5FEF")) { StrokeThickness = 3 },
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#5D5FEF")) { StrokeThickness = 3 },
                    GeometrySize = 8,
                    Fill = null
                },
                new LineSeries<double>
                {
                    Name = "Profit",
                    Values = Array.Empty<double>(),
                    Stroke = new SolidColorPaint(SKColor.Parse("#22C55E")) { StrokeThickness = 3 },
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#22C55E")) { StrokeThickness = 3 },
                    GeometrySize = 8,
                    Fill = null
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

            CategorySeries = Array.Empty<ISeries>();
        }

        private static string FormatCurrency(double value)
        {
            if (value >= 1_000_000_000)
            {
                return $"{value / 1_000_000_000:N1}B";
            }

            if (value >= 1_000_000)
            {
                return $"{value / 1_000_000:N1}M";
            }

            if (value >= 1_000)
            {
                return $"{value / 1_000:N0}K";
            }

            return $"{value:N0}";
        }

        public async Task LoadReportAsync(CancellationToken ct = default)
        {
            try
            {
                IsLoading = true;
                Error = null;

                var report = await _reportService.GetReportAsync(
                    FromDate.DateTime.Date,
                    ToDate.DateTime.Date,
                    SelectedGroupBy + 1,
                    ct
                );

                TotalRevenue = string.Format(_culture, "{0:N0} ₫", report.TotalRevenue);
                TotalProfit = string.Format(_culture, "{0:N0} ₫", report.TotalProfit);
                TotalOrders = report.TotalOrders.ToString("N0", _culture);
                TotalProductsSold = report.TotalProductsSold.ToString("N0", _culture);
                AverageOrderValue = string.Format(_culture, "{0:N0} ₫", report.AverageOrderValue);
                UniqueProductsCount = report.UniqueProductsCount.ToString();

                UpdateChangeIndicators(report.RevenueChangePercent, report.ProfitChangePercent);

                UpdateRevenueChart(report);
                UpdateCategoryChart(report);

                TopProducts.Clear();
                var rank = 1;

                var maxRevenue = report.TopProducts.FirstOrDefault()?.Revenue ?? 1;
                foreach (var p in report.TopProducts)
                {
                    TopProducts.Add(new TopProductItem
                    {
                        Rank = rank++,
                        ProductName = p.ProductName,
                        QuantitySoldText = $"{p.QuantitySold} sold",
                        RevenueText = string.Format(_culture, "{0:N0}đ", p.Revenue),
                        PercentOfTop = maxRevenue > 0 ? (double)(p.Revenue / maxRevenue * 100) : 0
                    });
                }

                PeriodComparisons.Clear();
                foreach (var period in report.PeriodComparisons)
                {
                    var growth = period.GrowthPercent;
                    PeriodComparisons.Add(new PeriodComparisonItem
                    {
                        PeriodLabel = period.PeriodLabel,
                        RevenueText = string.Format(_culture, "{0:N0}đ", period.Revenue),
                        ProfitText = string.Format(_culture, "{0:N0}đ", period.Profit),
                        OrderCount = period.OrderCount.ToString(),
                        GrowthText = growth >= 0 ? $"+{growth:F1}%" : $"{growth:F1}%",
                        GrowthColor = new SolidColorBrush(growth >= 0 ? Colors.Green : Colors.Red)
                    });
                }
            }
            catch (OperationCanceledException)
            {
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

        private void UpdateChangeIndicators(decimal revenueChange, decimal profitChange)
        {
            RevenueChangePercent = revenueChange >= 0 ? $"+{revenueChange:F1}%" : $"{revenueChange:F1}%";
            RevenueChangeIcon = revenueChange >= 0 ? "\uE70E" : "\uE70D";
            RevenueChangeColor = new SolidColorBrush(revenueChange >= 0 ? Colors.Green : Colors.Red);

            ProfitChangePercent = profitChange >= 0 ? $"+{profitChange:F1}%" : $"{profitChange:F1}%";
            ProfitChangeIcon = profitChange >= 0 ? "\uE70E" : "\uE70D";
            ProfitChangeColor = new SolidColorBrush(profitChange >= 0 ? Colors.Green : Colors.Red);
        }

        private void UpdateRevenueChart(ReportResponseDto report)
        {
            var revenueValues = report.ChartData.Select(x => (double)x.Revenue).ToArray();
            var profitValues = report.ChartData.Select(x => (double)x.Profit).ToArray();
            var labels = report.ChartData.Select(x => x.DateLabel).ToArray();

            RevenueProfitSeries =
            [
                new LineSeries<double>
                {
                    Name = "Revenue",
                    Values = revenueValues,
                    Stroke = new SolidColorPaint(SKColor.Parse("#5D5FEF")) { StrokeThickness = 3 },
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#5D5FEF")) { StrokeThickness = 3 },
                    GeometrySize = 8,
                    Fill = new SolidColorPaint(SKColor.Parse("#5D5FEF").WithAlpha(30))
                },
                new LineSeries<double>
                {
                    Name = "Profit",
                    Values = profitValues,
                    Stroke = new SolidColorPaint(SKColor.Parse("#22C55E")) { StrokeThickness = 3 },
                    GeometryStroke = new SolidColorPaint(SKColor.Parse("#22C55E")) { StrokeThickness = 3 },
                    GeometrySize = 8,
                    Fill = new SolidColorPaint(SKColor.Parse("#22C55E").WithAlpha(30))
                }
            ];

            XAxes = new List<ICartesianAxis>
            {
                new Axis
                {
                    Labels = labels,
                    LabelsRotation = labels.Length > 8 ? 45 : 0,
                    TextSize = 11,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#64748B"))
                }
            };
        }

        private void UpdateCategoryChart(ReportResponseDto report)
        {
            var colors = new[]
            {
                SKColor.Parse("#5D5FEF"),
                SKColor.Parse("#22C55E"),
                SKColor.Parse("#F59E0B"),
                SKColor.Parse("#EC4899"),
                SKColor.Parse("#8B5CF6"),
                SKColor.Parse("#06B6D4"),
                SKColor.Parse("#EF4444"),
                SKColor.Parse("#84CC16")
            };

            var series = new List<ISeries>();
            var colorIndex = 0;

            foreach (var product in report.TopProducts)
            {
                series.Add(new PieSeries<double>
                {
                    Name = product.ProductName,
                    Values = new[] { (double)product.Revenue },
                    Fill = new SolidColorPaint(colors[colorIndex % colors.Length]),
                    Pushout = colorIndex == 0 ? 5 : 0
                });

                colorIndex++;
            }

            CategorySeries = series;
        }

        public async Task ExportReportAsync()
        {
            try
            {
                IsLoading = true;
                Error = null;

                var picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeChoices.Add("Excel File", new List<string> { ".xlsx" });
                picker.SuggestedFileName = $"Report_{FromDate.DateTime:yyyyMMdd}_{ToDate.DateTime:yyyyMMdd}";

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    var bytes = await _reportService.ExportExcelAsync(
                        FromDate.DateTime.Date,
                        ToDate.DateTime.Date,
                        SelectedGroupBy + 1
                    );

                    await FileIO.WriteBytesAsync(file, bytes);
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

    public class TopProductItem
    {
        public int Rank { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string QuantitySoldText { get; set; } = string.Empty;
        public string RevenueText { get; set; } = string.Empty;
        public double PercentOfTop { get; set; }
    }

    public class PeriodComparisonItem
    {
        public string PeriodLabel { get; set; } = string.Empty;
        public string RevenueText { get; set; } = string.Empty;
        public string ProfitText { get; set; } = string.Empty;
        public string OrderCount { get; set; } = string.Empty;
        public string GrowthText { get; set; } = string.Empty;
        public SolidColorBrush GrowthColor { get; set; } = new(Colors.Green);
    }
}