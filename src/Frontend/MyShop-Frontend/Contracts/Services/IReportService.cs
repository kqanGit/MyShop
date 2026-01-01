using MyShop_Frontend.Contracts.Dtos.Reports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IReportService
    {
        Task<ReportResponseDto> GetReportAsync(
            DateTime fromDate,
            DateTime toDate,
            int groupBy,
            CancellationToken ct = default);

        Task<byte[]> ExportExcelAsync(
            DateTime fromDate,
            DateTime toDate,
            int groupBy,
            CancellationToken ct = default);
    }
}