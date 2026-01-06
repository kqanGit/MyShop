using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IApiClient _api;

        public OrderService(IApiClient api)
        {
            _api = api;
        }

        public Task<OrderResultDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default)
            => _api.PostAsync<OrderResultDto>("api/Orders", request, ct);

        public Task<OrderResponse> GetOrdersAsync(DateTime? fromDate, DateTime? toDate, int pageIndex = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var url = $"api/Orders?pageIndex={pageIndex}&pageSize={pageSize}";

            if (fromDate.HasValue)
                url += $"&from_date={fromDate.Value:yyyy-MM-dd}";

            if (toDate.HasValue)
                url += $"&to_date={toDate.Value:yyyy-MM-dd}";

            // user_id is derived from token on backend in most flows; do not pass here.

            return _api.GetAsync<OrderResponse>(url, ct);
        }

        public Task<OrderDetailDto> GetOrderByIdAsync(int id, CancellationToken ct = default)
            => _api.GetAsync<OrderDetailDto>($"api/Orders/{id}", ct);

        public async Task DeleteOrderAsync(int id, CancellationToken ct = default)
        {
            await _api.DeleteAsync($"api/Orders/{id}", ct);
        }
    }
}
