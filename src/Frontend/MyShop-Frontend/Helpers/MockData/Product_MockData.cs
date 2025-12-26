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
                    ProductId = "PRD-025",
                    ProductName = "Sữa tươi tiệt trùng Vinamilk 100%",
                    CategoryId = 1,
                    CategoryName = "Sữa & Trứng",
                    Price = 32.5m,
                    Stock = 200,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-026",
                    ProductName = "Sữa đậu nành Fami",
                    CategoryId = 1,
                    CategoryName = "Sữa & Trứng",
                    Price = 28m,
                    Stock = 150,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-027",
                    ProductName = "Trứng gà ta (10 quả)",
                    CategoryId = 1,
                    CategoryName = "Sữa & Trứng",
                    Price = 45m,
                    Stock = 120,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-028",
                    ProductName = "Rau muống sạch",
                    CategoryId = 2,
                    CategoryName = "Rau củ",
                    Price = 12m,
                    Stock = 300,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-029",
                    ProductName = "Cải thìa Đà Lạt",
                    CategoryId = 2,
                    CategoryName = "Rau củ",
                    Price = 18.5m,
                    Stock = 220,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-030",
                    ProductName = "Cà chua bi",
                    CategoryId = 2,
                    CategoryName = "Rau củ",
                    Price = 25m,
                    Stock = 180,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-031",
                    ProductName = "Khoai tây Đà Lạt",
                    CategoryId = 2,
                    CategoryName = "Rau củ",
                    Price = 22m,
                    Stock = 160,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-032",
                    ProductName = "Thịt heo ba rọi",
                    CategoryId = 3,
                    CategoryName = "Thịt & Cá",
                    Price = 95m,
                    Stock = 90,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-033",
                    ProductName = "Thịt bò Úc nhập khẩu",
                    CategoryId = 3,
                    CategoryName = "Thịt & Cá",
                    Price = 98m,
                    Stock = 70,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-034",
                    ProductName = "Ức gà phi lê",
                    CategoryId = 3,
                    CategoryName = "Thịt & Cá",
                    Price = 65m,
                    Stock = 110,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-035",
                    ProductName = "Cá hồi phi lê Na Uy",
                    CategoryId = 3,
                    CategoryName = "Thịt & Cá",
                    Price = 99.9m,
                    Stock = 60,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-036",
                    ProductName = "Cá basa tươi",
                    CategoryId = 3,
                    CategoryName = "Thịt & Cá",
                    Price = 48m,
                    Stock = 130,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-037",
                    ProductName = "Gạo ST25 thượng hạng",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 85m,
                    Stock = 100,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-038",
                    ProductName = "Gạo thơm Jasmine",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 72m,
                    Stock = 140,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-039",
                    ProductName = "Nước mắm Phú Quốc 40 độ đạm",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 68m,
                    Stock = 170,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-040",
                    ProductName = "Dầu ăn Simply",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 54m,
                    Stock = 190,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-041",
                    ProductName = "Đường tinh luyện Biên Hòa",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 32m,
                    Stock = 210,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-042",
                    ProductName = "Muối i-ốt",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 9m,
                    Stock = 400,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-043",
                    ProductName = "Mì gói Hảo Hảo tôm chua cay",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 4.5m,
                    Stock = 500,
                    Image = "",
                    IsRemoved = false
                },
                new Product
                {
                    ProductId = "PRD-044",
                    ProductName = "Nước tương Nam Dương",
                    CategoryId = 4,
                    CategoryName = "Đồ khô & Gia vị",
                    Price = 18m,
                    Stock = 260,
                    Image = "",
                    IsRemoved = false
                }


            };
        }
    }
}
