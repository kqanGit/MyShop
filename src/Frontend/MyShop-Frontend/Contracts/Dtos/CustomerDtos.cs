using System;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Contracts.Dtos
{
    // ===== Customer DTOs =====
    
    public class CustomerDto
    {
        [JsonPropertyName("customerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("point")]
        public int Point { get; set; }

        [JsonPropertyName("tierName")]
        public string TierName { get; set; } = string.Empty;
    }

    public class CustomerDetailDto : CustomerDto
    {
        [JsonPropertyName("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonPropertyName("membership")]
        public MembershipDto? Membership { get; set; }
    }

    public class CreateCustomerDto
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
    }

    public class UpdateCustomerDto
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
    }

    public class MembershipDto
    {
        [JsonPropertyName("tierId")]
        public int TierId { get; set; }

        [JsonPropertyName("tierName")]
        public string TierName { get; set; } = string.Empty;

        [JsonPropertyName("threshold")]
        public int? Threshold { get; set; }

        [JsonPropertyName("discount")]
        public double? Discount { get; set; }
    }
}
