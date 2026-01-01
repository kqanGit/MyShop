using MyShop_Frontend.Contracts.Dtos.Reports;
using MyShop_Frontend.Contracts.Dtos.Stats;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public class ReportService : IReportService
    {
        private readonly IApiClient _apiClient;

        public ReportService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ReportResponseDto> GetReportAsync(
            DateTime fromDate,
            DateTime toDate,
            int groupBy,
            CancellationToken ct = default)
        {
            ValidateDateRange(fromDate, toDate);

            var from = fromDate.ToString("yyyy-MM-dd");
            var to = toDate.ToString("yyyy-MM-dd");

            var url = $"api/Stats/dashboard?fromDate={Uri.EscapeDataString(from)}&toDate={Uri.EscapeDataString(to)}&groupBy={groupBy}";

            var dashboardDto = await _apiClient.GetAsync<DashboardResponseDto>(url, ct);

            return ConvertToReportResponse(dashboardDto);
        }

        public Task<byte[]> ExportExcelAsync(
            DateTime fromDate,
            DateTime toDate,
            int groupBy,
            CancellationToken ct = default)
        {
            ValidateDateRange(fromDate, toDate);

            var from = fromDate.ToString("yyyy-MM-dd");
            var to = toDate.ToString("yyyy-MM-dd");
            var url = $"api/Stats/export-excel?fromDate={Uri.EscapeDataString(from)}&toDate={Uri.EscapeDataString(to)}&groupBy={groupBy}";

            return _apiClient.GetBytesAsync(url, ct);
        }

        private static void ValidateDateRange(DateTime fromDate, DateTime toDate)
        {
            if (fromDate.Date > toDate.Date)
                throw new ArgumentException("`fromDate` must be less than or equal to `toDate`.");
        }

        private static ReportResponseDto ConvertToReportResponse(DashboardResponseDto dto)
        {
            dto ??= new DashboardResponseDto();

            var chartData = dto.RevenueChart?.Select(c => new ChartDataPoint
            {
                DateLabel = c.DateLabel,
                Revenue = c.Revenue,
                Profit = c.Profit
            }).ToList() ?? new List<ChartDataPoint>();

            var topProducts = dto.TopSellingProducts?.Select(p => new TopProductDto
            {
                ProductName = p.ProductName,
                QuantitySold = p.QuantitySold,
                Revenue = p.Revenue
            }).ToList() ?? new List<TopProductDto>();

            int totalProductsSold = dto.RevenueChart?.Sum(c => c.TotalQuantity) ?? 0;
            decimal avgOrderValue = dto.TotalOrders > 0 ? dto.TotalRevenue / dto.TotalOrders : 0;

            var periodComparisons = new List<PeriodComparisonDto>();
            decimal? previousRevenue = null;

            foreach (var point in chartData)
            {
                decimal growth = 0;
                if (previousRevenue.HasValue && previousRevenue.Value > 0)
                {
                    growth = (point.Revenue - previousRevenue.Value) / previousRevenue.Value * 100;
                }

                periodComparisons.Add(new PeriodComparisonDto
                {
                    PeriodLabel = point.DateLabel,
                    Revenue = point.Revenue,
                    Profit = point.Profit,
                    OrderCount = 0,
                    GrowthPercent = growth
                });

                previousRevenue = point.Revenue;
            }

            decimal revenueChangePercent = 0;
            decimal profitChangePercent = 0;

            if (chartData.Count >= 2)
            {
                var first = chartData.First();
                var last = chartData.Last();

                if (first.Revenue > 0)
                    revenueChangePercent = (last.Revenue - first.Revenue) / first.Revenue * 100;
                if (first.Profit > 0)
                    profitChangePercent = (last.Profit - first.Profit) / first.Profit * 100;
            }

            return new ReportResponseDto
            {
                TotalRevenue = dto.TotalRevenue,
                TotalProfit = dto.TotalProfit,
                TotalOrders = dto.TotalOrders,
                TotalProductsSold = totalProductsSold,
                AverageOrderValue = avgOrderValue,
                UniqueProductsCount = topProducts.Count,
                RevenueChangePercent = revenueChangePercent,
                ProfitChangePercent = profitChangePercent,
                ChartData = chartData,
                TopProducts = topProducts,
                CategorySales = new List<CategorySalesDto>(),
                PeriodComparisons = periodComparisons.TakeLast(6).ToList()
            };
        }
    }
}