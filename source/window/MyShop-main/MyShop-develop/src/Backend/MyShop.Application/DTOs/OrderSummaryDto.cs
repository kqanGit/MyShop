namespace MyShop.Application.DTOs.Order
{
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal FinalPrice { get; set; }
        public string StatusName { get; set; }
        public int TotalItems { get; set; }
    }
}