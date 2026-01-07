using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IProductService
    {
        /// <summary>
        /// GET /api/Product
        /// </summary>
        Task<Dtos.PagedResult<Product>> GetProductsAsync(
            string? keyword = null,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sort = null,
            int pageIndex = 1,
            int pageSize = 10,
            CancellationToken ct = default);

        /// <summary>
        /// GET /api/Product/{id}
        /// </summary>
        Task<Product> GetProductByIdAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// POST /api/Product
        /// </summary>
        Task<Product> AddProductAsync(Product product, CancellationToken ct = default);

        /// <summary>
        /// PUT /api/Product/{id}
        /// </summary>
        Task<Product> UpdateProductAsync(Product product, CancellationToken ct = default);

        /// <summary>
        /// DELETE /api/Product/{id}
        /// </summary>
        Task<bool> DeleteProductAsync(int id, CancellationToken ct = default);
    }
}
