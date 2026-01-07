using System.Text.Json.Serialization;

namespace MyShop_Frontend.Models
{
    /// <summary>
    /// UserConfig Entity - Pure domain model
    /// </summary>
    public class UserConfig
    {
        public int SettingId { get; set; }
        public int? UserId { get; set; }
        public int? PerPage { get; set; }
        public string? LastModule { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}
