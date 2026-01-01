using System.Collections.Generic;

namespace MyShop_Frontend.Contracts.Dtos.Reports
{
    public sealed class ReportResponseDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProductsSold { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int UniqueProductsCount { get; set; }
        public decimal RevenueChangePercent { get; set; }
        public decimal ProfitChangePercent { get; set; }
        public List<ChartDataPoint> ChartData { get; set; } = new();
        public List<TopProductDto> TopProducts { get; set; } = new();
        public List<CategorySalesDto> CategorySales { get; set; } = new();
        public List<PeriodComparisonDto> PeriodComparisons { get; set; } = new();
    }

    public sealed class ChartDataPoint
    {
        public string DateLabel { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
    }

    public sealed class TopProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public sealed class CategorySalesDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public sealed class PeriodComparisonDto
    {
        public string PeriodLabel { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
        public int OrderCount { get; set; }
        public decimal GrowthPercent { get; set; }
    }
}
