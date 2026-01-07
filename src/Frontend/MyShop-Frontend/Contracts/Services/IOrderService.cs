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

        /// <summary>
        /// PUT /api/Orders/{id}/status
        /// </summary>
        Task<bool> UpdateStatusAsync(int orderId, int status, CancellationToken ct = default);

        /// <summary>
        /// PUT /api/Orders/{id}/pay
        /// </summary>
        Task<bool> PayOrderAsync(int orderId, CancellationToken ct = default);

        /// <summary>
        /// PUT /api/Orders/{id}/cancel
        /// </summary>
        Task<bool> CancelOrderAsync(int orderId, CancellationToken ct = default);
    }
}
