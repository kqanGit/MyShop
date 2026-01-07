using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
 
        public async Task<IEnumerable<Product>> GetProductsByFilterAsync(string keyword, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var query = Context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => EF.Functions.ILike(p.ProductName!, $"%{keyword}%"));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CheckStockAsync(int productId, int quantityRequested)
        {
            var product = await Context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == productId);
            return product != null && product.Stock >= quantityRequested;
        }
    }
}
