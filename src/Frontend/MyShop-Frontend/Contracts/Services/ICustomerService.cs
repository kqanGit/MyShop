using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<Customer>> GetCustomersAsync(int pageIndex, int pageSize, string? phone = null, string? name = null, CancellationToken ct = default);
        Task<Customer> AddCustomerAsync(Customer customer, CancellationToken ct = default);
        Task<Customer> UpdateCustomerAsync(Customer customer, CancellationToken ct = default);
        Task<Customer?> GetCustomerDetailAsync(int customerId, CancellationToken ct = default);
    }
}