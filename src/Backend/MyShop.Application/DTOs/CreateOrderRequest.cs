using System.Collections.Generic;

namespace MyShop.Application.DTOs
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }     
        public string? VoucherCode { get; set; } 
        public string? Note { get; set; }

        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderResultDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public decimal FinalPrice { get; set; }
        public int EarnedPoints { get; set; }
        public string? Message { get; set; }
    }
}