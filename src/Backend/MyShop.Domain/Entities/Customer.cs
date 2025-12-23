using System;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int Point { get; set; }

    public int? TierId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Membership? Tier { get; set; }
}
