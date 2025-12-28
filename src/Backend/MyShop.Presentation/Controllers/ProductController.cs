using Microsoft.AspNetCore.Mvc;
using MyShop.Application.DTOs;
using MyShop.Application.Services;
using Microsoft.AspNetCore.Authorization;


namespace MyShop.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });
            return Ok(product);
        }

        /*
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategory(categoryId);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string? keyword,
            [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var products = await _productService.SearchProducts(keyword, categoryId, minPrice, maxPrice);
            return Ok(products);
        }
        */
    }
}
