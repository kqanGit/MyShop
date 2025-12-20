using System;
using System.Collections.Generic;

namespace MyShop.Domain.Entities;

public partial class Voucher
{
    public int VoucherId { get; set; }

    public string? VoucherCode { get; set; }

    public string? Description { get; set; }

    public int? Type { get; set; }

    public decimal? Discount { get; set; }

    public decimal? MinThreshold { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsRemoved { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
