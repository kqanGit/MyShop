using System;
using System.Collections.Generic;

namespace MyShop.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } 

        public string? VoucherCode { get; set; }
        public decimal TotalPrice { get; set; }      
        public decimal DiscountAmount { get; set; }  
        public decimal FinalPrice { get; set; }      


        public List<OrderDetailResponseDto> OrderDetails { get; set; } = new List<OrderDetailResponseDto>();
    }

    public class OrderDetailResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; } 
        public decimal TotalLine { get; set; }       
    }
}