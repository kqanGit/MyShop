using System.Collections.Generic;

namespace MyShop.Application.DTOs.Stats
{
    public class DashboardStatsDto
    {
     
        public decimal TotalRevenue { get; set; } 
        public decimal TotalProfit { get; set; }  
        public int TotalOrders { get; set; }      

     
        public List<RevenueChartDto> RevenueChart { get; set; } = new List<RevenueChartDto>();

      
        public List<TopProductDto> TopSellingProducts { get; set; } = new List<TopProductDto>();

        public int NewCustomersCount { get; set; } 
        public int TotalProducts { get; set; }    
        public List<ProductLowStockDto> LowStockProducts { get; set; }
    }

    public class RevenueChartDto
    {
        public string DateLabel { get; set; }
        public decimal Revenue { get; set; }  
        public decimal Profit { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public string? Image { get; set; }
    }

    public class ProductLowStockDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; } 
    }
}