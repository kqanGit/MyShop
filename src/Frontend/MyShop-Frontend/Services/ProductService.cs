using MyShop_Frontend.Contracts.Dtos;
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

        public async Task<PagedResult<Product>> GetProductsAsync(
            string? keyword = null,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sort = null,
            int pageIndex = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(keyword))
                queryParams.Add($"keyword={keyword}");

            if (categoryId.HasValue)
                queryParams.Add($"categoryId={categoryId}");

            if (minPrice.HasValue)
                queryParams.Add($"minPrice={minPrice}");

            if (maxPrice.HasValue)
                queryParams.Add($"maxPrice={maxPrice}");

            if (!string.IsNullOrWhiteSpace(sort))
                queryParams.Add($"sort={sort}");

            queryParams.Add($"pageIndex={pageIndex}");
            queryParams.Add($"pageSize={pageSize}");

            var query = string.Join("&", queryParams);
            return await _api.GetAsync<PagedResult<Product>>($"api/Product?{query}", ct);
        }

        public async Task<Product> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            return await _api.GetAsync<Product>($"api/Product/{id}", ct);
        }

        public async Task<Product> AddProductAsync(Product product, CancellationToken ct = default)
        {
            return await _api.PostAsync<Product>("api/Product", product, ct);
        }

        public async Task<Product> UpdateProductAsync(Product product, CancellationToken ct = default)
        {
            return await _api.PutAsync<Product>($"api/Product/{product.ProductId}", product, ct);
        }

        public async Task<bool> DeleteProductAsync(int id, CancellationToken ct = default)
        {
            return await _api.DeleteAsync($"api/Product/{id}", ct);
        }
    }
}