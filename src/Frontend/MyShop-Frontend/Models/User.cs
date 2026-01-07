using System;

namespace MyShop_Frontend.Models
{
    /// <summary>
    /// User Entity - Pure domain model without JSON serialization attributes
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public int? RoleId { get; set; }
        public bool Status { get; set; } = true;
        public string? PhoneNumber { get; set; }

        // Navigation property
        public Role? Role { get; set; }
    }
}
