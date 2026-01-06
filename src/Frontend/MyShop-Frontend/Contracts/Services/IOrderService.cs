using MyShop_Frontend.Contracts.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// POST /api/Orders
        /// </summary>
        Task<OrderResultDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default);

        /// <summary>
        /// GET /api/Orders
        /// </summary>
        Task<PagedResult<OrderSummaryDto>> GetOrdersAsync(GetOrdersRequest request, CancellationToken ct = default);

        /// <summary>
        /// GET /api/Orders/{id}
        /// </summary>
        Task<OrderResponseDto> GetOrderByIdAsync(int orderId, CancellationToken ct = default);

        /// <summary>
        /// DELETE /api/Orders/{id} (if exists)
        /// </summary>
        Task<bool> DeleteOrderAsync(int orderId, CancellationToken ct = default);
    }
}
