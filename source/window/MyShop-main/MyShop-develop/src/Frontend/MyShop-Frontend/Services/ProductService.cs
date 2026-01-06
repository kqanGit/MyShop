using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace MyShop_Frontend.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IApiClient _api;

        public ProductService(IApiClient api)
        {
            _api = api;
        }

        public async Task<PagedResult<Product>> GetProductsAsync(
            string? keyword,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string? sort,
            int pageIndex = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            var query = $"api/Product?pageIndex={pageIndex}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(keyword))
                query += $"&keyword={Uri.EscapeDataString(keyword)}";

            if (categoryId.HasValue)
                query += $"&categoryId={categoryId.Value}";

            if (minPrice.HasValue)
                query += $"&minPrice={minPrice.Value}";

            if (maxPrice.HasValue)
                query += $"&maxPrice={maxPrice.Value}";

            if (!string.IsNullOrWhiteSpace(sort))
                query += $"&sort={Uri.EscapeDataString(sort)}";

            return await _api.GetAsync<PagedResult<Product>>(query, ct);
        }

        public async Task<PagedResult<Product>> GetProductsAsync(int pageIndex = 1, int pageSize = 5, CancellationToken ct = default)
        {
            var url = $"api/Product?pageIndex={pageIndex}&pageSize={pageSize}";
            return await _api.GetAsync<PagedResult<Product>>(url, ct);
        }

        public async Task<List<Product>> GetAllProductsAsync(CancellationToken ct = default)
        {
            // Based on provided API list: GET /api/Product/ returns all
            return await _api.GetAsync<List<Product>>("api/Product", ct);
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