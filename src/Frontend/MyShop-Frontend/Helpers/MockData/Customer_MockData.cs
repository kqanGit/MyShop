using MyShop_Frontend.Models;
using System;
using System.Collections.Generic;

namespace MyShop_Frontend.Helpers.MockData
{
    public static class Customer_MockData
    {
        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    CustomerId = 1,
                    FullName = "Nguyễn Văn Hùng",
                    Phone = "0901234567",
                    Address = "12 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội",
                    Point = 150,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 2,
                    FullName = "Trần Thị Mai",
                    Phone = "0912345678",
                    Address = "45 Lê Lợi, Quận 1, TP. Hồ Chí Minh",
                    Point = 50,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 3,
                    FullName = "Lê Quốc Anh",
                    Phone = "0987654321",
                    Address = "78 Nguyễn Văn Linh, Hải Châu, Đà Nẵng",
                    Point = 200,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 4,
                    FullName = "Phạm Thị Lan",
                    Phone = "0978123456",
                    Address = "25 Cách Mạng Tháng 8, Ninh Kiều, Cần Thơ",
                    Point = 10,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 5,
                    FullName = "Đặng Minh Tuấn",
                    Phone = "0909988776",
                    Address = "202 Võ Văn Tần, Quận 3, TP. Hồ Chí Minh",
                    Point = 120,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 6,
                    FullName = "Nguyễn Văn An",
                    Phone = "0901234567",
                    Address = "12 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội",
                    Point = 80,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 7,
                    FullName = "Trần Thị Bích",
                    Phone = "0912345678",
                    Address = "45 Lê Lợi, Quận 1, TP. Hồ Chí Minh",
                    Point = 30,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 8,
                    FullName = "Lê Hoàng Minh",
                    Phone = "0987654321",
                    Address = "78 Nguyễn Văn Linh, Hải Châu, Đà Nẵng",
                    Point = 160,
                    TierName = "Gold"
                },
                new Customer
                {
                    CustomerId = 9,
                    FullName = "Phạm Thị Lan",
                    Phone = "0978123456",
                    Address = "25 Cách Mạng Tháng 8, Ninh Kiều, Cần Thơ",
                    Point = 60,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 10,
                    FullName = "Đặng Quốc Huy",
                    Phone = "0909988776",
                    Address = "90 Võ Văn Tần, Quận 3, TP. Hồ Chí Minh",
                    Point = 210,
                    TierName = "Gold"
                },
                new Customer
                {
                    CustomerId = 11,
                    FullName = "Võ Thị Thu Hà",
                    Phone = "0934567890",
                    Address = "14 Phan Đình Phùng, Ba Đình, Hà Nội",
                    Point = 40,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 12,
                    FullName = "Bùi Văn Long",
                    Phone = "0961122334",
                    Address = "67 Nguyễn Trãi, Thanh Xuân, Hà Nội",
                    Point = 130,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 13,
                    FullName = "Ngô Thị Hạnh",
                    Phone = "0945566778",
                    Address = "102 Lý Thường Kiệt, Quận 10, TP. Hồ Chí Minh",
                    Point = 20,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 14,
                    FullName = "Phan Anh Tuấn",
                    Phone = "0923344556",
                    Address = "56 Điện Biên Phủ, Bình Thạnh, TP. Hồ Chí Minh",
                    Point = 170,
                    TierName = "Gold"
                },
                new Customer
                {
                    CustomerId = 15,
                    FullName = "Trịnh Thị Mai",
                    Phone = "0919988776",
                    Address = "9 Nguyễn Huệ, TP. Huế",
                    Point = 55,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 16,
                    FullName = "Hoàng Văn Nam",
                    Phone = "0905566778",
                    Address = "88 Trần Phú, TP. Nha Trang, Khánh Hòa",
                    Point = 190,
                    TierName = "Gold"
                },
                new Customer
                {
                    CustomerId = 17,
                    FullName = "Nguyễn Thị Thuý",
                    Phone = "0973344556",
                    Address = "34 Nguyễn Du, TP. Vinh, Nghệ An",
                    Point = 45,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 18,
                    FullName = "Lý Minh Khoa",
                    Phone = "0932211009",
                    Address = "120 Phạm Văn Đồng, Thủ Đức, TP. Hồ Chí Minh",
                    Point = 140,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 19,
                    FullName = "Đỗ Thị Ngọc",
                    Phone = "0912233445",
                    Address = "5 Lê Hồng Phong, TP. Hải Phòng",
                    Point = 25,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 20,
                    FullName = "Mai Quốc Khánh",
                    Phone = "0981122334",
                    Address = "77 Hoàng Diệu, TP. Đà Lạt, Lâm Đồng",
                    Point = 115,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 21,
                    FullName = "Nguyễn Văn Phúc",
                    Phone = "0906677889",
                    Address = "19 Nguyễn Văn Cừ, Long Biên, Hà Nội",
                    Point = 95,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 22,
                    FullName = "Trần Thị Kim Oanh",
                    Phone = "0948899776",
                    Address = "66 Lê Duẩn, TP. Buôn Ma Thuột, Đắk Lắk",
                    Point = 35,
                    TierName = "Bronze"
                },
                new Customer
                {
                    CustomerId = 23,
                    FullName = "Phạm Đức Thành",
                    Phone = "0967788990",
                    Address = "11 Nguyễn Thị Minh Khai, Quận 1, TP. Hồ Chí Minh",
                    Point = 220,
                    TierName = "Gold"
                },
                new Customer
                {
                    CustomerId = 24,
                    FullName = "Lê Thị Ánh",
                    Phone = "0934455667",
                    Address = "39 Hùng Vương, TP. Quảng Ngãi",
                    Point = 65,
                    TierName = "Silver"
                },
                new Customer
                {
                    CustomerId = 25,
                    FullName = "Nguyễn Quốc Bảo",
                    Phone = "0975566778",
                    Address = "150 Lạc Long Quân, Tây Hồ, Hà Nội",
                    Point = 180,
                    TierName = "Gold"
                }

            };
        }
    }
}
