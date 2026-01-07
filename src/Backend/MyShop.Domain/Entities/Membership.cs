using System;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class Membership
{
    public int TierId { get; set; }

    public string? TierName { get; set; }

    public int? Threshold { get; set; }

    public double? Discount { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
