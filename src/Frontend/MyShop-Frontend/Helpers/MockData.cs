using MyShop_Frontend.Models;
using System;
using System.Collections.Generic;

namespace MyShop_Frontend.Helpers
{
    public static class MockData
    {
        public static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    CustomerId = "C001",
                    FullName = "John Smith",
                    Phone = "+1 (555) 123-4567",
                    Address = "123 Tech Park, NY",
                    Point = 150,
                    TierId = 1,
                    CreateDate = DateTime.Now.AddMonths(-2)
                },
                new Customer
                {
                    CustomerId = "C002",
                    FullName = "Sarah Johnson",
                    Phone = "+1 (555) 987-6543",
                    Address = "456 Design Blvd, CA",
                    Point = 50,
                    TierId = 2,
                    CreateDate = DateTime.Now.AddDays(-10)
                },
                new Customer
                {
                    CustomerId = "C003",
                    FullName = "Mike Davis",
                    Phone = "+1 (555) 456-7890",
                    Address = "789 Startup Ave, TX",
                    Point = 200,
                    TierId = 1,
                    CreateDate = DateTime.Now.AddMonths(-1)
                },
                new Customer
                {
                    CustomerId = "C004",
                    FullName = "Emily Brown",
                    Phone = "+1 (555) 321-0987",
                    Address = "101 Freelance Ln, FL",
                    Point = 10,
                    TierId = 3,
                    CreateDate = DateTime.Now.AddDays(-5)
                },
                new Customer
                {
                    CustomerId = "C005",
                    FullName = "David Wilson",
                    Phone = "+1 (555) 654-3210",
                    Address = "202 Marketing St, WA",
                    Point = 120,
                    TierId = 2,
                    CreateDate = DateTime.Now.AddMonths(-3)
                }
            };
        }
    }
}
