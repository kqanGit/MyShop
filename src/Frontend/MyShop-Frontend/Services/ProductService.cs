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

        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken ct = default)
        {
            // Backend trả về danh sách products từ API
            return await _api.GetAsync<List<Product>>("api/products", ct);
        }

        public async Task<Product?> GetProductByIdAsync(int productId, CancellationToken ct = default)
        {
            return await _api.GetAsync<Product>($"api/products/{productId}", ct);
        }
    }
}