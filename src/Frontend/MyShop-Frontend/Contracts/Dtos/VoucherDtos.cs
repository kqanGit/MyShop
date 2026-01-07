using System;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Contracts.Dtos
{
    // ===== Voucher DTOs =====
    
    public class VoucherDto
    {
        [JsonPropertyName("voucherId")]
        public int VoucherId { get; set; }

        [JsonPropertyName("voucherCode")]
        public string VoucherCode { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; } // 1 = Percentage, 2 = Fixed Amount

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("minThreshold")]
        public decimal MinThreshold { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("isRemoved")]
        public bool? IsRemoved { get; set; }
    }

    public class CreateVoucherDto
    {
        [JsonPropertyName("voucherCode")]
        public string VoucherCode { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("minThreshold")]
        public decimal MinThreshold { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
    }

    public class VoucherCheckResponseDto
    {
        [JsonPropertyName("isValid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("voucher")]
        public VoucherDto? Voucher { get; set; }
    }
}
