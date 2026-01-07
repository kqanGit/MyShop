using System.Text.Json.Serialization;

namespace MyShop_Frontend.Contracts.Dtos
{
    public class UserConfigClientDto
    {
        [JsonPropertyName("perPage")]
        public int PerPage { get; set; }

        [JsonPropertyName("lastModule")]
        public string? LastModule { get; set; }
    }
}
