using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Models
{
    public class Order
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; }

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("finalPrice")]
        public decimal FinalPrice { get; set; }

        [JsonPropertyName("statusName")]
        public string StatusName { get; set; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }
    }

    public class OrderResponse
    {
        [JsonPropertyName("items")]
        public List<Order> Items { get; set; }

        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
    }
}
