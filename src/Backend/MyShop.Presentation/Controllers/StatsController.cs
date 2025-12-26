using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Enums;
using MyShop.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace MyShop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("dashboard")]
        [Authorize] 
        public async Task<IActionResult> GetDashboardStats(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] StatsGroupBy groupBy = StatsGroupBy.Day) 
        {
            try
            {

                var start = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var end = toDate ?? DateTime.Now;

            
                end = end.Date.AddDays(1).AddTicks(-1);

                var result = await _statsService.GetDashboardStatsAsync(start, end, groupBy);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("export-excel")] 
        [Authorize] 
        public async Task<IActionResult> ExportStatsToExcel(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] StatsGroupBy groupBy = StatsGroupBy.Day)
        {
            try
            {
               
                var start = fromDate ?? DateTime.Now.AddDays(-30);
                var end = toDate ?? DateTime.Now;

                
                end = end.Date.AddDays(1).AddTicks(-1);

                var stats = await _statsService.GetDashboardStatsAsync(start, end, groupBy);

            
                using (var workbook = new XLWorkbook())
                {
                  
                    var wsSummary = workbook.Worksheets.Add("Summary");

               
                    wsSummary.Cell(1, 1).Value = "BUSINESS PERFORMANCE REPORT";
                    wsSummary.Cell(1, 1).Style.Font.Bold = true;
                    wsSummary.Cell(1, 1).Style.Font.FontSize = 16;
                    wsSummary.Cell(1, 1).Style.Font.FontColor = XLColor.Blue;

                    wsSummary.Cell(2, 1).Value = $"Period: {start:dd/MM/yyyy} - {end:dd/MM/yyyy}";
                    wsSummary.Cell(3, 1).Value = $"Statistics Type: {groupBy}";

                
                    wsSummary.Cell(5, 1).Value = "Metric";
                    wsSummary.Cell(5, 2).Value = "Value";
                    wsSummary.Range("A5:B5").Style.Font.Bold = true;
                    wsSummary.Range("A5:B5").Style.Fill.BackgroundColor = XLColor.LightGray;

                    wsSummary.Cell(6, 1).Value = "Total Revenue";
                    wsSummary.Cell(6, 2).Value = stats.TotalRevenue;
                    wsSummary.Cell(6, 2).Style.NumberFormat.Format = "#,##0"; 

                    wsSummary.Cell(7, 1).Value = "Total Profit";
                    wsSummary.Cell(7, 2).Value = stats.TotalProfit;
                    wsSummary.Cell(7, 2).Style.NumberFormat.Format = "#,##0";

                    wsSummary.Cell(8, 1).Value = "Total Orders";
                    wsSummary.Cell(8, 2).Value = stats.TotalOrders;

                    wsSummary.Columns().AdjustToContents(); 

                 
                    var wsChart = workbook.Worksheets.Add("Chart Data");

                
                    wsChart.Cell(1, 1).Value = "Date";
                    wsChart.Cell(1, 2).Value = "Revenue (VND)";
                    wsChart.Cell(1, 3).Value = "Profit (VND)";
                    wsChart.Cell(1, 4).Value = "Quantity Sold (Items)"; 
                    wsChart.Range("A1:D1").Style.Font.Bold = true;
                    wsChart.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.Yellow;

              
                    int row = 2;
                    foreach (var item in stats.RevenueChart)
                    {
                        wsChart.Cell(row, 1).Value = item.DateLabel; 

                        wsChart.Cell(row, 2).Value = item.Revenue;
                        wsChart.Cell(row, 2).Style.NumberFormat.Format = "#,##0";

                        wsChart.Cell(row, 3).Value = item.Profit;
                        wsChart.Cell(row, 3).Style.NumberFormat.Format = "#,##0";

                        wsChart.Cell(row, 4).Value = item.TotalQuantity;

                        row++;
                    }
                    wsChart.Columns().AdjustToContents();

                   
                    var wsTop = workbook.Worksheets.Add("Top Products");

                    wsTop.Cell(1, 1).Value = "No.";
                    wsTop.Cell(1, 2).Value = "Product Name";
                    wsTop.Cell(1, 3).Value = "Quantity Sold";
                    wsTop.Cell(1, 4).Value = "Revenue Contribution";
                    wsTop.Range("A1:D1").Style.Font.Bold = true;
                    wsTop.Range("A1:D1").Style.Fill.BackgroundColor = XLColor.LightGreen;

                    row = 2;
                    int stt = 1;
                    foreach (var item in stats.TopSellingProducts)
                    {
                        wsTop.Cell(row, 1).Value = stt++;
                        wsTop.Cell(row, 2).Value = item.ProductName;
                        wsTop.Cell(row, 3).Value = item.QuantitySold;

                        wsTop.Cell(row, 4).Value = item.Revenue;
                        wsTop.Cell(row, 4).Style.NumberFormat.Format = "#,##0";

                        row++;
                    }
                    wsTop.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        string fileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                      
                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}