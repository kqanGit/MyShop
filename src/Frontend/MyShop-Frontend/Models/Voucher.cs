using System;

namespace MyShop_Frontend.Models
{
    /// <summary>
    /// Voucher Entity - Pure domain model
    /// </summary>
    public class Voucher
    {
        public int VoucherId { get; set; }
        public string? VoucherCode { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; } // 1 = Percentage, 2 = Fixed Amount
        public decimal Discount { get; set; }
        public decimal MinThreshold { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsRemoved { get; set; }

        // Helper properties
        public bool IsActive => !IsRemoved.GetValueOrDefault() 
            && DateTime.Now >= StartDate 
            && DateTime.Now <= EndDate;

        public string TypeText => Type switch
        {
            1 => "Percentage",
            2 => "Fixed Amount",
            _ => "Unknown"
        };

        public string DiscountText => Type switch
        {
            1 => Discount.ToString() + "%",
            2 => Discount.ToString("N0") + " VND",
            _ => Discount.ToString()
        };
    }
}
