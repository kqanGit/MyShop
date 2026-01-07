using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public class ReportService : IReportService
    {
        private readonly IApiClient _api;

        public ReportService(IApiClient api)
        {
            _api = api;
        }

        public async Task<DashboardStatsDto> GetReportStatsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            StatsGroupBy groupBy = StatsGroupBy.Day,
            CancellationToken ct = default)
        {
            var queryParams = new List<string>
            {
                $"groupBy={(int)groupBy}"
            };

            if (fromDate.HasValue)
                queryParams.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");

            if (toDate.HasValue)
                queryParams.Add($"toDate={toDate.Value:yyyy-MM-dd}");

            var query = string.Join("&", queryParams);
            return await _api.GetAsync<DashboardStatsDto>($"api/Stats/dashboard?{query}", ct);
        }

        public async Task<byte[]> ExportToExcelAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            StatsGroupBy groupBy = StatsGroupBy.Day,
            CancellationToken ct = default)
        {
            var queryParams = new List<string>
            {
                $"groupBy={(int)groupBy}"
            };

            if (fromDate.HasValue)
                queryParams.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");

            if (toDate.HasValue)
                queryParams.Add($"toDate={toDate.Value:yyyy-MM-dd}");

            var query = string.Join("&", queryParams);
            return await _api.GetBytesAsync($"api/Stats/export-excel?{query}", ct);
        }
    }
}
