using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Dtos.Stats
{
    public sealed class DashboardResponseDto
    {
        [JsonPropertyName("totalRevenue")]
        public decimal TotalRevenue { get; set; }

        [JsonPropertyName("totalProfit")]
        public decimal TotalProfit { get; set; }

        [JsonPropertyName("totalOrders")]
        public int TotalOrders { get; set; }

        [JsonPropertyName("newCustomersCount")]
        public int NewCustomersCount { get; set; }

        [JsonPropertyName("totalProducts")]
        public int TotalProducts { get; set; }

        [JsonPropertyName("revenueChart")]
        public List<RevenueChartPointDto> RevenueChart { get; set; } = [];

        [JsonPropertyName("topSellingProducts")]
        public List<TopSellingProductDto> TopSellingProducts { get; set; } = [];

        [JsonPropertyName("lowStockProducts")]
        public List<LowStockProductDto> LowStockProducts { get; set; } = [];
    }

    public sealed class RevenueChartPointDto
    {
        [JsonPropertyName("dateLabel")]
        public string DateLabel { get; set; } = "";

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; set; }

        [JsonPropertyName("profit")]
        public decimal Profit { get; set; }

        [JsonPropertyName("totalQuantity")]
        public int TotalQuantity { get; set; }
    }

    public sealed class TopSellingProductDto
    {
        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = "";

        [JsonPropertyName("quantitySold")]
        public int QuantitySold { get; set; }

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; set; }

        // Display properties (computed in ViewModel)
        public int Rank { get; set; }
        public string QuantitySoldText => $"{QuantitySold} sold";
        public string RevenueText { get; set; } = "";
    }

    public sealed class LowStockProductDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = "";

        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        // Display property (computed in ViewModel)
        public string StockText => $"Stock: {Stock}";
    }
}
