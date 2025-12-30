using MyShop_Frontend.Contracts.Dtos.Stats;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public sealed class DashboardService : IDashboardService
    {
        private readonly IApiClient _api;

        public DashboardService(IApiClient api) => _api = api;

        public Task<DashboardResponseDto> GetDashboardAsync(DateTime fromDate, DateTime toDate, int groupByIndex, CancellationToken ct = default)
        {
            // groupByIndex from ComboBox: 0=Day, 1=Week, 2=Month, 3=Year
            // API expects: 1=Day, 2=Week, 3=Month, 4=Year
            var groupBy = groupByIndex + 1;

            var from = fromDate.ToString("yyyy-MM-dd");
            var to = toDate.ToString("yyyy-MM-dd");

            var url = $"api/Stats/dashboard?fromDate={from}&toDate={to}&groupBy={groupBy}";
            return _api.GetAsync<DashboardResponseDto>(url, ct);
        }

        public Task<byte[]> ExportExcelAsync(DateTime fromDate, DateTime toDate, int groupByIndex, CancellationToken ct = default)
        {
            // groupByIndex from ComboBox: 0=Day, 1=Week, 2=Month, 3=Year
            // API expects: 1=Day, 2=Week, 3=Month, 4=Year
            var groupBy = groupByIndex + 1;

            var from = fromDate.ToString("yyyy-MM-dd");
            var to = toDate.ToString("yyyy-MM-dd");

            var url = $"api/Stats/export-excel?fromDate={from}&toDate={to}&groupBy={groupBy}";
            return _api.GetBytesAsync(url, ct);
        }
    }
}
