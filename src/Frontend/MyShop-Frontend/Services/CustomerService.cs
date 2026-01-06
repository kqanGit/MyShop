using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
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

        public async Task<PagedResult<CustomerDto>> GetCustomersAsync(int pageIndex = 1, int pageSize = 10, CancellationToken ct = default)
        {
            return await _api.GetAsync<PagedResult<CustomerDto>>($"api/customers?pageIndex={pageIndex}&pageSize={pageSize}", ct);
        }

        public async Task<PagedResult<CustomerDto>> SearchCustomersAsync(
            int pageIndex = 1, 
            int pageSize = 10, 
            string? phone = null, 
            string? name = null, 
            int? tierId = null, 
            CancellationToken ct = default)
        {
            var queryParams = new List<string>
            {
                $"pageIndex={pageIndex}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(phone))
                queryParams.Add($"phone={phone}");

            if (!string.IsNullOrWhiteSpace(name))
                queryParams.Add($"name={name}");

            if (tierId.HasValue)
                queryParams.Add($"tierId={tierId}");

            var query = string.Join("&", queryParams);
            return await _api.GetAsync<PagedResult<CustomerDto>>($"api/customers/search?{query}", ct);
        }

        public async Task<CustomerDetailDto> GetCustomerDetailAsync(int id, CancellationToken ct = default)
        {
            return await _api.GetAsync<CustomerDetailDto>($"api/customers/{id}", ct);
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto request, CancellationToken ct = default)
        {
            return await _api.PostAsync<CustomerDto>("api/customers", request, ct);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerDto request, CancellationToken ct = default)
        {
            return await _api.PutAsync<CustomerDto>($"api/customers/{id}", request, ct);
        }

        public async Task<bool> DeleteCustomerAsync(int id, CancellationToken ct = default)
        {
            return await _api.DeleteAsync($"api/customers/{id}", ct);
        }
    }
}
