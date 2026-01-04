using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Models
{
    public sealed class OrderDetailDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; } = string.Empty;

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("voucherCode")]
        public string? VoucherCode { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonPropertyName("discountAmount")]
        public decimal DiscountAmount { get; set; }

        [JsonPropertyName("finalPrice")]
        public decimal FinalPrice { get; set; }

        [JsonPropertyName("orderDetails")]
        public List<OrderDetailLineDto> OrderDetails { get; set; } = new();
    }

    public sealed class OrderDetailLineDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("priceAtPurchase")]
        public decimal PriceAtPurchase { get; set; }

        [JsonPropertyName("totalLine")]
        public decimal TotalLine { get; set; }
    }
}
