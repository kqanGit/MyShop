using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByFilterAsync(string keyword, int? categoryId, decimal? minPrice, decimal? maxPrice);
        Task<bool> CheckStockAsync(int productId, int quantityRequested);
    }
}
