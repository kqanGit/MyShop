using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Collections.Generic;
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

        public async Task<OrderResultDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default)
        {
            return await _api.PostAsync<OrderResultDto>("api/Orders", request, ct);
        }

        public async Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(GetOrdersRequest request, CancellationToken ct = default)
        {
            var queryParams = new List<string>
            {
                $"pageIndex={request.PageIndex}",
                $"pageSize={request.PageSize}"
            };

            if (request.FromDate.HasValue)
                queryParams.Add($"fromDate={request.FromDate.Value:yyyy-MM-dd}");

            if (request.ToDate.HasValue)
                queryParams.Add($"toDate={request.ToDate.Value:yyyy-MM-dd}");

            var query = string.Join("&", queryParams);
            return await _api.GetAsync<PagedResult<OrderSummaryDto>>($"api/Orders?{query}", ct);
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int orderId, CancellationToken ct = default)
        {
            return await _api.GetAsync<OrderResponseDto>($"api/Orders/{orderId}", ct);
        }

        public async Task<bool> DeleteOrderAsync(int orderId, CancellationToken ct = default)
        {
            return await _api.DeleteAsync($"api/Orders/{orderId}", ct);
        }
    }
}
