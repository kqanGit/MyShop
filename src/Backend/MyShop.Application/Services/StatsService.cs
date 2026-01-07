using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs.Stats;
using MyShop.Application.Enums;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.Services
{
    public interface IStatsService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync(DateTime fromDate, DateTime toDate, StatsGroupBy groupBy);
    }

    public class StatsService : IStatsService
    {
        private readonly AppDbContext _context;

        public StatsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync(DateTime fromDate, DateTime toDate, StatsGroupBy groupBy)
        {
       
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product) 
                .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
                .ToListAsync();

            var result = new DashboardStatsDto();

            result.TotalOrders = orders.Count;
            result.TotalRevenue = orders.Sum(o => o.FinalPrice);
            result.TotalProfit = orders.SelectMany(o => o.OrderDetails)
                .Sum(od => od.TotalLine - (od.CurrentCost * od.Quantity));
            result.NewCustomersCount = await _context.Customers.CountAsync(c => c.CreateDate >= fromDate && c.CreateDate <= toDate);

            // With boolean IsRemoved, filter directly server-side
            result.TotalProducts = await _context.Products
                .CountAsync(p => p.IsRemoved == false || p.IsRemoved == null);

            result.LowStockProducts = await _context.Products
                .Where(p => (p.IsRemoved == false || p.IsRemoved == null) && p.Stock < 5)
                .OrderBy(p => p.Stock)
                .Take(5)
                .Select(p => new ProductLowStockDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Stock = p.Stock
                })
                .ToListAsync();
            IEnumerable<RevenueChartDto> chartData = null;

            switch (groupBy)
            {
                case StatsGroupBy.Day: 
                    chartData = orders
                        .GroupBy(o => o.OrderDate.Date)
                        .Select(g => new RevenueChartDto
                        {
                            DateLabel = g.Key.ToString("dd/MM/yyyy"),
                            Revenue = g.Sum(o => o.FinalPrice),
                            Profit = g.SelectMany(o => o.OrderDetails).Sum(od => od.TotalLine - (od.CurrentCost * od.Quantity)),
                            TotalQuantity = g.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity)
                        })
                        .OrderBy(x => DateTime.ParseExact(x.DateLabel, "dd/MM/yyyy", null));
                    break;

                case StatsGroupBy.Week: 
                    chartData = orders
                        .GroupBy(o => {
                            var cal = CultureInfo.CurrentCulture.Calendar;
                            int weekNum = cal.GetWeekOfYear(o.OrderDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            return new { Year = o.OrderDate.Year, Week = weekNum };
                        })
                        .Select(g => new RevenueChartDto
                        {
                            DateLabel = $"W{g.Key.Week:00}/{g.Key.Year}",
                            Revenue = g.Sum(o => o.FinalPrice),
                            Profit = g.SelectMany(o => o.OrderDetails).Sum(od => od.TotalLine - (od.CurrentCost * od.Quantity)),
                            TotalQuantity = g.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity)
                        })
                        .OrderBy(x => x.DateLabel);
                    break;

                case StatsGroupBy.Month: 
                    chartData = orders
                        .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                        .Select(g => new RevenueChartDto
                        {
                            DateLabel = $"{g.Key.Month:00}/{g.Key.Year}",
                            Revenue = g.Sum(o => o.FinalPrice),
                            Profit = g.SelectMany(o => o.OrderDetails).Sum(od => od.TotalLine - (od.CurrentCost * od.Quantity)),
                            TotalQuantity = g.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity)
                        })
                      
                        .OrderBy(x => x.DateLabel.Substring(3))
                        .ThenBy(x => x.DateLabel.Substring(0, 2));
                    break;

                case StatsGroupBy.Year: 
                    chartData = orders
                        .GroupBy(o => o.OrderDate.Year)
                        .Select(g => new RevenueChartDto
                        {
                            DateLabel = g.Key.ToString(),
                            Revenue = g.Sum(o => o.FinalPrice),
                            Profit = g.SelectMany(o => o.OrderDetails).Sum(od => od.TotalLine - (od.CurrentCost * od.Quantity)),
                            TotalQuantity = g.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity)
                        })
                        .OrderBy(x => x.DateLabel);
                    break;
            }

            result.RevenueChart = chartData?.ToList() ?? new List<RevenueChartDto>();

          
            result.TopSellingProducts = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(od => new { od.ProductId, od.Product.ProductName, od.Product.Image })
                .Select(g => new TopProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName ?? "Unknown",
                    QuantitySold = g.Sum(od => od.Quantity),
                    Revenue = g.Sum(od => od.TotalLine),
                    Image = g.Key.Image
                })
                .OrderByDescending(x => x.QuantitySold) 
                .Take(5) 
                .ToList();

            return result;
        }
    }
}