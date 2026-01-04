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
                new Product { ProductId = 1, ProductName = "Snack Oishi Tôm Cay", CategoryId = 1, CategoryName = "Food & Drink", Unit = "Gói", Cost = 4000, Price = 6000, Stock = 100, Image = "", IsRemoved = false },
                new Product { ProductId = 6, ProductName = "Bia Heineken Silver (Lon)", CategoryId = 1, CategoryName = "Food & Drink", Unit = "Lon", Cost = 17500, Price = 21000, Stock = 240, Image = "", IsRemoved = false },
                new Product { ProductId = 11, ProductName = "Nước Rửa Chén Sunlight Chanh 750g", CategoryId = 2, CategoryName = "Personal Care", Unit = "Chai", Cost = 22000, Price = 28000, Stock = 45, Image = "", IsRemoved = false },
                new Product { ProductId = 20, ProductName = "Giấy Vệ Sinh E'mos Classic (10 cuộn)", CategoryId = 3, CategoryName = "Household", Unit = "Lốc", Cost = 60000, Price = 75000, Stock = 30, Image = "", IsRemoved = false },
            };
        }
    }
}
