using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IApiClient _api;

        public ProductService(IApiClient api)
        {
            _api = api;
        }

        public async Task<PagedResult<Product>> GetProductsAsync(int pageIndex = 1, int pageSize = 5, CancellationToken ct = default)
        {
            // Backend returns PagedResult<Product>
            // Append Query String
            var url = $"api/Product?pageIndex={pageIndex}&pageSize={pageSize}";

            System.Diagnostics.Debug.WriteLine($"Calling API: {url}");
            return await _api.GetAsync<PagedResult<Product>>(url, ct);
        }

        public async Task<Product?> GetProductByIdAsync(int productId, CancellationToken ct = default)
        {
            return await _api.GetAsync<Product>($"api/Product/{productId}", ct);
        }

        public async Task<Product> AddProductAsync(Product product, CancellationToken ct = default)
        {
            return await _api.PostAsync<Product>("api/Product", product, ct);
        }

        public async Task<Product> UpdateProductAsync(Product product, CancellationToken ct = default)
        {
            return await _api.PutAsync<Product>($"api/Product/{product.ProductId}", product, ct);
        }

        public async Task DeleteProductAsync(int productId, CancellationToken ct = default)
        {
            await _api.DeleteAsync($"api/Product/{productId}", ct);
        }
    }
}