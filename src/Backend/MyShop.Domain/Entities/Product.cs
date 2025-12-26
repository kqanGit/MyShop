using System;
using System.Collections;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int CategoryId { get; set; }

    public string? Unit { get; set; }

    public decimal Cost { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? Image { get; set; }

    public BitArray? IsRemoved { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
