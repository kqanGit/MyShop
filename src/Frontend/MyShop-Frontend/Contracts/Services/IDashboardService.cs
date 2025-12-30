using MyShop_Frontend.Contracts.Dtos.Stats;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetDashboardAsync(DateTime fromDate, DateTime toDate, int groupBy, CancellationToken ct = default);

        Task<byte[]> ExportExcelAsync(DateTime fromDate, DateTime toDate, int groupBy, CancellationToken ct = default);
    }
}
