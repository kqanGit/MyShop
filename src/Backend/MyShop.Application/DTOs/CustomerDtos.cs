using System;

namespace MyShop.Application.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Point { get; set; }
        public string TierName { get; set; }
    }

    public class CreateCustomerDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class UpdateCustomerDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class CustomerDetailDto : CustomerDto
    {
        public DateTime CreateDate { get; set; }
        public MembershipDto Membership { get; set; }
    }

    public class MembershipDto
    {
        public int TierId { get; set; }
        public string TierName { get; set; }
        public int? Threshold { get; set; }
        public double? Discount { get; set; }

    }
}
