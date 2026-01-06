using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly IApiClient _api;

        public CategoryService(IApiClient api)
        {
            _api = api;
        }

        public Task<List<Category>> GetCategoriesAsync(CancellationToken ct = default)
            => _api.GetAsync<List<Category>>("api/Category", ct);

        public Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default)
            => _api.GetAsync<Category>($"api/Category/{id}", ct);

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto request, CancellationToken ct = default)
        {
            return await _api.PostAsync<Category>("api/Category", request, ct);
        }
    }
}
