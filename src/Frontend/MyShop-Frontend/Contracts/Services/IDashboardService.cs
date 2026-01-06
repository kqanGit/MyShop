using MyShop_Frontend.Contracts.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IDashboardService
    {
        /// <summary>
        /// GET /api/Stats/dashboard
        /// </summary>
        Task<DashboardStatsDto> GetDashboardStatsAsync(
            DateTime? fromDate = null, 
            DateTime? toDate = null, 
            StatsGroupBy groupBy = StatsGroupBy.Day, 
            CancellationToken ct = default);
    }
}