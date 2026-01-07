using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Contracts.Dtos
{
    // ===== Stats/Dashboard DTOs =====
    
    public class DashboardStatsDto
    {
        [JsonPropertyName("totalOrders")]
        public int TotalOrders { get; set; }

        [JsonPropertyName("totalRevenue")]
        public decimal TotalRevenue { get; set; }

        [JsonPropertyName("totalProfit")]
        public decimal TotalProfit { get; set; }

        [JsonPropertyName("newCustomersCount")]
        public int NewCustomersCount { get; set; }

        [JsonPropertyName("totalProducts")]
        public int TotalProducts { get; set; }

        [JsonPropertyName("lowStockProducts")]
        public List<ProductLowStockDto> LowStockProducts { get; set; } = new();

        [JsonPropertyName("revenueChart")]
        public List<RevenueChartDto> RevenueChart { get; set; } = new();

        [JsonPropertyName("topSellingProducts")]
        public List<TopProductDto> TopSellingProducts { get; set; } = new();
    }

    public class ProductLowStockDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("stock")]
        public int Stock { get; set; }
    }

    public class RevenueChartDto
    {
        [JsonPropertyName("dateLabel")]
        public string DateLabel { get; set; } = string.Empty;

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; set; }

        [JsonPropertyName("profit")]
        public decimal Profit { get; set; }

        [JsonPropertyName("totalQuantity")]
        public int TotalQuantity { get; set; }
    }

    public class TopProductDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("quantitySold")]
        public int QuantitySold { get; set; }

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }
    }

    public enum StatsGroupBy
    {
        Day = 1,
        Week = 2,
        Month = 3,
        Year = 4
    }
}
