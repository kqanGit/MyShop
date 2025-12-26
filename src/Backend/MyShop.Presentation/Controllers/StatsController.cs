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
    }
}