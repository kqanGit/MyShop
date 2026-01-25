using MyShop_Frontend.Contracts.Dtos.Stats;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public sealed class DashboardService : IDashboardService
    {
        private readonly IApiClient _api;

        public DashboardService(IApiClient api) => _api = api;

        public Task<DashboardResponseDto> GetDashboardAsync(DateTime fromDate, DateTime toDate, int groupBy, CancellationToken ct = default)
        {
            // groupBy: 1=ngày, 2=tuần, 3=tháng, 4=năm
            var from = fromDate.ToString("yyyy-MM-dd");
            var to = toDate.ToString("yyyy-MM-dd");

            var url = $"api/Stats/dashboard?fromDate={from}&toDate={to}&groupBy={groupBy}";
            return _api.GetAsync<DashboardResponseDto>(url, ct);
        }
    }
}
