using MyShop_Frontend.Contracts.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IReportService
    {
        /// <summary>
        /// GET /api/Stats/dashboard (same as Dashboard)
        /// </summary>
        Task<DashboardStatsDto> GetReportStatsAsync(
            DateTime? fromDate = null, 
            DateTime? toDate = null, 
            StatsGroupBy groupBy = StatsGroupBy.Day, 
            CancellationToken ct = default);

        /// <summary>
        /// GET /api/Stats/export-excel
        /// </summary>
        Task<byte[]> ExportToExcelAsync(
            DateTime? fromDate = null, 
            DateTime? toDate = null, 
            StatsGroupBy groupBy = StatsGroupBy.Day, 
            CancellationToken ct = default);
    }
}
