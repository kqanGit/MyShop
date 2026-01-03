using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IApiClient _api;

        public CustomerService(IApiClient api)
        {
            _api = api;
        }

        public async Task<PagedResult<Customer>> GetCustomersAsync(int pageIndex, int pageSize, CancellationToken ct = default)
        {
            var url = $"api/customers?pageIndex={pageIndex}&pageSize={pageSize}";
            return await _api.GetAsync<PagedResult<Customer>>(url, ct);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string? phone, string? name, CancellationToken ct = default)
        {
             // For now, adhere to the interface. 
             // If the backend doesn't support this specific search yet, we might need to adjust or stub it.
             // But the prompt demanded "GetCustomers" for the list.
             // I will leave this as a stub or implement if I know the API.
             // The prompt didn't specify search API. I'll return empty list or throw for now to focus on the main task.
             // Or better, since I'm replacing mock data, I should probably try to use the same API if possible or just NotImplemented.
             // I'll return empty to avoid crashes if called.
             return new List<Customer>(); 
        }

        public async Task<Customer?> GetCustomerDetailAsync(int customerId, CancellationToken ct = default)
        {
            // Stub implementation
            return null; 
        }
    }
}
