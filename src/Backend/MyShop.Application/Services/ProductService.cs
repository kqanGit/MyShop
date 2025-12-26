using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(int productId);
    }
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return products
                .Where(p => p.IsRemoved == null || !p.IsRemoved[0])
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.CategoryName,
                    Unit = p.Unit,
                    Cost = p.Cost,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Image,
                    IsRemoved = p.IsRemoved != null && p.IsRemoved[0]
                })
                .ToList();
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
                
            if (product == null) throw new KeyNotFoundException("Product not found");
            
            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName,
                Unit = product.Unit,
                Cost = product.Cost,
                Price = product.Price,
                Stock = product.Stock,
                Image = product.Image,
                IsRemoved = product.IsRemoved != null && product.IsRemoved[0]
            };
        }
    }
}
