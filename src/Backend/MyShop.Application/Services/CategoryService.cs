using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;

namespace MyShop.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int categoryId);
        Task<CategoryDto> CreateCategory(CreateCategoryRequest request);
    }
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                })
                .ToListAsync();
        }

        public async Task<CategoryDto> GetCategoryById(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            if (category == null) throw new KeyNotFoundException("Category not found");
            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
            };
        }

        public async Task<CategoryDto> CreateCategory(CreateCategoryRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                throw new ArgumentException("Category name is required", nameof(request.CategoryName));

            var normalizedName = request.CategoryName.Trim();

            var exists = await _context.Categories.AnyAsync(c => c.CategoryName == normalizedName);
            if (exists)
                throw new InvalidOperationException("Category already exists");

            var entity = new Category
            {
                CategoryName = normalizedName,
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
            };
        }
    }
}