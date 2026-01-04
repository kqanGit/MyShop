using MyShop_Frontend.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IOrderService
    {
        Task<OrderResponse> GetOrdersAsync(DateTime? fromDate, DateTime? toDate, int pageIndex = 1, int pageSize = 10, CancellationToken ct = default);
        Task<OrderDetailDto> GetOrderByIdAsync(int id, CancellationToken ct = default);

        Task<OrderResultDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default);
        Task DeleteOrderAsync(int id, CancellationToken ct = default);
    }
}
