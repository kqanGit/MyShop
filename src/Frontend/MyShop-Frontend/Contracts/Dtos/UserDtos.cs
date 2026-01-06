using System.Text.Json.Serialization;

namespace MyShop_Frontend.Contracts.Dtos
{
    // ===== User DTOs =====
    
    public class UserDto
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("roleId")]
        public int RoleId { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }

    public class CreateUserDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("roleId")]
        public int RoleId { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }

    public class UserConfigDto
    {
        [JsonPropertyName("perPage")]
        public int? PerPage { get; set; }

        [JsonPropertyName("lastModule")]
        public string? LastModule { get; set; }
    }
}
