using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> SearchCustomersAsync(string? phone, string? name, CancellationToken ct = default);
        Task<Customer?> GetCustomerDetailAsync(int customerId, CancellationToken ct = default);
    }
}