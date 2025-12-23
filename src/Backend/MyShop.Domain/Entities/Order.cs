using System;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderCode { get; set; }

    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public int? VoucherId { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal FinalPrice { get; set; }

    public int Status { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User User { get; set; }

    public virtual Voucher? Voucher { get; set; }
}
