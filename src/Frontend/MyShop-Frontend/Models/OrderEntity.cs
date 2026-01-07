using System;
using System.Collections.Generic;

namespace MyShop_Frontend.Models
{
    /// <summary>
    /// Complete Order Entity - Pure domain model matching database schema
    /// </summary>
    public class OrderEntity
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int? VoucherId { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public int Status { get; set; } // 1=New, 2=Delivering, 3=Completed

        // Navigation properties
        public Customer? Customer { get; set; }
        public User? User { get; set; }
        public Voucher? Voucher { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new();

        // Helper properties
        public string StatusText => Status switch
        {
            1 => "New",
            2 => "Paid",
            3 => "Canceled",
            _ => "Unknown"
        };

        public int TotalItems => OrderDetails.Count;
    }
}
