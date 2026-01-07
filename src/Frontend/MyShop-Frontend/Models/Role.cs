namespace MyShop_Frontend.Models
{
    /// <summary>
    /// Role Entity - Pure domain model
    /// </summary>
    public class Role
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public override string ToString() => RoleName ?? $"Role {RoleId}";
    }
}
