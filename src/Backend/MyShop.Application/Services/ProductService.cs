using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Application.DTOs.Common;
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
        Task<PagedResult<ProductDto>> GetProductsPaged(int pageIndex, int pageSize, int? categoryId);
        Task<ProductDto> GetProductById(int productId);
        Task<ProductDto> CreateProduct(CreateProductDto productDto);
        Task<ProductDto> UpdateProduct(int id, CreateProductDto productDto);
        Task<bool> DeleteProduct(int id);
    }
    
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductDto>> GetProductsPaged(int pageIndex, int pageSize, int? categoryId)
        {
            var totalRecords = await _context.Products
                .Where(p => p.IsRemoved == null || p.IsRemoved == false)
                .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
                .CountAsync();

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsRemoved == null || p.IsRemoved == false)
                .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
                .OrderBy(p => p.ProductId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null,
                    Unit = p.Unit,
                    Cost = p.Cost,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Image,
                    IsRemoved = p.IsRemoved == true
                })
                .ToListAsync();

            return new PagedResult<ProductDto>(products, totalRecords, pageIndex, pageSize);
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
                IsRemoved = product.IsRemoved == true
            };
        }

        public async Task<ProductDto> CreateProduct(CreateProductDto productDto)
        {
            var maxId = await _context.Products.AnyAsync()
                ? await _context.Products.MaxAsync(p => p.ProductId)
                : 0;

            var product = new Domain.Entities.Product
            {
                ProductId = maxId + 1,
                ProductName = productDto.ProductName,
                CategoryId = productDto.CategoryId,
                Unit = productDto.Unit,
                Cost = productDto.Cost,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Image = productDto.Image,
                IsRemoved = false
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Unit = product.Unit,
                Cost = product.Cost,
                Price = product.Price,
                Stock = product.Stock,
                Image = product.Image,
                IsRemoved = false
            };
        }
        public async Task<ProductDto> UpdateProduct(int id, CreateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            product.ProductName = productDto.ProductName;
            product.CategoryId = productDto.CategoryId;
            product.Unit = productDto.Unit;
            product.Cost = productDto.Cost;
            product.Price = productDto.Price;
            product.Stock = productDto.Stock;
            product.Image = productDto.Image;
            await _context.SaveChangesAsync();
            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Unit = product.Unit,
                Cost = product.Cost,
                Price = product.Price,
                Stock = product.Stock,
                Image = product.Image,
                IsRemoved = false
            };
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            product.IsRemoved = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
