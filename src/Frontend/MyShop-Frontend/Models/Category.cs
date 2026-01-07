using System.Text.Json.Serialization;

namespace MyShop_Frontend.Models
{
    public sealed class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public override string ToString() => CategoryName;
    }
}
