using System;

namespace MyShop_Frontend.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Point { get; set; }
        public int TierId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
