using System.Text.Json.Serialization;

namespace MyShop.Application.DTOs
{
    public class LogoutRequestDto
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
