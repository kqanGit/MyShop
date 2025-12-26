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

        [JsonPropertyName("revenueChart")]
        public List<RevenueChartPointDto> RevenueChart { get; set; } = [];

        [JsonPropertyName("topSellingProducts")]
        public List<TopSellingProductDto> TopSellingProducts { get; set; } = [];
    }

    public sealed class RevenueChartPointDto
    {
        [JsonPropertyName("dateLabel")]
        public string DateLabel { get; set; } = "";

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; set; }

        [JsonPropertyName("profit")]
        public decimal Profit { get; set; }
    }

    public sealed class TopSellingProductDto
    {
        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = "";

        [JsonPropertyName("quantitySold")]
        public int QuantitySold { get; set; }

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; set; }
    }
}
