using System;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal CurrentPrice { get; set; }

    public decimal CurrentCost { get; set; }

    public decimal TotalLine { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
