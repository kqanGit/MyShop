using System;
using System.Text.Json.Serialization;

namespace MyShop_Frontend.Contracts.Dtos
{
    // ===== Authentication DTOs =====
    
    public class LoginRequestDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequestDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }

    public class LogoutRequestDto
    {
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
