using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync(CancellationToken ct = default);
        Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default);
    }
}
