using MyShop_Frontend.Models;
using System.Collections.Generic;

namespace MyShop_Frontend.Helpers.MockData
{
    public static class Product_MockData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId = "PRD-024",
                    ProductName = "2% Reduced Fat Milk",
                    CategoryId = 1,
                    CategoryName = "Dairy & Eggs",
                    Price = 4.79m,
                    Stock = 180,
                    Image = "", // Image empty as requested
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-037",
                    ProductName = "Almond Milk",
                    CategoryId = 1,
                    CategoryName = "Dairy & Eggs",
                    Price = 3.99m,
                    Stock = 130,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-071",
                    ProductName = "Apple Juice",
                    CategoryId = 2,
                    CategoryName = "Beverages",
                    Price = 3.99m,
                    Stock = 165,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-066",
                    ProductName = "Apple Pie",
                    CategoryId = 3,
                    CategoryName = "Bakery",
                    Price = 8.99m,
                    Stock = 75,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-016",
                    ProductName = "Avocados",
                    CategoryId = 4,
                    CategoryName = "Fresh Produce",
                    Price = 1.99m,
                    Stock = 155,
                    Image = "",
                    IsRemoved = false
                },
                 new Product
                {
                    ProductId = "PRD-012",
                    ProductName = "Baby Spinach",
                    CategoryId = 4,
                    CategoryName = "Fresh Produce",
                    Price = 2.79m,
                    Stock = 140,
                    Image = "",
                    IsRemoved = false
                }
            };
        }
    }
}
