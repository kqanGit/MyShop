using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Contracts.Dtos
{
    // ===== Order DTOs =====
    
    public class CreateOrderRequest
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; } = 1;

        [JsonPropertyName("voucherCode")]
        public string? VoucherCode { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; } = 1; // 1=New, 2=Paid, 3=Canceled

        [JsonPropertyName("items")]
        public List<CartItemDto> Items { get; set; } = new();
    }

    public class CartItemDto
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class OrderResultDto
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

    public class OrderSummaryDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; } = string.Empty;

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("finalPrice")]
        public decimal FinalPrice { get; set; }

        [JsonPropertyName("statusName")]
        public string StatusName { get; set; } = string.Empty;

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }
    }

    public class OrderResponseDto
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
        public List<OrderDetailResponseDto> OrderDetails { get; set; } = new();
    }

    public class OrderDetailResponseDto
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

    public class GetOrdersRequest
    {
        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; } = 1;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 10;

        [JsonPropertyName("fromDate")]
        public DateTime? FromDate { get; set; }

        [JsonPropertyName("toDate")]
        public DateTime? ToDate { get; set; }

        [JsonPropertyName("searchTerm")]
        public string? SearchTerm { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}
