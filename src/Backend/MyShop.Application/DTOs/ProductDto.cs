using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Unit { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public bool IsRemoved { get; set; }
    }

    public class CreateProductDto
    {
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? Unit { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
    }

}
