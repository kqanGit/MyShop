using System;

namespace MyShop_Frontend.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } // Helper for UI
        public string Unit { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; } // Path or URL to image
        public bool IsRemoved { get; set; }
        public System.Collections.Generic.List<ProductImage> Images { get; set; } = new();
    }

    public class ProductImage
    {
        public int ImageIndex { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }
}
