using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// GET /api/Category
        /// </summary>
        Task<List<Category>> GetCategoriesAsync(CancellationToken ct = default);

        /// <summary>
        /// GET /api/Category/{id}
        /// </summary>
        Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default);

        /// <summary>
        /// POST /api/Category
        /// </summary>
        Task<Category> CreateCategoryAsync(CreateCategoryDto request, CancellationToken ct = default);
    }
}
