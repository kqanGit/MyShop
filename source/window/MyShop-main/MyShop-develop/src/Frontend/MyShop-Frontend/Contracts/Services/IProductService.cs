using MyShop_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> GetProductsAsync(int pageIndex = 1, int pageSize = 5, CancellationToken ct = default);
        Task<Product?> GetProductByIdAsync(int productId, CancellationToken ct = default);
        Task<Product> AddProductAsync(Product product, CancellationToken ct = default);
        Task<Product> UpdateProductAsync(Product product, CancellationToken ct = default);
        Task DeleteProductAsync(int productId, CancellationToken ct = default);

        Task<PagedResult<Product>> GetProductsAsync(
            string? keyword,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string? sort,
            int pageIndex = 1,
            int pageSize = 10,
            CancellationToken ct = default);

        Task<List<Product>> GetAllProductsAsync(CancellationToken ct = default);
    }
}
