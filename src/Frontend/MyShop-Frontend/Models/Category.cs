using System.Text.Json.Serialization;

namespace MyShop_Frontend.Models
{
    public sealed class Category
    {
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; } = string.Empty;

        public override string ToString() => CategoryName;
    }
}
