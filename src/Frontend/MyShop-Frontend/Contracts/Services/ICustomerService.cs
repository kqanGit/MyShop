using MyShop_Frontend.Contracts.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// GET /api/customers
        /// </summary>
        Task<PagedResult<CustomerDto>> GetCustomersAsync(int pageIndex = 1, int pageSize = 10, CancellationToken ct = default);

        /// <summary>
        /// GET /api/customers/search
        /// </summary>
        Task<PagedResult<CustomerDto>> SearchCustomersAsync(int pageIndex = 1, int pageSize = 10, string? phone = null, string? name = null, int? tierId = null, CancellationToken ct = default);

        /// <summary>
        /// GET /api/customers/{id}
        /// </summary>
        Task<CustomerDetailDto> GetCustomerDetailAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// POST /api/customers
        /// </summary>
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto request, CancellationToken ct = default);

        /// <summary>
        /// PUT /api/customers/{id}
        /// </summary>
        Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerDto request, CancellationToken ct = default);

        /// <summary>
        /// DELETE /api/customers/{id}
        /// </summary>
        Task<bool> DeleteCustomerAsync(int id, CancellationToken ct = default);
    }
}