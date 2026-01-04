using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Models
{
    public sealed class CreateOrderRequest
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; } = 1;

        [JsonPropertyName("voucherCode")]
        public string? VoucherCode { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("items")]
        public List<CartItemDto> Items { get; set; } = new();
    }

    public sealed class CartItemDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public sealed class OrderResultDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; } = string.Empty;

        [JsonPropertyName("finalPrice")]
        public decimal FinalPrice { get; set; }

        [JsonPropertyName("earnedPoints")]
        public int EarnedPoints { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
