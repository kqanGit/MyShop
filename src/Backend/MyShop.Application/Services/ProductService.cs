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
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<PagedResult<ProductDto>> GetProductsPaged(int pageIndex, int pageSize);
        Task<ProductDto> GetProductById(int productId);
        Task<ProductDto> CreateProduct(CreateProductDto productDto);
        Task<ProductDto> UpdateProduct(int id, ProductDto productDto);
        Task<bool> DeleteProduct(int id);
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
                    CategoryName = p.Category != null ? p.Category.CategoryName : null,
                    Unit = p.Unit,
                    Cost = p.Cost,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Image,
                    IsRemoved = p.IsRemoved != null && p.IsRemoved[0]
                })
                .ToList();
        }

        public async Task<PagedResult<ProductDto>> GetProductsPaged(int pageIndex, int pageSize)
        {
            var totalRecords = await _context.Products
                .Where(p => p.IsRemoved == null || !p.IsRemoved[0])
                .CountAsync();

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsRemoved == null || !p.IsRemoved[0])
                .OrderBy(p => p.ProductId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productDtos = products
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
                    IsRemoved = p.IsRemoved != null && p.IsRemoved[0]
                })
                .ToList();

            return new PagedResult<ProductDto>(productDtos, totalRecords, pageIndex, pageSize);
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
                IsRemoved = new System.Collections.BitArray(new[] { false })
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
        public async Task<ProductDto> UpdateProduct(int id, ProductDto productDto)
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
            product.IsRemoved = new System.Collections.BitArray(new[] { productDto.IsRemoved });
            await _context.SaveChangesAsync();
            return productDto;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");
            product.IsRemoved = new System.Collections.BitArray(new[] { true });
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
