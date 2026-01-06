using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.DTOs
{
    public class VoucherDto
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public decimal Discount { get; set; }
        public decimal MinThreshold { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsRemoved { get; set; }
    }

    public class CreateVoucherDto
    {
        public string VoucherCode { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public decimal Discount { get; set; }
        public decimal MinThreshold { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CheckVoucherDto
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public decimal Discount { get; set; }
        public decimal MinThreshold { get; set; }
        public bool IsValid { get; set; }
    }
}
