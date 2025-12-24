using System;
using System.Collections;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public int? RoleId { get; set; }

    public BitArray? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<UserConfig> UserConfigs { get; set; } = new List<UserConfig>();

    public string? PhoneNumber { get; set; }
}
